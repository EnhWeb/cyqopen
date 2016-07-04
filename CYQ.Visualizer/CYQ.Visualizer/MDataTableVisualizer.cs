using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Windows.Forms;
using System.Drawing;
//using TY.Data.Table;
using CYQ.Data.Table;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(CYQ.Visualizer.MDataTableVisualizer),
typeof(VisualizerObjectSource),
Target = typeof(CYQ.Data.Table.MDataTable),
Description = "MDataTable Visualizer")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(CYQ.Visualizer.MDataRowVisualizer),
typeof(VisualizerObjectSource),
Target = typeof(CYQ.Data.Table.MDataRow),
Description = "MDataRow Visualizer")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(CYQ.Visualizer.MDataColumnVisualizer),
typeof(VisualizerObjectSource),
Target = typeof(CYQ.Data.Table.MDataColumn),
Description = "MDataColumn Visualizer")]

//[assembly: System.Diagnostics.DebuggerVisualizer(
//typeof(CYQ.Visualizer.MDataTableVisualizer),
//typeof(VisualizerObjectSource),
//Target = typeof(TY.Data.Table.MDataTable),
//Description = "MDataTable Visualizer")]

namespace CYQ.Visualizer
{
  
    public class MDataTableVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            MDataTable dt = objectProvider.GetObject() as MDataTable;
            Form form = new Form();
            form.Text = string.Format("表名 : {0}    行数： {1}    列数： {2}", dt.TableName, dt.Rows.Count, dt.Columns.Count);
            form.ClientSize = new Size(600, 400);
            form.FormBorderStyle = FormBorderStyle.Sizable;

            DataGridView dg = new DataGridView();
            dg.Parent = form;
            dg.ReadOnly = true;
            dg.Dock = DockStyle.Fill;
            dg.ScrollBars = ScrollBars.Both;
            dg.AutoSize = true;
            dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //dg.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.;
            try
            {
                dt = dt.Select(10000, null);
                //插入行号
                dt.Columns.Insert(0, new MCellStruct("[系统行号]", System.Data.SqlDbType.Int));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i][0].Value = i + 1;
                }
                dt.Bind(dg);

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            windowService.ShowDialog(form);
        }
    }
    public class MDataRowVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            MDataRow row = objectProvider.GetObject() as MDataRow;
            Form form = new Form();
            form.Text = string.Format("表名 : {0}    列数： {1}", row.TableName, row.Columns.Count);
            form.ClientSize = new Size(600, 400);
            form.FormBorderStyle = FormBorderStyle.Sizable;

            DataGridView dg = new DataGridView();
            dg.Parent = form;
            dg.ReadOnly = true;
            dg.Dock = DockStyle.Fill;
            dg.ScrollBars = ScrollBars.Both;
            dg.AutoSize = true;
            dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //dg.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.;
            try
            {
                MDataTable dt = new MDataTable(row.TableName);
                dt.Columns.Add("系统行号", System.Data.SqlDbType.Int);
                dt.Columns.Add("列名", System.Data.SqlDbType.NVarChar);
                dt.Columns.Add("值", System.Data.SqlDbType.NVarChar);
                for (int i = 0; i < row.Count; i++)
                {
                    dt.NewRow(true).Set(0, i + 1).Set(1, row[i].ColumnName).Set(2, row[i].ToString());
                }
                dt.Bind(dg);

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            windowService.ShowDialog(form);
        }
    }
    public class MDataColumnVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            MDataColumn mdc = objectProvider.GetObject() as MDataColumn;
            Form form = new Form();
            form.Text = string.Format("表结构列数： {0}", mdc.Count);
            form.ClientSize = new Size(600, 400);
            form.FormBorderStyle = FormBorderStyle.Sizable;

            DataGridView dg = new DataGridView();
            dg.Parent = form;
            dg.ReadOnly = true;
            dg.Dock = DockStyle.Fill;
            dg.AutoSize = true;
            dg.ScrollBars = ScrollBars.Both;
            dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //dg.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.;
            try
            {
                MDataTable dt = new MDataTable();
                dt.Columns.Add("系统行号", System.Data.SqlDbType.Int);
                dt.Columns.Add("ColumnName");
                dt.Columns.Add("MaxSize");
                dt.Columns.Add("Scale");
                dt.Columns.Add("IsCanNull");
                dt.Columns.Add("IsAutoIncrement");
                dt.Columns.Add("SqlType");
                dt.Columns.Add("IsPrimaryKey");
                dt.Columns.Add("DefaultValue");
                dt.Columns.Add("Description");


                for (int i = 0; i < mdc.Count; i++)
                {
                    MCellStruct ms=mdc[i];
                    dt.NewRow(true).Set(0, i + 1)
                        .Set(1, ms.ColumnName)
                        .Set(2, ms.MaxSize)
                        .Set(3, ms.Scale)
                        .Set(4, ms.IsCanNull)
                        .Set(5, ms.IsAutoIncrement)
                        .Set(6, ms.SqlType)
                        .Set(7, ms.IsPrimaryKey)
                        .Set(8, ms.DefaultValue)
                        .Set(9, ms.Description);
                }
               
                dt.Bind(dg);

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            windowService.ShowDialog(form);
        }
    }
}
