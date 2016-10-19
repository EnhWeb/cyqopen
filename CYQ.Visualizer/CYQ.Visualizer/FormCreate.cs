using CYQ.Data.Table;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CYQ.Visualizer
{
    internal static class FormCreate
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
        public static void BindTable(IDialogVisualizerService windowService, MDataTable dt, string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                title = string.Format("TableName : {0}    Rows： {1}    Columns： {2}", dt.TableName, dt.Rows.Count, dt.Columns.Count);
            }
            Form form = FormCreate.CreateForm(title);
            DataGridView dg = FormCreate.CreateGrid(form);
            try
            {
                if (dt.Rows.Count > 200)
                {
                    dt = dt.Select(200, null);
                }
                //插入行号
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
}
