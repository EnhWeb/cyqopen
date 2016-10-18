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

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(CYQ.Visualizer.DataRowVisualizer),
typeof(VisualizerObjectSource),
Target = typeof(System.Data.DataRow),
Description = "DataRow Visualizer")]

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
            string title= string.Format("TableName : {0}    Rows£∫ {1}    Columns£∫ {2}", dt.TableName, dt.Rows.Count, dt.Columns.Count);
            Form form = FormCreate.CreateForm(title);
            DataGridView dg = FormCreate.CreateGrid(form);
            try
            {
                if (dt.Rows.Count > 200)
                {
                    dt = dt.Select(200, null);
                }
                //≤Â»Î––∫≈
                dt.Columns.Insert(0, new MCellStruct("[No.]", System.Data.SqlDbType.Int));
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
            string title = string.Format("TableName : {0}    Columns£∫ {1}", row.TableName, row.Columns.Count);
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
    public class MDataColumnVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            MDataColumn mdc = objectProvider.GetObject() as MDataColumn;
            string title = string.Format("Columns£∫ {0}", mdc.Count);
            Form form = FormCreate.CreateForm(title);
            DataGridView dg = FormCreate.CreateGrid(form);
            try
            {

                MDataTable dt = mdc.ToTable();
                MCellStruct ms=new MCellStruct("[No.]", System.Data.SqlDbType.Int);
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
    public class DataRowVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            MDataRow row = objectProvider.GetObject() as DataRow;
            string title = string.Format("TableName : {0}    Columns£∫ {1}", row.TableName, row.Columns.Count);
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
    internal class FormCreate
    {
        public static Form CreateForm(string title)
        {
            Form form = new Form();
            form.Text = title;
            form.ClientSize = new Size(600, 400);
            form.FormBorderStyle = FormBorderStyle.Sizable;
            form.HorizontalScroll.Enabled = true;
            form.VerticalScroll.Enabled = true;
            return form;
        }
        public static DataGridView CreateGrid(Form parent)
        {
            DataGridView dg = new DataGridView();
            dg.Parent = parent;
            dg.ReadOnly = true;
            dg.Dock = DockStyle.Fill;
            dg.ScrollBars = ScrollBars.Both;
            dg.AutoSize = true;
            dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            return dg;
        }
    }
}
