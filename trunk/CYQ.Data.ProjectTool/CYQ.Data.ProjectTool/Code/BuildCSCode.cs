using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CYQ.Data.Table;
using CYQ.Data.SQL;

namespace CYQ.Data.ProjectTool
{
    class BuildCSCode
    {
        internal delegate void CreateEndHandle(int count);
        internal static event CreateEndHandle OnCreateEnd;
        internal static void Create(object nameObj)
        {
            int count = 0;
            try
            {
                string name = Convert.ToString(nameObj);

                using (ProjectConfig config = new ProjectConfig())
                {
                    try
                    {
                        if (config.Fill("Name='" + name + "'"))
                        {
                            string dbName = string.Empty;
                            Dictionary<string, string> tables = Tool.DBTool.GetTables(config.Conn, out dbName);
                            if (tables != null && tables.Count > 0)
                            {
                                dbName = dbName[0].ToString().ToUpper() + dbName.Substring(1, dbName.Length - 1);
                                count = tables.Count;
                                if (config.BuildMode.Contains("枚举"))//枚举型。
                                {
                                    BuildTableEnumText(tables, config, dbName);

                                }
                                else
                                {
                                    BuildTableEntityText(tables, config, dbName);
                                }
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        Log.WriteLogToTxt(err);
                    }
                }
            }
            finally
            {
                if (OnCreateEnd != null)
                {
                    OnCreateEnd(count);
                }
            }
            //MessageBox.Show(string.Format("执行完成!共计{0}个表", count));
        }

        #region 枚举型的单文件

        static void BuildTableEnumText(Dictionary<string, string> tables, ProjectConfig config, string dbName)
        {
            try
            {
                StringBuilder tableEnum = new StringBuilder();
                string nameSpace = string.Format(config.NameSpace, dbName).TrimEnd('.');
                tableEnum.AppendFormat("using System;\r\n\r\nnamespace {0}\r\n{{\r\n", nameSpace);
                //tableEnum.AppendFormat("using System;\r\n\r\nnamespace {0}\r\n{{\r\n", ((string.IsNullOrEmpty(dbName) || dbName.Contains(":")) ? config.NameSpace : config.NameSpace + "." + dbName));
                tableEnum.Append(config.MutilDatabase ? string.Format("    public enum {0}Enum {{", dbName) : "    public enum TableNames {");
                foreach (KeyValuePair<string, string> table in tables)
                {
                    tableEnum.Append(" " + table.Key + " ,");
                }
                tableEnum[tableEnum.Length - 1] = '}';//最后一个字符变成换大括号。

                tableEnum.Append("\r\n\r\n    #region 枚举 \r\n");

                foreach (KeyValuePair<string, string> table in tables)
                {
                    tableEnum.Append(GetFiledEnum(table.Key, config));
                }
                tableEnum.Append("    #endregion\r\n}");
                string fileName = "TableNames.cs";
                if (config.MutilDatabase)//多数据库模式。
                {
                    fileName = dbName + "Enum.cs";
                }
                System.IO.File.WriteAllText(config.ProjectPath.TrimEnd('/', '\\') + "\\" + fileName, tableEnum.ToString(), Encoding.Default);
            }
            catch (Exception err)
            {
                Log.WriteLogToTxt(err);
            }
        }
        static string GetFiledEnum(string tableName, ProjectConfig config)
        {
            string tableColumnNames = "    public enum " + tableName + " {";
            try
            {
                MDataColumn column = CYQ.Data.Tool.DBTool.GetColumns(tableName, config.Conn);

                if (column.Count > 0)
                {
                    for (int i = 0; i < column.Count; i++)
                    {
                        tableColumnNames += " " + column[i].ColumnName + " ,";
                    }
                    tableColumnNames = tableColumnNames.TrimEnd(',') + "}\r\n";
                }
                else
                {
                    tableColumnNames = tableColumnNames + "}\r\n";
                }
            }
            catch (Exception err)
            {
                Log.WriteLogToTxt(err);
            }
            return tableColumnNames;
        }

        #endregion

        #region 实体型的多文件
        static void BuildTableEntityText(Dictionary<string, string> tables, ProjectConfig config, string dbName)
        {
            foreach (KeyValuePair<string, string> table in tables)
            {
                BuildSingTableEntityText(table.Key, table.Value, config, dbName);
            }
        }
        static void BuildSingTableEntityText(string tableName, string description, ProjectConfig config, string dbName)
        {
            string fixTableName = tableName;
            if (config.MapName) { fixTableName = FixName(tableName); }
            bool onlyEntity = config.BuildMode.StartsWith("纯");//纯实体。
            try
            {
                StringBuilder csText = new StringBuilder();
                string nameSpace = string.Format(config.NameSpace, dbName).TrimEnd('.');

                // ((string.IsNullOrEmpty(dbName) || dbName.Contains(":")) ? config.NameSpace : config.NameSpace + "." + dbName);
                AppendText(csText, "using System;\r\n");
                AppendText(csText, "namespace {0}", nameSpace);
                AppendText(csText, "{");
                if (!string.IsNullOrEmpty(description))
                {
                    AppendText(csText, "    /// <summary>");
                    AppendText(csText, "    /// {0}", description);
                    AppendText(csText, "    /// </summary>");
                }
                AppendText(csText, "    public class {0} {1}", fixTableName + config.EntitySuffix, onlyEntity ? "" : ": CYQ.Data.Orm.OrmBase");
                AppendText(csText, "    {");
                if (!onlyEntity)
                {
                    AppendText(csText, "        public {0}()", fixTableName + config.EntitySuffix);
                    AppendText(csText, "        {");
                    AppendText(csText, "            base.SetInit(this, \"{0}\", \"{1}\");", fixTableName, config.Name);
                    AppendText(csText, "        }");
                }

                #region 循环字段
                MDataColumn column = CYQ.Data.Tool.DBTool.GetColumns(tableName, config.Conn);

                if (column.Count > 0)
                {
                    string columnName = string.Empty;
                    Type t = null;
                    if (config.ForTwoOnly)
                    {
                        foreach (MCellStruct st in column)
                        {
                            columnName=st.ColumnName;
                            if (config.MapName) { columnName = FixName(columnName); }
                            t = DataType.GetType(st.SqlType);
                            AppendText(csText, "        private {0} _{1};", FormatType(t.Name, t.IsValueType, config.ValueTypeNullable), columnName);
                            if (!string.IsNullOrEmpty(st.Description))
                            {
                                AppendText(csText, "        /// <summary>");
                                AppendText(csText, "        /// {0}", st.Description);
                                AppendText(csText, "        /// </summary>");
                            }
                            AppendText(csText, "        public {0} {1}", FormatType(t.Name, t.IsValueType, config.ValueTypeNullable), columnName);
                            AppendText(csText, "        {");
                            AppendText(csText, "            get");
                            AppendText(csText, "            {");
                            AppendText(csText, "                return _{0};", columnName);
                            AppendText(csText, "            }");
                            AppendText(csText, "            set");
                            AppendText(csText, "            {");
                            AppendText(csText, "                _{0} = value;", columnName);
                            AppendText(csText, "            }");
                            AppendText(csText, "        }");
                        }
                    }
                    else
                    {
                        foreach (MCellStruct st in column)
                        {
                            columnName = st.ColumnName;
                            if (config.MapName) { columnName = FixName(columnName); }
                            t = DataType.GetType(st.SqlType);
                            if (!string.IsNullOrEmpty(st.Description))
                            {
                                AppendText(csText, "        /// <summary>");
                                AppendText(csText, "        /// {0}", st.Description);
                                AppendText(csText, "        /// </summary>");
                            }
                            AppendText(csText, "        public {0} {1} {{ get; set; }}", FormatType(t.Name, t.IsValueType, config.ValueTypeNullable), columnName);
                        }
                    }
                }
                #endregion

                AppendText(csText, "    }");
                AppendText(csText, "}");
                string pPath = config.ProjectPath;
                System.IO.File.WriteAllText(pPath.TrimEnd('/', '\\') + "\\" + fixTableName + ".cs", csText.ToString(), Encoding.Default);
            }
            catch (Exception err)
            {
                Log.WriteLogToTxt(err);
            }
        }
        static void AppendText(StringBuilder sb, string text, params string[] format)
        {
            if (text.IndexOf("{0}") > -1)
            {
                sb.AppendFormat(text + "\r\n", format);
            }
            else
            {
                sb.AppendLine(text);
            }
        }
        #endregion
        static string FormatType(string tName, bool isValueType, bool nullable)
        {
            switch (tName)
            {
                case "Int32":
                    tName = "int";
                    break;
                case "String":
                    tName = "string";
                    nullable = false;
                    break;
                case "Boolean":
                    tName = "bool";
                    break;
            }
            if (nullable && isValueType && !tName.EndsWith("[]"))
            {
                tName += "?";
            }
            return tName;
        }
        internal static string FixName(string name)
        {

            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToLower();
                if (name == "id") { return "ID"; }
                bool isEndWithID = name.EndsWith("id");
                string[] items = name.Split('_');
                name = string.Empty;
                foreach (string item in items)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        if (item.Length == 1)
                        {
                            name += item.ToUpper();
                        }
                        else
                        {
                            name += item[0].ToString().ToUpper() + item.Substring(1, item.Length - 1);
                        }
                    }
                }
                if (isEndWithID)
                {
                    name = name.Substring(0, name.Length - 2) + "ID";
                }
            }
            return name;
        }
    }
}
