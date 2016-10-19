using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Windows.Forms;
using System.Drawing;
using CYQ.Data.Table;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(CYQ.Visualizer.DataRowVisualizer),
typeof(VisualizerObjectSource),
Target = typeof(System.Data.DataRow),
Description = "DataRow Visualizer")]


namespace CYQ.Visualizer
{
    /// <summary>
    /// DataRow未标记序列化，所以不支持
    /// </summary>
    public class DataRowVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            MDataRow row = objectProvider.GetObject() as DataRow;
            string title = string.Format("TableName : {0}    Columns： {1}", row.TableName, row.Columns.Count);
            Form form = FormCreate.CreateForm(title);
            DataGridView dg = FormCreate.CreateGrid(form);
            try
            {
                MDataTable dt = row.ToTable();
                MCellStruct ms = new MCellStruct("[No.]", System.Data.SqlDbType.Int);
                dt.Columns.Insert(0, ms);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i].Set(0, i + 1);
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
