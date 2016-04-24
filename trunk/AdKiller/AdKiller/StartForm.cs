using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace AdKiller
{
    public partial class StartForm : Form
    {
        UrlFilter filter = new UrlFilter();
        UrlOutWall outWall = new UrlOutWall();
        SystemHotKey hotKey = null;
        bool isAllowClose = false;//关闭窗体，处理无法关机问题。
        int port = 81;
        string icoText = string.Empty;
        public StartForm()
        {
            InitializeComponent();
            Form.CheckForIllegalCrossThreadCalls = false;
            hotKey = new SystemHotKey(IntPtr.Zero);
            hotKey.OnHotkey += new HotkeyEventHandler(hotKey_OnHotkey);
            timerGC.Tick += new EventHandler(timerGC_Tick);
            Config.LoadConfig();
            icoText = icoTip.Text;
        }

        void timerGC_Tick(object sender, EventArgs e)
        {
            SocketPool.Clear();
            Tool.ClearHostIPCache();
            GC.Collect();// 15分钟定时回收一次内存。
        }

        #region 开启及关闭
        private void btnStart_Click(object sender, EventArgs e)
        {
            SetControlState(true);
            Thread hostsThread = new Thread(new ThreadStart(WebHost.CheckUpdate), 512);
            hostsThread.IsBackground = true;
            hostsThread.Start();
            Thread listenThread = new Thread(new ThreadStart(StartListen), 512);
            listenThread.IsBackground = true;
            listenThread.Start();
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                SocketPool.Clear();
                Tool.ClearHostIPCache();
                Config.proxyList.Clear();
                if (chbProxy.Checked)//如果打勾状态，取消代理。
                {
                    IEProxy.SetProxy(null);
                }
                SetControlState(false);
                TcpListen.Stop();
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
            }
        }
        void StartListen()
        {
            try
            {
                if (chbProxy.Checked)
                {
                    IEProxy.SetProxy(ST.LocalIP + ":" + port);
                }
                icoTip.Text = icoText + string.Format(" - {0}", port);
                TcpListen.Listen(port);

            }
            catch (System.Net.Sockets.SocketException err)
            {
                if (chbProxy.Checked)
                {
                    IEProxy.SetProxy(null);
                }
                if (err.ErrorCode == 10013 || err.ErrorCode == 10048)
                {
                    MessageBox.Show(port + "端口已被占用，请在高级选项里更换端口", ST.MsgTitle);
                }
                else
                {
                    MessageBox.Show(err.Message, ST.MsgTitle);
                }
                SetControlState(false);
                TcpListen.Stop();
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
            }

        }
        void SetControlState(bool isStart)
        {
            btnStart.Enabled = !isStart;
            btnStop.Enabled = isStart;
            txtPort.Enabled = !isStart;
            chbProxy.Enabled = isStart;// && (port == 81 || Config.RunAtType == 2)

        }
        /// <summary>
        /// 禁止部分服务
        /// </summary>
        //void DisabledpartialServer(string tip)
        //{
        //    try
        //    {
        //        //MessageBox.Show("服务器暂时无法连通，系统将取消搜索引擎广告屏蔽功能，并继续运行！", "消息提示");
        //        //chbBaidu.Enabled = Config.Baidu = false;
        //        //chbSoso.Enabled = Config.Soso = false;
        //        //chbSogou.Enabled = Config.Sogou = false;
        //        HostEntity host = null;
        //        foreach (KeyValuePair<string, HostEntity> item in WebHost.DomainList)
        //        {
        //            host = item.Value;
        //            if (item.Value.IsNeedServerSupport)
        //            {
        //                item.Value.IsEnabled = false;
        //            }
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        DebugLog.WriteError(err);
        //    }

        //}
        #endregion

        #region 选项设置事件
        #region 高级项
        void hotKey_OnHotkey(int HotKeyID)
        {
            if (Visible && WindowState == FormWindowState.Normal) //可见状态
            {
                Visible = false;
            }
            else
            {
                Show();
            }
        }
        private void ddlShowMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = ddlShowMode.SelectedIndex;
            AdFormat.searchMode = (SearchMode)index;
            Config.Smode = index;
        }

        private void ddlKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            hotKey.UnregisterHotkeys();
            SystemHotKey.KeyFlags keyFlag = (SystemHotKey.KeyFlags)Enum.Parse(typeof(SystemHotKey.KeyFlags), "Alt");
            Keys keys = (Keys)Enum.Parse(typeof(Keys), ddlKey.Text);
            hotKey.RegisterHotkey(keyFlag, keys);

            Config.Altkey = ddlKey.SelectedIndex;

        }

        private void chbAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            SysRegKey.AutoStartComputer(chbAutoStart.Checked);
            Config.Sauto = chbAutoStart.Checked;
        }
        private void lnkEditFilter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!chbProxy.Checked)
            {
                chbProxy.Checked = true;
            }
            filter.OpenForEdit();
        }
        #endregion

        private void chbBaidu_CheckedChanged(object sender, EventArgs e)
        {
            if (!Config.AllowSaveConfig)
            {
                return;
            }
            CheckBox chb = sender as CheckBox;
            bool isCheck = chb.Checked;
            switch (chb.Name.Substring(3))
            {
                case "Baidu":
                    Config.Baidu = WebHost.DomainList[WebHost.domainBaidu].IsEnabled = isCheck;
                    break;
                case "Soso":
                    Config.Soso = WebHost.DomainList[WebHost.domainSoso].IsEnabled = isCheck;
                    break;
                case "Sogou":
                    Config.Sogou = WebHost.DomainList[WebHost.domainSogou].IsEnabled = isCheck;
                    break;
                case "Youku":
                    Config.Youku = WebHost.DomainList[WebHost.domainYouku].IsEnabled = isCheck;
                    break;
                case "Tudou":
                    Config.Tudou = WebHost.DomainList[WebHost.domainTudou].IsEnabled = isCheck;
                    break;
                case "Kankan":
                    Config.Kankan = WebHost.DomainList[WebHost.domainKankan].IsEnabled = isCheck;
                    break;
                case "Iqiyi":
                    Config.Iqiyi = WebHost.DomainList[WebHost.domainIqiyi].IsEnabled = isCheck;
                    break;
                case "Letv":
                    Config.Letv = WebHost.DomainList[WebHost.domainLetv].IsEnabled = isCheck;
                    break;
                case "Ku6":
                    Config.Ku6 = WebHost.DomainList[WebHost.domainKu6].IsEnabled = isCheck;
                    break;
                case "Vqq":
                    Config.Vqq = WebHost.DomainList[WebHost.domainVqq].IsEnabled = isCheck;
                    break;
                case "Wu6":
                    Config.Wu6 = WebHost.DomainList[WebHost.domainWu6].IsEnabled = isCheck;
                    break;
                case "Pps":
                    Config.Pps = WebHost.DomainList[WebHost.domainPps].IsEnabled = isCheck;
                    break;
                case "Sohu":
                    Config.Sohu = WebHost.DomainList[WebHost.domainSohu].IsEnabled = isCheck;
                    break;
            }
            Config.SaveConfig();
        }
        #endregion



        #region Form窗体事件

        bool isFirstUse = true;
        private void StartForm_Load(object sender, EventArgs e)
        {
            if (Program.processCount > 1)
            {
                Text += " - " + Program.processCount;
            }
            isFirstUse = !Config.Exists(true);
            Config.AllowSaveConfig = false;
            FormLoad();

            //检测是否首次使用
            if (isFirstUse)//首次使用
            {
                Show();
            }
            else //非首次使用
            {
                btnStart_Click(null, null);
            }
            Config.AllowSaveConfig = true;
            //Thread thread = new Thread(new ThreadStart(TimerClearDnsCache));
            //thread.IsBackground = true;
            //thread.Start();
        }
        //void TimerClearDnsCache()
        //{
        //    while (true)
        //    {
        //        Thread.Sleep(TimeSpan.FromMinutes(15));
        //        Tool.ClearHostIPCache();
        //    }
        //}
        void FormLoad()
        {
            if (isFirstUse)
            {
                ddlShowMode.SelectedIndex = 0;//设置显示的模式
                ddlKey.Text = "S";
            }
            LoadConfig();
            if (chbAutoStart.Checked)
            {
                SysRegKey.AutoStartComputer(true);
            }
        }
        #region 初始化数据

        void LoadConfig()
        {
            if (!isFirstUse)
            {
                //搜索引擎
                chbBaidu.Checked = Config.Baidu;
                chbSoso.Checked = Config.Soso;
                chbSogou.Checked = Config.Sogou;
                //视频网站
                chbYouku.Checked = Config.Youku;
                chbTudou.Checked = Config.Tudou;
                chbLetv.Checked = Config.Letv;
                chbVqq.Checked = Config.Vqq;
                chbKankan.Checked = Config.Kankan;
                chbWu6.Checked = Config.Wu6;
                chbPps.Checked = Config.Pps;
                chbKu6.Checked = Config.Ku6;
                chbSohu.Checked = Config.Sohu;
                chbIqiyi.Checked = Config.Iqiyi;

                //其它
                DebugLog.allowWriteLog = Config.WriteLog;

                //高级选项
                chbAutoStart.Checked = Config.Sauto;
                ddlShowMode.SelectedIndex = Config.Smode;
                ddlKey.SelectedIndex = Config.Altkey;

                //代理
                chbOutWall.Checked = Config.OutWall;
                txtPort.Text = Convert.ToString(Config.UsePort);
                chbProxy.Checked = Config.UseProxy;
                txtNextProxyIP.Text = Config.NextProxyIP;
                txtNextProxyPort.Text = Convert.ToString(Config.NextProxyPort);

                switch (Config.RunAtType)
                {
                    case 1:
                        radRunAtClient.Checked = true;
                        break;
                    case 2:
                        radRunAtServer.Checked = true;
                        break;
                    default:
                        radNone.Checked = true;
                        break;
                }
            }
        }

        #endregion
        private void StartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isAllowClose || e.CloseReason != CloseReason.UserClosing)
            {
                if (btnStop.Enabled)
                {
                    btnStop_Click(null, null);
                }
                // SysRegKey.CloseDnsCache(false);//恢复IE DNS缓存。
                if (chbProxy.Checked)
                {
                    IEProxy.SetProxy(null);//取消代理设置
                }
                Config.SaveConfig();//重新保存配置文件。
            }
            else
            {
                e.Cancel = true;
                Visible = false;
                // icoTip.ShowBalloonTip(50);
                GC.Collect();
            }
        }
        private void StartForm_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                if (Visible)
                {
                    Visible = false;
                }
            }
        }
        #endregion

        #region 通知图标事件
        private void icoTip_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Show();
            }
        }
        new void Show()
        {
            try
            {
                TopMost = true;
                if (!Visible)
                {
                    Visible = true;
                }
                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                }
                TopMost = false;
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
            }
        }
        #endregion

        #region 右键菜单
        private void menuClose_Click(object sender, EventArgs e)
        {
            isAllowClose = true;
            this.Close();
        }


        private void menuAbout_Click(object sender, EventArgs e)
        {
            string tip = "秋式广告杀手 是一款免费的广告过滤软件!\r\n\r\n";
            tip += "主要功能：\r\n\r\n";
            tip += "1：屏蔽搜索引擎竞价广告，避免误入虚假广告导致不良后果！\r\n";
            tip += "2：屏蔽视频广告，减少每天被强制浪费在广告上的时间，节省时间，延长寿命！\r\n";
            tip += "3：支持自定义网址过滤广告！\r\n";
            tip += "4：支持二次代理功能（同时也是一款免费的代理软件！）\r\n";

            MessageBox.Show(tip, "关于秋式广告杀手");
        }

        private void menuSinaWeiboUrl_Click(object sender, EventArgs e)
        {
            StartHttp("http://weibo.com/cyqdata");
        }

        private void menuUpdateUrl_Click(object sender, EventArgs e)
        {
            StartHttp("http://www.cyqdata.com/download/article-detail-54271");
        }

        #region 默认浏览器打开网址
        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            ShowCommands nShowCmd);
        /// <summary>
        /// 用默认浏览器打开网址
        /// </summary>
        protected static void StartHttp(string url)
        {
            try
            {
                ShellExecute(IntPtr.Zero, "open", url, "", "", ShowCommands.SW_SHOWNOACTIVATE);
            }
            catch
            {
                System.Diagnostics.Process.Start("IEXPLORE.EXE", url);
            }
        }
        public enum ShowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }
        #endregion

        private void menuOpenFolder_Click(object sender, EventArgs e)
        {
            Process.Start(AppDomain.CurrentDomain.BaseDirectory);
        }

        #endregion

        #region 代理事件
        private void chbProxy_CheckedChanged(object sender, EventArgs e)
        {
            if (!Config.AllowSaveConfig)
            {
                return;
            }
            Config.UseProxy = ((CheckBox)sender).Checked;
            if (!btnStart.Enabled)
            {
                IEProxy.SetProxy(chbProxy.Checked ? (ST.LocalIP + ":" + port) : null);
            }
        }
        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPort.Text, out port) || port < 1)
            {
                txtPort.Text = "81";
            }
            else
            {
                Config.UsePort = port;
            }
        }
        string proxyIP = string.Empty;
        int proxyPort = 0;
        private void btnStartNextProxy_Click(object sender, EventArgs e)
        {
            proxyPort = 0;
            proxyIP = txtNextProxyIP.Text.Trim();
            if (!int.TryParse(txtNextProxyPort.Text.Trim(), out proxyPort) || proxyPort < 0 || string.IsNullOrEmpty(proxyIP))
            {
                MessageBox.Show("IP和端口请填写正确", ST.MsgTitle);
                return;
            }
            btnStartNextProxy.Enabled = false;
            Thread thread = new Thread(new ThreadStart(TestProxy), 512);
            thread.IsBackground = true;
            thread.Start();

        }
        void TestProxy()
        {
            string tip = string.Empty;
            if (Tool.TestConnect(proxyIP, proxyPort, out tip))
            {
                Config.NextProxyIP = proxyIP;
                Config.NextProxyPort = proxyPort;
                Config.SetNextProxy();
                btnStopNextProxy.Enabled = true;
            }
            else
            {
                MessageBox.Show(tip, ST.MsgTitle);
                btnStartNextProxy.Enabled = true;
            }
        }
        private void btnStopNextProxy_Click(object sender, EventArgs e)
        {
            btnStartNextProxy.Enabled = true;
            btnStopNextProxy.Enabled = false;
            Config.ClearNextProxy();
            if (Config.RunAtType == 1)
            {
                radNone.Checked = true;
            }
        }

        private void txtNextProxyIP_TextChanged(object sender, EventArgs e)
        {
            if (btnStopNextProxy.Enabled)
            {
                btnStopNextProxy_Click(null, null);
            }
        }

        private void radRunAtClient_CheckedChanged(object sender, EventArgs e)
        {
            Config.RunAtType = radNone.Checked ? 0 : (radRunAtClient.Checked ? 1 : 2);
            if (Config.RunAtType == 1 && Config.NextProxy == null)
            {
                if (Config.AllowSaveConfig)
                {
                    MessageBox.Show("您需要在代理设置中开启连接服务端后才能设置此选项！", ST.MsgTitle);
                }
                radNone.Checked = true;
            }
        }

        private void lnkEditOutwallUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!chbOutWall.Checked)
            {
                chbOutWall.Checked = true;
            }
            outWall.OpenForEdit();
        }

        private void chbOutwall_CheckedChanged(object sender, EventArgs e)
        {
            Config.OutWall = chbOutWall.Checked;
            if (Config.OutWall)
            {
                chbOutWall.Text = "检测中...";
                chbOutWall.Enabled = false;
                Thread thread = new Thread(new ThreadStart(CheckProxy), 512);
                thread.IsBackground = true;
                thread.Start();
            }
        }
        void CheckProxy()
        {
            try
            {
                //检测默认代理是否可用
                if (Config.CheckSysProxy())
                {
                    if (Config.RunAtType != 0)
                    {
                        radNone.Checked = true;
                    }
                }
                else
                {
                    if (Config.AllowSaveConfig)
                    {
                        MessageBox.Show("抱歉，无法连接到服务器，请稍后重试，若有疑问，请联系作者！", ST.MsgTitle);
                    }
                    chbOutWall.Checked = false;
                }
            }
            catch
            {

            }
            finally
            {
                chbOutWall.Text = "一剑出墙";
                chbOutWall.Enabled = true;
            }

        }

        private void pbQQ_Click(object sender, EventArgs e)
        {
            StartHttp("http://wpa.qq.com/msgrd?v=3&uin=272657997&site=qq&menu=yes");
        }
        DetailForm df = null;
        private void lnkUrlDetail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (df == null)
            {
                df = new DetailForm();
            }
            df.Show();
            df.Activate();
        }
        ////一键出墙
        //private void btnOutWall_Click(object sender, EventArgs e)
        //{
        //    MessageBox.Show("由于服务器节点有限，目前只能义务提供个把小时服务仅供测试", ST.MsgTitle);
        //    btnOutWall.Enabled = false;
        //    Thread thread = new Thread(new ThreadStart(OutWall));
        //    thread.IsBackground = true;
        //    thread.Start();
        //}
        //void OutWall()
        //{
        //    string proxy = HttpNet.GetProxy();
        //    if (!string.IsNullOrEmpty(proxy))
        //    {
        //        string[] items = proxy.Split(':');
        //        if (items.Length == 2)
        //        {
        //            string msg = string.Empty;
        //            if (Tool.TestConnect(items[0], int.Parse(items[1]), out msg))
        //            {
        //                Config.NextProxyIP = items[0];
        //                Config.NextProxyPort = int.Parse(items[1]);
        //                Config.SetNextProxy();
        //            }
        //            else
        //            {

        //            }
        //        }
        //    }
        //}
        #endregion
    }
}