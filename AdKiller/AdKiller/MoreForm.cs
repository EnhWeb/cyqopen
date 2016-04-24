using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace AdKiller
{
    public partial class MoreForm : Form
    {
        public MoreForm()
        {
            InitializeComponent();
        }
        JsonHelper Json = null;
        ShareHostNet _ShareNet = null;
        ShareHostNet ShareNet
        {
            get
            {
                if (_ShareNet == null)
                {
                    _ShareNet = new ShareHostNet();
                    Json = new JsonHelper();
                }
                return _ShareNet;
            }
        }
        #region 添加Hosts
        string host, toIP, title, author;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                //添加到Host文件
                if (ShareHost.AddHost(host, toIP))
                {
                    if (chbShare.Checked)
                    {
                        //提交到服务器。
                        chbShare.Enabled = false;
                        Thread thread = new Thread(PostShare);
                        thread.IsBackground = true;
                        thread.Start();
                    }
                    txtHost.Text = "";
                    MessageBox.Show("添加成功!", ST.MsgTitle);
                }
            }
        }
        bool Check()
        {
            host = txtHost.Text.Trim();
            toIP = txtToIP.Text.Trim();
            title = txtTitle.Text.Trim();
            author = txtAuthor.Text.Trim();
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(toIP) ||
                (chbShare.Checked && (string.IsNullOrEmpty(title) || title.StartsWith("格式") || string.IsNullOrEmpty(author))))
            {
                MessageBox.Show("请填写必填项!", ST.MsgTitle);
                return false;
            }
            if (host.IndexOf('.') == host.LastIndexOf('.') || host.Split(' ').Length > 1)
            {
                MessageBox.Show("Host格式不正确（而且至少为二级域名）!", ST.MsgTitle);
                return false;
            }
            if (!string.IsNullOrEmpty(author) && !author.StartsWith("killer_"))
            {
                MessageBox.Show("Code错误，进QQ群（227664757）申请分享Code才能进行分享，!", ST.MsgTitle);
                return false;
            }
            return true;
        }

        #region Post到服务端
        void PostShare()
        {
            ShareNet.Post(host, toIP, title,author);
            Thread.Sleep(TimeSpan.FromMinutes(1));
            chbShare.Enabled = true;
        }
        #endregion
        #endregion





        #region 读取数据

        int pageIndex = 1, flag = 0;
        bool isHidden = true;
        private void btnGetShareHost_Click(object sender, EventArgs e)
        {
            lnkNext.Enabled = pageIndex > 0;
            lnkPre.Enabled = pageIndex > 1;
            if (pageIndex < 1)
            {
                pageIndex = 1;
                MessageBox.Show("已经是首页了！", ST.MsgTitle);
                return;
            }
            flag = rbtnAsc.Checked ? 0 : 1;
            btnGetShareHost.Enabled = false;
            isHidden = chbHidden.Checked;
            Thread thread = new Thread(new ThreadStart(GetShareHost));
            thread.IsBackground = true;
            thread.Start();
            lbTip.Text = "正在读取...";
        }
        void GetShareHost()
        {
            try
            {
                bool isEnd = true;
                string result = ShareNet.Get(pageIndex, flag);
                if (!string.IsNullOrEmpty(result) && result.StartsWith("{") && result.EndsWith("}"))
                {
                    DataTable dt = Json.Load(result);
                    isEnd = dt == null || dt.Rows.Count == 0;
                    if (!isEnd)
                    {
                        if (dt.Rows.Count < 20)
                        {
                            lnkNext.Enabled = false;
                        }
                        this.Invoke(new SetHandle(ThreadBindControl), new object[] { dt });
                    }
                }
                if (isEnd)
                {
                    MessageBox.Show("查不到数据了！", ST.MsgTitle);
                    lnkNext.Enabled = false;
                    pageIndex--;
                }
            }
            finally
            {
                lbTip.Text = "加载完成！";
                btnGetShareHost.Enabled = true;
            }


        }
        private void lnkPre_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pageIndex--;
            btnGetShareHost_Click(null, null);
        }

        private void lnkNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pageIndex++;
            btnGetShareHost_Click(null, null);
        }


        protected delegate void SetHandle(DataTable dt);
        protected void ThreadBindControl(DataTable dt)
        {
            //处理隐藏性
            dt = FilterHost(dt);
            if (dt.Rows.Count > 0)
            {
                gvShareList.DataSource = dt;
                if (gvShareList.Columns.Count > 0)
                {
                    gvShareList.Columns[0].Width = 60;
                }
            }
            else
            {
                MessageBox.Show("本页的数据已全部设置过了，全部隐藏了！", ST.MsgTitle);
            }
        }
        DataTable FilterHost(DataTable dt)
        {
            if (isHidden)
            {
                string hostText = WebHost.HostText;
                string host = string.Empty;
                if (dt.Columns["Host"] != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        host = Convert.ToString(dt.Rows[i]["Host"]);
                        if (hostText.Contains(ST.LocalIP + " " + host))
                        {
                            dt.Rows.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            return dt;
        }
        private void gvShareList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (MessageBox.Show("是否将本条规则添加进Hosts?", ST.MsgTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataGridViewRow row = gvShareList.Rows[e.RowIndex];
                    string host = Convert.ToString(row.Cells["Host"].Value);
                    string toIP = ST.LocalIP;
                    if (gvShareList.Columns["ToIP"] != null)
                    {
                        string ip = Convert.ToString(row.Cells["ToIP"].Value);
                        if (!string.IsNullOrEmpty(ip) && ip != toIP)
                        {
                            toIP = ip;
                        }
                    }
                    ShareHost.AddHost(host, toIP);
                    gvShareList.Rows.RemoveAt(e.RowIndex);
                }
            }
        }
        #endregion
        private void MoreForm_Load(object sender, EventArgs e)
        {
            //txtHost.Focus();
        }

        #region 其它事件
        private void chbShare_CheckedChanged(object sender, EventArgs e)
        {
            txtTitle.Enabled = txtAuthor.Enabled = chbShare.Checked;
            txtToIP.Enabled = !chbShare.Checked;
            if (chbShare.Checked)
            {
                txtToIP.Text = ST.LocalIP;

            }
        }

        private void lnkOpenHosts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("notepad.exe", WebHost.HostPath);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                WebHost.DelHostByFlag(WebHost.adKillerShareStart, WebHost.adKillerShareEnd);
                MessageBox.Show("清空成功！", ST.MsgTitle);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, ST.MsgTitle);
            }
        }
        #endregion

    }
}