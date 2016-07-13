using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CYQ.Data;
using CYQ.Data.Table;
namespace MAction_Demo
{
    public partial class 单表操作 : Form
    {
        string tableName = "Users";
        public 单表操作()
        {
            AppConfig.DB.EditTimeFields = "EditTime";//该配置的字段，在更新时会自动被更新时间。
            InitializeComponent();
            Pager.OnPageChanged += Pager_OnPageChanged;
        }

        void Pager_OnPageChanged(object sender, EventArgs e)
        {
            LoadData();
        }



        private void 单表操作_Load(object sender, EventArgs e)
        {
            LoadData();

        }
        private void LoadData()
        {
            MDataTable dt;
            using (MAction action = new MAction(tableName))
            {
                dt = action.Select(Pager.PageIndex, Pager.PageSize, "order by " + action.Data.PrimaryCell.ColumnName + " desc");
                OutDebugSql(action.DebugInfo);
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                if (txtUserID.Text == "")
                {
                    dt.Rows[0].SetToAll(this);
                }
            }
            dt.Bind(dgView);
            Pager.DrawControl(dt.RecordsAffected);
        }

        private void OutDebugSql(string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                msg = "Auto Cache...";//相关的配置，如：AppConfig.Cache.IsAutoCache = false;
            }
            rtxtSql.Text = msg;
        }

        private void btnFill_Click(object sender, EventArgs e)
        {
            using (MAction action = new MAction(tableName))
            {
                if (action.Fill(txtUserID))
                {
                    action.UI.SetToAll(this);
                    OutDebugSql(action.DebugInfo);
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            using (MAction action = new MAction(tableName))
            {
                if (!action.Exists(txtName))
                {
                    action.AllowInsertID = chbInsertID.Checked;
                    action.UI.SetAutoParentControl(this);//Web开发的不需要这条
                    if (action.Insert(true, InsertOp.ID))
                    {
                        action.UI.SetToAll(this);
                        LoadData();
                    }
                }
                OutDebugSql(action.DebugInfo);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (MAction action = new MAction(tableName))
            {
                action.UI.SetAutoParentControl(this);
                if (action.Update(true))
                {
                    LoadData();
                }
                OutDebugSql(action.DebugInfo);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (MAction action = new MAction(tableName))
            {
                if (action.Delete(txtUserID))
                {
                    LoadData();
                }
                OutDebugSql(action.DebugInfo);
            }
        }

        private void btnNoDelete_Click(object sender, EventArgs e)
        {
            AppConfig.DB.DeleteField = "IsDeleted";//演示用代码，一般配置在config对全局起使用。
            btnDelete_Click(sender, e);
            AppConfig.DB.DeleteField = "";
        }



        private void btn_Click(object sender, EventArgs e)
        {
            using (MAction action = new MAction(tableName))
            {
                action.Exists(txtUserID);
                action.Exists(txtName);//自动推导
                OutDebugSql(action.DebugInfo);
            }
        }
        private void btnOpenMutipleTable_Click(object sender, EventArgs e)
        {
            多表查询 m = new 多表查询();
            m.Show();
        }
        private void btnMutipleOperator_Click(object sender, EventArgs e)
        {
            多表操作 m = new 多表操作();
            m.Show();
        }
    }
}
