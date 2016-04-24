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
        #region ���Hosts
        string host, toIP, title, author;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                //��ӵ�Host�ļ�
                if (ShareHost.AddHost(host, toIP))
                {
                    if (chbShare.Checked)
                    {
                        //�ύ����������
                        chbShare.Enabled = false;
                        Thread thread = new Thread(PostShare);
                        thread.IsBackground = true;
                        thread.Start();
                    }
                    txtHost.Text = "";
                    MessageBox.Show("��ӳɹ�!", ST.MsgTitle);
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
                (chbShare.Checked && (string.IsNullOrEmpty(title) || title.StartsWith("��ʽ") || string.IsNullOrEmpty(author))))
            {
                MessageBox.Show("����д������!", ST.MsgTitle);
                return false;
            }
            if (host.IndexOf('.') == host.LastIndexOf('.') || host.Split(' ').Length > 1)
            {
                MessageBox.Show("Host��ʽ����ȷ����������Ϊ����������!", ST.MsgTitle);
                return false;
            }
            if (!string.IsNullOrEmpty(author) && !author.StartsWith("killer_"))
            {
                MessageBox.Show("Code���󣬽�QQȺ��227664757���������Code���ܽ��з���!", ST.MsgTitle);
                return false;
            }
            return true;
        }

        #region Post�������
        void PostShare()
        {
            ShareNet.Post(host, toIP, title,author);
            Thread.Sleep(TimeSpan.FromMinutes(1));
            chbShare.Enabled = true;
        }
        #endregion
        #endregion





        #region ��ȡ����

        int pageIndex = 1, flag = 0;
        bool isHidden = true;
        private void btnGetShareHost_Click(object sender, EventArgs e)
        {
            lnkNext.Enabled = pageIndex > 0;
            lnkPre.Enabled = pageIndex > 1;
            if (pageIndex < 1)
            {
                pageIndex = 1;
                MessageBox.Show("�Ѿ�����ҳ�ˣ�", ST.MsgTitle);
                return;
            }
            flag = rbtnAsc.Checked ? 0 : 1;
            btnGetShareHost.Enabled = false;
            isHidden = chbHidden.Checked;
            Thread thread = new Thread(new ThreadStart(GetShareHost));
            thread.IsBackground = true;
            thread.Start();
            lbTip.Text = "���ڶ�ȡ...";
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
                    MessageBox.Show("�鲻�������ˣ�", ST.MsgTitle);
                    lnkNext.Enabled = false;
                    pageIndex--;
                }
            }
            finally
            {
                lbTip.Text = "������ɣ�";
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
            //����������
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
                MessageBox.Show("��ҳ��������ȫ�����ù��ˣ�ȫ�������ˣ�", ST.MsgTitle);
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
                if (MessageBox.Show("�Ƿ񽫱���������ӽ�Hosts?", ST.MsgTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
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

        #region �����¼�
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
                MessageBox.Show("��ճɹ���", ST.MsgTitle);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, ST.MsgTitle);
            }
        }
        #endregion

    }
}