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
}
