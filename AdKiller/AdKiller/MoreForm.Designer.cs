namespace AdKiller
{
    partial class MoreForm
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
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            this.gbAddRules = new System.Windows.Forms.GroupBox();
            this.lnkOpenHosts = new System.Windows.Forms.LinkLabel();
            this.chbShare = new System.Windows.Forms.CheckBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtToIP = new System.Windows.Forms.TextBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.txtAuthor = new System.Windows.Forms.TextBox();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbHidden = new System.Windows.Forms.CheckBox();
            this.lnkNext = new System.Windows.Forms.LinkLabel();
            this.lnkPre = new System.Windows.Forms.LinkLabel();
            this.rbtnDesc = new System.Windows.Forms.RadioButton();
            this.rbtnAsc = new System.Windows.Forms.RadioButton();
            this.label12 = new System.Windows.Forms.Label();
            this.lbTip = new System.Windows.Forms.Label();
            this.btnGetShareHost = new System.Windows.Forms.Button();
            this.gvShareList = new System.Windows.Forms.DataGridView();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.gbAddRules.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvShareList)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(236, 25);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(41, 12);
            label3.TabIndex = 0;
            label3.Text = "作用：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(236, 57);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(65, 12);
            label4.TabIndex = 0;
            label4.Text = "分享Code：";
            // 
            // gbAddRules
            // 
            this.gbAddRules.Controls.Add(this.lnkOpenHosts);
            this.gbAddRules.Controls.Add(this.chbShare);
            this.gbAddRules.Controls.Add(this.btnAdd);
            this.gbAddRules.Controls.Add(this.txtToIP);
            this.gbAddRules.Controls.Add(this.txtTitle);
            this.gbAddRules.Controls.Add(this.txtAuthor);
            this.gbAddRules.Controls.Add(this.txtHost);
            this.gbAddRules.Controls.Add(label4);
            this.gbAddRules.Controls.Add(label3);
            this.gbAddRules.Controls.Add(this.label2);
            this.gbAddRules.Controls.Add(this.label6);
            this.gbAddRules.Controls.Add(this.label7);
            this.gbAddRules.Controls.Add(this.label8);
            this.gbAddRules.Controls.Add(this.label5);
            this.gbAddRules.Controls.Add(this.label1);
            this.gbAddRules.Location = new System.Drawing.Point(9, 302);
            this.gbAddRules.Name = "gbAddRules";
            this.gbAddRules.Size = new System.Drawing.Size(521, 125);
            this.gbAddRules.TabIndex = 0;
            this.gbAddRules.TabStop = false;
            this.gbAddRules.Text = "添加规则";
            // 
            // lnkOpenHosts
            // 
            this.lnkOpenHosts.AutoSize = true;
            this.lnkOpenHosts.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkOpenHosts.Location = new System.Drawing.Point(423, 96);
            this.lnkOpenHosts.Name = "lnkOpenHosts";
            this.lnkOpenHosts.Size = new System.Drawing.Size(83, 12);
            this.lnkOpenHosts.TabIndex = 4;
            this.lnkOpenHosts.TabStop = true;
            this.lnkOpenHosts.Text = "打开Hosts文件";
            this.lnkOpenHosts.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOpenHosts_LinkClicked);
            // 
            // chbShare
            // 
            this.chbShare.AutoSize = true;
            this.chbShare.Location = new System.Drawing.Point(174, 89);
            this.chbShare.Name = "chbShare";
            this.chbShare.Size = new System.Drawing.Size(72, 16);
            this.chbShare.TabIndex = 3;
            this.chbShare.Text = "分享规则";
            this.chbShare.UseVisualStyleBackColor = true;
            this.chbShare.CheckedChanged += new System.EventHandler(this.chbShare_CheckedChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(93, 85);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtToIP
            // 
            this.txtToIP.Location = new System.Drawing.Point(93, 52);
            this.txtToIP.MaxLength = 17;
            this.txtToIP.Name = "txtToIP";
            this.txtToIP.Size = new System.Drawing.Size(113, 21);
            this.txtToIP.TabIndex = 2;
            this.txtToIP.Text = "127.0.0.1";
            // 
            // txtTitle
            // 
            this.txtTitle.Enabled = false;
            this.txtTitle.Location = new System.Drawing.Point(276, 20);
            this.txtTitle.MaxLength = 50;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(208, 21);
            this.txtTitle.TabIndex = 3;
            this.txtTitle.Text = "屏蔽 - xx网站名 - 视频(网页)广告";
            // 
            // txtAuthor
            // 
            this.txtAuthor.Enabled = false;
            this.txtAuthor.Location = new System.Drawing.Point(301, 52);
            this.txtAuthor.MaxLength = 50;
            this.txtAuthor.Name = "txtAuthor";
            this.txtAuthor.Size = new System.Drawing.Size(183, 21);
            this.txtAuthor.TabIndex = 1;
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(93, 20);
            this.txtHost.MaxLength = 50;
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(113, 21);
            this.txtHost.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "指向IP：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(212, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(495, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(11, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "*";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(495, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(11, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "*";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(212, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Host域名：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbHidden);
            this.groupBox1.Controls.Add(this.lnkNext);
            this.groupBox1.Controls.Add(this.lnkPre);
            this.groupBox1.Controls.Add(this.rbtnDesc);
            this.groupBox1.Controls.Add(this.rbtnAsc);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.lbTip);
            this.groupBox1.Controls.Add(this.btnGetShareHost);
            this.groupBox1.Controls.Add(this.gvShareList);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(518, 284);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "分享规则列表（PS：读取规则后，可双击行添加规则）";
            // 
            // chbHidden
            // 
            this.chbHidden.AutoSize = true;
            this.chbHidden.Checked = true;
            this.chbHidden.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbHidden.Location = new System.Drawing.Point(401, 257);
            this.chbHidden.Name = "chbHidden";
            this.chbHidden.Size = new System.Drawing.Size(108, 16);
            this.chbHidden.TabIndex = 8;
            this.chbHidden.Text = "隐藏已设置规则";
            this.chbHidden.UseVisualStyleBackColor = true;
            // 
            // lnkNext
            // 
            this.lnkNext.AutoSize = true;
            this.lnkNext.Enabled = false;
            this.lnkNext.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkNext.Location = new System.Drawing.Point(137, 257);
            this.lnkNext.Name = "lnkNext";
            this.lnkNext.Size = new System.Drawing.Size(41, 12);
            this.lnkNext.TabIndex = 7;
            this.lnkNext.TabStop = true;
            this.lnkNext.Text = "下一页";
            this.lnkNext.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNext_LinkClicked);
            // 
            // lnkPre
            // 
            this.lnkPre.AutoSize = true;
            this.lnkPre.Enabled = false;
            this.lnkPre.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkPre.Location = new System.Drawing.Point(90, 257);
            this.lnkPre.Name = "lnkPre";
            this.lnkPre.Size = new System.Drawing.Size(41, 12);
            this.lnkPre.TabIndex = 7;
            this.lnkPre.TabStop = true;
            this.lnkPre.Text = "上一页";
            this.lnkPre.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPre_LinkClicked);
            // 
            // rbtnDesc
            // 
            this.rbtnDesc.AutoSize = true;
            this.rbtnDesc.Checked = true;
            this.rbtnDesc.Location = new System.Drawing.Point(347, 256);
            this.rbtnDesc.Name = "rbtnDesc";
            this.rbtnDesc.Size = new System.Drawing.Size(47, 16);
            this.rbtnDesc.TabIndex = 6;
            this.rbtnDesc.TabStop = true;
            this.rbtnDesc.Text = "最新";
            this.rbtnDesc.UseVisualStyleBackColor = true;
            // 
            // rbtnAsc
            // 
            this.rbtnAsc.AutoSize = true;
            this.rbtnAsc.Location = new System.Drawing.Point(298, 256);
            this.rbtnAsc.Name = "rbtnAsc";
            this.rbtnAsc.Size = new System.Drawing.Size(47, 16);
            this.rbtnAsc.TabIndex = 6;
            this.rbtnAsc.Text = "最先";
            this.rbtnAsc.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(257, 258);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 5;
            this.label12.Text = "排序：";
            // 
            // lbTip
            // 
            this.lbTip.AutoSize = true;
            this.lbTip.ForeColor = System.Drawing.Color.Red;
            this.lbTip.Location = new System.Drawing.Point(183, 258);
            this.lbTip.Name = "lbTip";
            this.lbTip.Size = new System.Drawing.Size(71, 12);
            this.lbTip.TabIndex = 5;
            this.lbTip.Text = "提示：...！";
            // 
            // btnGetShareHost
            // 
            this.btnGetShareHost.Location = new System.Drawing.Point(9, 251);
            this.btnGetShareHost.Name = "btnGetShareHost";
            this.btnGetShareHost.Size = new System.Drawing.Size(75, 23);
            this.btnGetShareHost.TabIndex = 1;
            this.btnGetShareHost.Text = "读取规则";
            this.btnGetShareHost.UseVisualStyleBackColor = true;
            this.btnGetShareHost.Click += new System.EventHandler(this.btnGetShareHost_Click);
            // 
            // gvShareList
            // 
            this.gvShareList.AllowUserToAddRows = false;
            this.gvShareList.AllowUserToDeleteRows = false;
            this.gvShareList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvShareList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvShareList.Dock = System.Windows.Forms.DockStyle.Top;
            this.gvShareList.Location = new System.Drawing.Point(3, 17);
            this.gvShareList.Name = "gvShareList";
            this.gvShareList.ReadOnly = true;
            this.gvShareList.RowTemplate.Height = 23;
            this.gvShareList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvShareList.Size = new System.Drawing.Size(512, 228);
            this.gvShareList.TabIndex = 0;
            this.gvShareList.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvShareList_CellMouseDoubleClick);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(366, 40);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(118, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "清除已设置的规则";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClear);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(9, 433);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(518, 70);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "其它说明：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(347, 12);
            this.label11.TabIndex = 5;
            this.label11.Text = "说明2：分享规则不会随软件关闭而删除，如要禁止，可以点击：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(275, 12);
            this.label10.TabIndex = 5;
            this.label10.Text = "说明1：添加分享规则后需要重启浏览器才能生效！";
            // 
            // MoreForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 516);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbAddRules);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MoreForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "更多分享规则 - 秋式广告杀手";
            this.Load += new System.EventHandler(this.MoreForm_Load);
            this.gbAddRules.ResumeLayout(false);
            this.gbAddRules.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvShareList)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbAddRules;
        private System.Windows.Forms.TextBox txtToIP;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chbShare;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView gvShareList;
        private System.Windows.Forms.Button btnGetShareHost;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel lnkOpenHosts;
        private System.Windows.Forms.Label lbTip;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.RadioButton rbtnDesc;
        private System.Windows.Forms.RadioButton rbtnAsc;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.LinkLabel lnkNext;
        private System.Windows.Forms.LinkLabel lnkPre;
        private System.Windows.Forms.TextBox txtAuthor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chbHidden;
    }
}