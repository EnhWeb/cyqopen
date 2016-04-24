namespace AdKiller
{
    partial class StartForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.chbProxy = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ddlKey = new System.Windows.Forms.ComboBox();
            this.chbAutoStart = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ddlShowMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.icoTip = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsForNotity = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSinaWeiboUrl = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUpdateUrl = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.chbBaidu = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbSogou = new System.Windows.Forms.CheckBox();
            this.chbSoso = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chbIqiyi = new System.Windows.Forms.CheckBox();
            this.chbKankan = new System.Windows.Forms.CheckBox();
            this.chbSohu = new System.Windows.Forms.CheckBox();
            this.chbVqq = new System.Windows.Forms.CheckBox();
            this.chbPps = new System.Windows.Forms.CheckBox();
            this.chbKu6 = new System.Windows.Forms.CheckBox();
            this.chbWu6 = new System.Windows.Forms.CheckBox();
            this.chbLetv = new System.Windows.Forms.CheckBox();
            this.chbTudou = new System.Windows.Forms.CheckBox();
            this.chbYouku = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabOption = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lnkEditFilter = new System.Windows.Forms.LinkLabel();
            this.tabProxyOption = new System.Windows.Forms.TabPage();
            this.btnStopNextProxy = new System.Windows.Forms.Button();
            this.btnStartNextProxy = new System.Windows.Forms.Button();
            this.txtNextProxyPort = new System.Windows.Forms.TextBox();
            this.txtNextProxyIP = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabEncode = new System.Windows.Forms.TabPage();
            this.chbOutWall = new System.Windows.Forms.CheckBox();
            this.radRunAtServer = new System.Windows.Forms.RadioButton();
            this.radNone = new System.Windows.Forms.RadioButton();
            this.radRunAtClient = new System.Windows.Forms.RadioButton();
            this.lnkEditOutwallUrl = new System.Windows.Forms.LinkLabel();
            this.label9 = new System.Windows.Forms.Label();
            this.tabOtherOption = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.timerGC = new System.Windows.Forms.Timer(this.components);
            this.pbQQ = new System.Windows.Forms.PictureBox();
            this.lnkUrlDetail = new System.Windows.Forms.LinkLabel();
            this.cmsForNotity.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabOption.SuspendLayout();
            this.tabProxyOption.SuspendLayout();
            this.tabEncode.SuspendLayout();
            this.tabOtherOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbQQ)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(38, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开启";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(158, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 0;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // chbProxy
            // 
            this.chbProxy.AutoSize = true;
            this.chbProxy.Checked = true;
            this.chbProxy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbProxy.Enabled = false;
            this.chbProxy.Location = new System.Drawing.Point(10, 13);
            this.chbProxy.Name = "chbProxy";
            this.chbProxy.Size = new System.Drawing.Size(72, 16);
            this.chbProxy.TabIndex = 14;
            this.chbProxy.Text = "离线模式";
            this.chbProxy.UseVisualStyleBackColor = true;
            this.chbProxy.CheckedChanged += new System.EventHandler(this.chbProxy_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(102, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "+";
            // 
            // ddlKey
            // 
            this.ddlKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlKey.FormattingEnabled = true;
            this.ddlKey.Items.AddRange(new object[] {
            "Q",
            "W",
            "E",
            "R",
            "T",
            "A",
            "S",
            "D",
            "F",
            "G",
            "Z",
            "X",
            "C",
            "V",
            "B",
            "1",
            "2",
            "3",
            "4",
            "5",
            "F1",
            "F2",
            "F3",
            "F4"});
            this.ddlKey.Location = new System.Drawing.Point(119, 10);
            this.ddlKey.Name = "ddlKey";
            this.ddlKey.Size = new System.Drawing.Size(49, 20);
            this.ddlKey.TabIndex = 9;
            this.ddlKey.SelectedIndexChanged += new System.EventHandler(this.ddlKey_SelectedIndexChanged);
            // 
            // chbAutoStart
            // 
            this.chbAutoStart.AutoSize = true;
            this.chbAutoStart.Checked = true;
            this.chbAutoStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbAutoStart.Location = new System.Drawing.Point(16, 44);
            this.chbAutoStart.Name = "chbAutoStart";
            this.chbAutoStart.Size = new System.Drawing.Size(72, 16);
            this.chbAutoStart.TabIndex = 2;
            this.chbAutoStart.Text = "开机启动";
            this.chbAutoStart.UseVisualStyleBackColor = true;
            this.chbAutoStart.CheckedChanged += new System.EventHandler(this.chbAutoStart_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "快捷键：";
            // 
            // ddlShowMode
            // 
            this.ddlShowMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlShowMode.FormattingEnabled = true;
            this.ddlShowMode.Items.AddRange(new object[] {
            "屏蔽广告显示",
            "高亮显示广告"});
            this.ddlShowMode.Location = new System.Drawing.Point(82, 51);
            this.ddlShowMode.Name = "ddlShowMode";
            this.ddlShowMode.Size = new System.Drawing.Size(154, 20);
            this.ddlShowMode.TabIndex = 1;
            this.ddlShowMode.SelectedIndexChanged += new System.EventHandler(this.ddlShowMode_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "显示方式：";
            // 
            // icoTip
            // 
            this.icoTip.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.icoTip.BalloonTipText = "程序将在后台继续运行...";
            this.icoTip.BalloonTipTitle = "友情提示";
            this.icoTip.ContextMenuStrip = this.cmsForNotity;
            this.icoTip.Icon = ((System.Drawing.Icon)(resources.GetObject("icoTip.Icon")));
            this.icoTip.Text = "秋式广告杀手 - 免费的广告过滤软件";
            this.icoTip.Visible = true;
            this.icoTip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.icoTip_MouseDown);
            // 
            // cmsForNotity
            // 
            this.cmsForNotity.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAbout,
            this.menuOpenFolder,
            this.menuSinaWeiboUrl,
            this.menuUpdateUrl,
            this.menuClose});
            this.cmsForNotity.Name = "cmsForNotity";
            this.cmsForNotity.Size = new System.Drawing.Size(173, 114);
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(172, 22);
            this.menuAbout.Text = "关于秋式广告杀手";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // menuOpenFolder
            // 
            this.menuOpenFolder.Name = "menuOpenFolder";
            this.menuOpenFolder.Size = new System.Drawing.Size(172, 22);
            this.menuOpenFolder.Text = "打开软件目录";
            this.menuOpenFolder.Click += new System.EventHandler(this.menuOpenFolder_Click);
            // 
            // menuSinaWeiboUrl
            // 
            this.menuSinaWeiboUrl.Name = "menuSinaWeiboUrl";
            this.menuSinaWeiboUrl.Size = new System.Drawing.Size(172, 22);
            this.menuSinaWeiboUrl.Text = "作者新浪微博";
            this.menuSinaWeiboUrl.Click += new System.EventHandler(this.menuSinaWeiboUrl_Click);
            // 
            // menuUpdateUrl
            // 
            this.menuUpdateUrl.Name = "menuUpdateUrl";
            this.menuUpdateUrl.Size = new System.Drawing.Size(172, 22);
            this.menuUpdateUrl.Text = "官方下载地址";
            this.menuUpdateUrl.Click += new System.EventHandler(this.menuUpdateUrl_Click);
            // 
            // menuClose
            // 
            this.menuClose.Name = "menuClose";
            this.menuClose.Size = new System.Drawing.Size(172, 22);
            this.menuClose.Text = "退出程序";
            this.menuClose.Click += new System.EventHandler(this.menuClose_Click);
            // 
            // chbBaidu
            // 
            this.chbBaidu.AutoSize = true;
            this.chbBaidu.Checked = true;
            this.chbBaidu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbBaidu.Location = new System.Drawing.Point(38, 24);
            this.chbBaidu.Name = "chbBaidu";
            this.chbBaidu.Size = new System.Drawing.Size(48, 16);
            this.chbBaidu.TabIndex = 11;
            this.chbBaidu.Text = "百度";
            this.chbBaidu.UseVisualStyleBackColor = true;
            this.chbBaidu.CheckedChanged += new System.EventHandler(this.chbBaidu_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chbSogou);
            this.groupBox2.Controls.Add(this.chbSoso);
            this.groupBox2.Controls.Add(this.chbBaidu);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.ddlShowMode);
            this.groupBox2.Location = new System.Drawing.Point(11, 42);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(259, 82);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "搜索引擎";
            // 
            // chbSogou
            // 
            this.chbSogou.AutoSize = true;
            this.chbSogou.Checked = true;
            this.chbSogou.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbSogou.Location = new System.Drawing.Point(168, 24);
            this.chbSogou.Name = "chbSogou";
            this.chbSogou.Size = new System.Drawing.Size(48, 16);
            this.chbSogou.TabIndex = 11;
            this.chbSogou.Text = "搜狗";
            this.chbSogou.UseVisualStyleBackColor = true;
            this.chbSogou.CheckedChanged += new System.EventHandler(this.chbBaidu_CheckedChanged);
            // 
            // chbSoso
            // 
            this.chbSoso.AutoSize = true;
            this.chbSoso.Checked = true;
            this.chbSoso.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbSoso.Location = new System.Drawing.Point(100, 24);
            this.chbSoso.Name = "chbSoso";
            this.chbSoso.Size = new System.Drawing.Size(48, 16);
            this.chbSoso.TabIndex = 11;
            this.chbSoso.Text = "搜搜";
            this.chbSoso.UseVisualStyleBackColor = true;
            this.chbSoso.CheckedChanged += new System.EventHandler(this.chbBaidu_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chbIqiyi);
            this.groupBox3.Controls.Add(this.chbKankan);
            this.groupBox3.Controls.Add(this.chbSohu);
            this.groupBox3.Controls.Add(this.chbVqq);
            this.groupBox3.Controls.Add(this.chbPps);
            this.groupBox3.Controls.Add(this.chbKu6);
            this.groupBox3.Controls.Add(this.chbWu6);
            this.groupBox3.Controls.Add(this.chbLetv);
            this.groupBox3.Controls.Add(this.chbTudou);
            this.groupBox3.Controls.Add(this.chbYouku);
            this.groupBox3.Location = new System.Drawing.Point(11, 130);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(259, 90);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "视频";
            // 
            // chbIqiyi
            // 
            this.chbIqiyi.AutoSize = true;
            this.chbIqiyi.Checked = true;
            this.chbIqiyi.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbIqiyi.Location = new System.Drawing.Point(193, 19);
            this.chbIqiyi.Name = "chbIqiyi";
            this.chbIqiyi.Size = new System.Drawing.Size(60, 16);
            this.chbIqiyi.TabIndex = 16;
            this.chbIqiyi.Text = "爱奇艺";
            this.chbIqiyi.UseVisualStyleBackColor = true;
            this.chbIqiyi.CheckedChanged += new System.EventHandler(this.chbBaidu_CheckedChanged);
            // 
            // chbKankan
            // 
            this.chbKankan.AutoSize = true;
            this.chbKankan.Checked = true;
            this.chbKankan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbKankan.Location = new System.Drawing.Point(114, 20);
            this.chbKankan.Name = "chbKankan";
            this.chbKankan.Size = new System.Drawing.Size(72, 16);
            this.chbKankan.TabIndex = 14;
            this.chbKankan.Text = "迅雷看看";
            this.chbKankan.UseVisualStyleBackColor = true;
            this.chbKankan.CheckedChanged += new System.EventHandler(this.chbBaidu_CheckedChanged);
            // 
            // chbSohu
            // 
            this.chbSohu.AutoSize = true;
            this.chbSohu.Checked = true;
            this.chbSohu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbSohu.Location = new System.Drawing.Point(114, 64);
            this.chbSohu.Name = "chbSohu";
            this.chbSohu.Size = new System.Drawing.Size(72, 16);
            this.chbSohu.TabIndex = 14;
            this.chbSohu.Text = "搜狐视频";
            this.chbSohu.UseVisualStyleBackColor = true;
            this.chbSohu.CheckedChanged += new System.EventHandler(this.chbBaidu_CheckedChanged);
            // 
            // chbVqq
            // 
            this.chbVqq.AutoSize = true;
            this.chbVqq.Checked = true;
            this.chbVqq.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbVqq.Location = new System.Drawing.Point(114, 42);
            this.chbVqq.Name = "chbVqq";
            this.chbVqq.Size = new System.Drawing.Size(72, 16);
            this.chbVqq.TabIndex = 14;
            this.chbVqq.Text = "腾讯视频";
            this.chbVqq.UseVisualStyleBackColor = true;
            this.chbVqq.CheckedChanged += new System.EventHandler(this.chbBaidu_CheckedChanged);
            // 
            // chbPps
            // 
            this.chbPps.AutoSize = true;
            this.chbPps.Checked = true;
            this.chbPps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbPps.Location = new System.Drawing.Point(60, 64);
            this.chbPps.Name = "chbPps";
            this.chbPps.Size = new System.Drawing.Size(42, 16);
            this.chbPps.TabIndex = 12;
            this.chbPps.Text = "pps";
            this.chbPps.UseVisualStyleBackColor = true;
            this.chbPps.CheckedChanged += new System.EventHandler(this.chbBaidu_CheckedChanged);
            // 
            // chbKu6
            // 
            this.chbKu6.AutoSize = true;
            this.chbKu6.Checked = true;
            this.chbKu6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbKu6.Location = new System.Drawing.Point(60, 42);
            this.chbKu6.Name = "chbKu6";
            this.chbKu6.Size = new System.Drawing.Size(42, 16);
            this.chbKu6.TabIndex = 12;
            this.chbKu6.Text = "酷6";
            this.chbKu6.UseVisualStyleBackColor = true;
            this.chbKu6.CheckedChanged += new System.EventHandler(this.chbBaidu_CheckedChanged);
            // 
            // chbWu6
            // 
            this.chbWu6.AutoSize = true;
            this.chbWu6.Checked = true;
            this.chbWu6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbWu6.Location = new System.Drawing.Point(12, 64);
            this.chbWu6.Name = "chbWu6";
            this.chbWu6.Size = new System.Drawing.Size(36, 16);
            this.chbWu6.TabIndex = 12;
            this.chbWu6.Text = "56";
            this.chbWu6.UseVisualStyleBackColor = true;
            this.chbWu6.CheckedChanged += new System.EventHandler(this.chbBaidu_CheckedChanged);
            // 
            // chbLetv
            // 
            this.chbLetv.AutoSize = true;
            this.chbLetv.Checked = true;
            this.chbLetv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbLetv.Location = new System.Drawing.Point(12, 42);
            this.chbLetv.Name = "chbLetv";
            this.chbLetv.Size = new System.Drawing.Size(48, 16);
            this.chbLetv.TabIndex = 12;
            this.chbLetv.Text = "乐视";
            this.chbLetv.UseVisualStyleBackColor = true;
            this.chbLetv.CheckedChanged += new System.EventHandler(this.chbBaidu_CheckedChanged);
            // 
            // chbTudou
            // 
            this.chbTudou.AutoSize = true;
            this.chbTudou.Checked = true;
            this.chbTudou.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbTudou.Location = new System.Drawing.Point(60, 20);
            this.chbTudou.Name = "chbTudou";
            this.chbTudou.Size = new System.Drawing.Size(48, 16);
            this.chbTudou.TabIndex = 11;
            this.chbTudou.Text = "土豆";
            this.chbTudou.UseVisualStyleBackColor = true;
            this.chbTudou.CheckedChanged += new System.EventHandler(this.chbBaidu_CheckedChanged);
            // 
            // chbYouku
            // 
            this.chbYouku.AutoSize = true;
            this.chbYouku.Checked = true;
            this.chbYouku.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbYouku.Location = new System.Drawing.Point(12, 20);
            this.chbYouku.Name = "chbYouku";
            this.chbYouku.Size = new System.Drawing.Size(48, 16);
            this.chbYouku.TabIndex = 11;
            this.chbYouku.Text = "优酷";
            this.chbYouku.UseVisualStyleBackColor = true;
            this.chbYouku.CheckedChanged += new System.EventHandler(this.chbBaidu_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabOption);
            this.tabControl1.Controls.Add(this.tabProxyOption);
            this.tabControl1.Controls.Add(this.tabEncode);
            this.tabControl1.Controls.Add(this.tabOtherOption);
            this.tabControl1.Location = new System.Drawing.Point(11, 226);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(259, 93);
            this.tabControl1.TabIndex = 14;
            // 
            // tabOption
            // 
            this.tabOption.BackColor = System.Drawing.Color.Transparent;
            this.tabOption.Controls.Add(this.lnkUrlDetail);
            this.tabOption.Controls.Add(this.label6);
            this.tabOption.Controls.Add(this.txtPort);
            this.tabOption.Controls.Add(this.label5);
            this.tabOption.Controls.Add(this.lnkEditFilter);
            this.tabOption.Controls.Add(this.chbProxy);
            this.tabOption.Location = new System.Drawing.Point(4, 22);
            this.tabOption.Margin = new System.Windows.Forms.Padding(0);
            this.tabOption.Name = "tabOption";
            this.tabOption.Size = new System.Drawing.Size(251, 67);
            this.tabOption.TabIndex = 0;
            this.tabOption.Text = "高级选项";
            this.tabOption.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(170, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "(81-1024)";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(127, 10);
            this.txtPort.MaxLength = 5;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(38, 21);
            this.txtPort.TabIndex = 17;
            this.txtPort.Text = "812";
            this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(88, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "端口：";
            // 
            // lnkEditFilter
            // 
            this.lnkEditFilter.AutoSize = true;
            this.lnkEditFilter.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkEditFilter.Location = new System.Drawing.Point(140, 44);
            this.lnkEditFilter.Name = "lnkEditFilter";
            this.lnkEditFilter.Size = new System.Drawing.Size(89, 12);
            this.lnkEditFilter.TabIndex = 15;
            this.lnkEditFilter.TabStop = true;
            this.lnkEditFilter.Text = "自定义网址过滤";
            this.lnkEditFilter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkEditFilter_LinkClicked);
            // 
            // tabProxyOption
            // 
            this.tabProxyOption.Controls.Add(this.btnStopNextProxy);
            this.tabProxyOption.Controls.Add(this.btnStartNextProxy);
            this.tabProxyOption.Controls.Add(this.txtNextProxyPort);
            this.tabProxyOption.Controls.Add(this.txtNextProxyIP);
            this.tabProxyOption.Controls.Add(this.label8);
            this.tabProxyOption.Controls.Add(this.label7);
            this.tabProxyOption.Location = new System.Drawing.Point(4, 22);
            this.tabProxyOption.Name = "tabProxyOption";
            this.tabProxyOption.Size = new System.Drawing.Size(251, 67);
            this.tabProxyOption.TabIndex = 2;
            this.tabProxyOption.Text = "代理设置";
            this.tabProxyOption.UseVisualStyleBackColor = true;
            // 
            // btnStopNextProxy
            // 
            this.btnStopNextProxy.Enabled = false;
            this.btnStopNextProxy.Location = new System.Drawing.Point(143, 36);
            this.btnStopNextProxy.Name = "btnStopNextProxy";
            this.btnStopNextProxy.Size = new System.Drawing.Size(50, 23);
            this.btnStopNextProxy.TabIndex = 3;
            this.btnStopNextProxy.Text = "停用";
            this.btnStopNextProxy.UseVisualStyleBackColor = true;
            this.btnStopNextProxy.Click += new System.EventHandler(this.btnStopNextProxy_Click);
            // 
            // btnStartNextProxy
            // 
            this.btnStartNextProxy.Location = new System.Drawing.Point(56, 36);
            this.btnStartNextProxy.Name = "btnStartNextProxy";
            this.btnStartNextProxy.Size = new System.Drawing.Size(50, 23);
            this.btnStartNextProxy.TabIndex = 3;
            this.btnStartNextProxy.Text = "启用";
            this.btnStartNextProxy.UseVisualStyleBackColor = true;
            this.btnStartNextProxy.Click += new System.EventHandler(this.btnStartNextProxy_Click);
            // 
            // txtNextProxyPort
            // 
            this.txtNextProxyPort.Location = new System.Drawing.Point(189, 9);
            this.txtNextProxyPort.Name = "txtNextProxyPort";
            this.txtNextProxyPort.Size = new System.Drawing.Size(40, 21);
            this.txtNextProxyPort.TabIndex = 2;
            this.txtNextProxyPort.TextChanged += new System.EventHandler(this.txtNextProxyIP_TextChanged);
            // 
            // txtNextProxyIP
            // 
            this.txtNextProxyIP.Location = new System.Drawing.Point(36, 9);
            this.txtNextProxyIP.Name = "txtNextProxyIP";
            this.txtNextProxyIP.Size = new System.Drawing.Size(100, 21);
            this.txtNextProxyIP.TabIndex = 1;
            this.txtNextProxyIP.TextChanged += new System.EventHandler(this.txtNextProxyIP_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(144, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "端口：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "IP：";
            // 
            // tabEncode
            // 
            this.tabEncode.Controls.Add(this.chbOutWall);
            this.tabEncode.Controls.Add(this.radRunAtServer);
            this.tabEncode.Controls.Add(this.radNone);
            this.tabEncode.Controls.Add(this.radRunAtClient);
            this.tabEncode.Controls.Add(this.lnkEditOutwallUrl);
            this.tabEncode.Controls.Add(this.label9);
            this.tabEncode.Location = new System.Drawing.Point(4, 22);
            this.tabEncode.Name = "tabEncode";
            this.tabEncode.Size = new System.Drawing.Size(251, 67);
            this.tabEncode.TabIndex = 3;
            this.tabEncode.Text = "破墙设置";
            this.tabEncode.UseVisualStyleBackColor = true;
            // 
            // chbOutWall
            // 
            this.chbOutWall.AutoSize = true;
            this.chbOutWall.Location = new System.Drawing.Point(11, 40);
            this.chbOutWall.Name = "chbOutWall";
            this.chbOutWall.Size = new System.Drawing.Size(72, 16);
            this.chbOutWall.TabIndex = 16;
            this.chbOutWall.Text = "一剑出墙";
            this.chbOutWall.UseVisualStyleBackColor = true;
            this.chbOutWall.CheckedChanged += new System.EventHandler(this.chbOutwall_CheckedChanged);
            // 
            // radRunAtServer
            // 
            this.radRunAtServer.AutoSize = true;
            this.radRunAtServer.Location = new System.Drawing.Point(177, 13);
            this.radRunAtServer.Name = "radRunAtServer";
            this.radRunAtServer.Size = new System.Drawing.Size(59, 16);
            this.radRunAtServer.TabIndex = 4;
            this.radRunAtServer.Text = "服务端";
            this.radRunAtServer.UseVisualStyleBackColor = true;
            this.radRunAtServer.CheckedChanged += new System.EventHandler(this.radRunAtClient_CheckedChanged);
            // 
            // radNone
            // 
            this.radNone.AutoSize = true;
            this.radNone.Checked = true;
            this.radNone.Location = new System.Drawing.Point(133, 13);
            this.radNone.Name = "radNone";
            this.radNone.Size = new System.Drawing.Size(35, 16);
            this.radNone.TabIndex = 4;
            this.radNone.TabStop = true;
            this.radNone.Text = "无";
            this.radNone.UseVisualStyleBackColor = true;
            this.radNone.CheckedChanged += new System.EventHandler(this.radRunAtClient_CheckedChanged);
            // 
            // radRunAtClient
            // 
            this.radRunAtClient.AutoSize = true;
            this.radRunAtClient.Location = new System.Drawing.Point(68, 13);
            this.radRunAtClient.Name = "radRunAtClient";
            this.radRunAtClient.Size = new System.Drawing.Size(59, 16);
            this.radRunAtClient.TabIndex = 4;
            this.radRunAtClient.Text = "客户端";
            this.radRunAtClient.UseVisualStyleBackColor = true;
            this.radRunAtClient.CheckedChanged += new System.EventHandler(this.radRunAtClient_CheckedChanged);
            // 
            // lnkEditOutwallUrl
            // 
            this.lnkEditOutwallUrl.AutoSize = true;
            this.lnkEditOutwallUrl.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkEditOutwallUrl.Location = new System.Drawing.Point(143, 42);
            this.lnkEditOutwallUrl.Name = "lnkEditOutwallUrl";
            this.lnkEditOutwallUrl.Size = new System.Drawing.Size(89, 12);
            this.lnkEditOutwallUrl.TabIndex = 15;
            this.lnkEditOutwallUrl.TabStop = true;
            this.lnkEditOutwallUrl.Text = "自定义破墙网址";
            this.lnkEditOutwallUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkEditOutwallUrl_LinkClicked);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "设置为：";
            // 
            // tabOtherOption
            // 
            this.tabOtherOption.BackColor = System.Drawing.Color.Transparent;
            this.tabOtherOption.Controls.Add(this.label4);
            this.tabOtherOption.Controls.Add(this.label10);
            this.tabOtherOption.Controls.Add(this.ddlKey);
            this.tabOtherOption.Controls.Add(this.label3);
            this.tabOtherOption.Controls.Add(this.chbAutoStart);
            this.tabOtherOption.Controls.Add(this.label2);
            this.tabOtherOption.Controls.Add(this.pbQQ);
            this.tabOtherOption.Location = new System.Drawing.Point(4, 22);
            this.tabOtherOption.Margin = new System.Windows.Forms.Padding(0);
            this.tabOtherOption.Name = "tabOtherOption";
            this.tabOtherOption.Size = new System.Drawing.Size(251, 67);
            this.tabOtherOption.TabIndex = 1;
            this.tabOtherOption.Text = "其它选项";
            this.tabOtherOption.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(191, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "在线反馈";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(73, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 12);
            this.label10.TabIndex = 11;
            this.label10.Text = "Alt";
            // 
            // timerGC
            // 
            this.timerGC.Enabled = true;
            this.timerGC.Interval = 450000;
            // 
            // pbQQ
            // 
            this.pbQQ.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbQQ.ErrorImage = global::AdKiller.Properties.Resources.qq;
            this.pbQQ.Image = global::AdKiller.Properties.Resources.qq;
            this.pbQQ.InitialImage = global::AdKiller.Properties.Resources.qq;
            this.pbQQ.Location = new System.Drawing.Point(164, 37);
            this.pbQQ.Name = "pbQQ";
            this.pbQQ.Size = new System.Drawing.Size(25, 25);
            this.pbQQ.TabIndex = 12;
            this.pbQQ.TabStop = false;
            this.pbQQ.Click += new System.EventHandler(this.pbQQ_Click);
            // 
            // lnkUrlDetail
            // 
            this.lnkUrlDetail.AutoSize = true;
            this.lnkUrlDetail.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkUrlDetail.Location = new System.Drawing.Point(10, 43);
            this.lnkUrlDetail.Name = "lnkUrlDetail";
            this.lnkUrlDetail.Size = new System.Drawing.Size(47, 12);
            this.lnkUrlDetail.TabIndex = 19;
            this.lnkUrlDetail.TabStop = true;
            this.lnkUrlDetail.Text = "URL明细";
            this.lnkUrlDetail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUrlDetail_LinkClicked);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 332);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "StartForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "秋式广告杀手 V2.8";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.StartForm_Load);
            this.SizeChanged += new System.EventHandler(this.StartForm_SizeChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StartForm_FormClosing);
            this.cmsForNotity.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabOption.ResumeLayout(false);
            this.tabOption.PerformLayout();
            this.tabProxyOption.ResumeLayout(false);
            this.tabProxyOption.PerformLayout();
            this.tabEncode.ResumeLayout(false);
            this.tabEncode.PerformLayout();
            this.tabOtherOption.ResumeLayout(false);
            this.tabOtherOption.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbQQ)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ComboBox ddlShowMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddlKey;
        private System.Windows.Forms.CheckBox chbAutoStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NotifyIcon icoTip;
        private System.Windows.Forms.CheckBox chbBaidu;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chbSogou;
        private System.Windows.Forms.CheckBox chbSoso;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chbYouku;
        private System.Windows.Forms.CheckBox chbTudou;
        private System.Windows.Forms.ContextMenuStrip cmsForNotity;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.ToolStripMenuItem menuSinaWeiboUrl;
        private System.Windows.Forms.ToolStripMenuItem menuUpdateUrl;
        private System.Windows.Forms.ToolStripMenuItem menuClose;
        private System.Windows.Forms.CheckBox chbVqq;
        private System.Windows.Forms.CheckBox chbLetv;
        private System.Windows.Forms.CheckBox chbKankan;
        private System.Windows.Forms.CheckBox chbWu6;
        private System.Windows.Forms.CheckBox chbPps;
        private System.Windows.Forms.CheckBox chbKu6;
        private System.Windows.Forms.CheckBox chbSohu;
        private System.Windows.Forms.CheckBox chbIqiyi;
        private System.Windows.Forms.CheckBox chbProxy;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabOption;
        private System.Windows.Forms.TabPage tabOtherOption;
        private System.Windows.Forms.LinkLabel lnkEditFilter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripMenuItem menuOpenFolder;
        private System.Windows.Forms.TabPage tabProxyOption;
        private System.Windows.Forms.TextBox txtNextProxyPort;
        private System.Windows.Forms.TextBox txtNextProxyIP;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnStartNextProxy;
        private System.Windows.Forms.Button btnStopNextProxy;
        private System.Windows.Forms.TabPage tabEncode;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton radRunAtServer;
        private System.Windows.Forms.RadioButton radRunAtClient;
        private System.Windows.Forms.RadioButton radNone;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.LinkLabel lnkEditOutwallUrl;
        private System.Windows.Forms.CheckBox chbOutWall;
        private System.Windows.Forms.Timer timerGC;
        private System.Windows.Forms.PictureBox pbQQ;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel lnkUrlDetail;
    }
}

