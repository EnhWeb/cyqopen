namespace CYQ.Data.ProjectTool
{
    partial class OpForm
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
            this.ddlDBType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtConn = new System.Windows.Forms.TextBox();
            this.btnTestConn = new System.Windows.Forms.Button();
            this.ddlName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chbMutilDatabase = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtProjectPath = new System.Windows.Forms.TextBox();
            this.gbConn = new System.Windows.Forms.GroupBox();
            this.gbBuild = new System.Windows.Forms.GroupBox();
            this.txtEntitySuffix = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.chbForTwoOnly = new System.Windows.Forms.CheckBox();
            this.chbValueTypeNullable = new System.Windows.Forms.CheckBox();
            this.btnOpenProjectFolder = new System.Windows.Forms.Button();
            this.txtNameSpace = new System.Windows.Forms.TextBox();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.btnBuild = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.ddlBuildMode = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lnkGotoUrl = new System.Windows.Forms.LinkLabel();
            this.lnkOpenFolder = new System.Windows.Forms.LinkLabel();
            this.lnkCopyPath = new System.Windows.Forms.LinkLabel();
            this.chbMapName = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gbConn.SuspendLayout();
            this.gbBuild.SuspendLayout();
            this.SuspendLayout();
            // 
            // ddlDBType
            // 
            this.ddlDBType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDBType.FormattingEnabled = true;
            this.ddlDBType.Items.AddRange(new object[] {
            "Mssql",
            "Oracle",
            "MySql",
            "SQLite",
            "Sybase",
            "Access/Excel(2003及以下)",
            "Access/Excel(2007及以上)",
            "Txt",
            "Xml"});
            this.ddlDBType.Location = new System.Drawing.Point(104, 47);
            this.ddlDBType.Name = "ddlDBType";
            this.ddlDBType.Size = new System.Drawing.Size(199, 20);
            this.ddlDBType.TabIndex = 0;
            this.ddlDBType.SelectedIndexChanged += new System.EventHandler(this.ddlProvider_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "数据库类型：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "链接字符串：";
            // 
            // txtConn
            // 
            this.txtConn.Location = new System.Drawing.Point(104, 73);
            this.txtConn.Name = "txtConn";
            this.txtConn.Size = new System.Drawing.Size(402, 21);
            this.txtConn.TabIndex = 2;
            // 
            // btnTestConn
            // 
            this.btnTestConn.Location = new System.Drawing.Point(417, 24);
            this.btnTestConn.Name = "btnTestConn";
            this.btnTestConn.Size = new System.Drawing.Size(75, 23);
            this.btnTestConn.TabIndex = 3;
            this.btnTestConn.Text = "测试链接";
            this.btnTestConn.UseVisualStyleBackColor = true;
            this.btnTestConn.Click += new System.EventHandler(this.btnTestConn_Click);
            // 
            // ddlName
            // 
            this.ddlName.FormattingEnabled = true;
            this.ddlName.Location = new System.Drawing.Point(104, 21);
            this.ddlName.Name = "ddlName";
            this.ddlName.Size = new System.Drawing.Size(199, 20);
            this.ddlName.TabIndex = 0;
            this.ddlName.SelectedIndexChanged += new System.EventHandler(this.ddlName_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "配置名称：";
            // 
            // chbMutilDatabase
            // 
            this.chbMutilDatabase.AutoSize = true;
            this.chbMutilDatabase.Location = new System.Drawing.Point(329, 24);
            this.chbMutilDatabase.Name = "chbMutilDatabase";
            this.chbMutilDatabase.Size = new System.Drawing.Size(120, 16);
            this.chbMutilDatabase.TabIndex = 4;
            this.chbMutilDatabase.Text = "项目有多个数据库";
            this.chbMutilDatabase.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "文件保存路径：";
            // 
            // txtProjectPath
            // 
            this.txtProjectPath.Location = new System.Drawing.Point(104, 119);
            this.txtProjectPath.Name = "txtProjectPath";
            this.txtProjectPath.Size = new System.Drawing.Size(306, 21);
            this.txtProjectPath.TabIndex = 2;
            // 
            // gbConn
            // 
            this.gbConn.Controls.Add(this.txtConn);
            this.gbConn.Controls.Add(this.ddlDBType);
            this.gbConn.Controls.Add(this.btnTestConn);
            this.gbConn.Controls.Add(this.ddlName);
            this.gbConn.Controls.Add(this.label1);
            this.gbConn.Controls.Add(this.label3);
            this.gbConn.Controls.Add(this.label2);
            this.gbConn.Location = new System.Drawing.Point(3, 12);
            this.gbConn.Name = "gbConn";
            this.gbConn.Size = new System.Drawing.Size(517, 100);
            this.gbConn.TabIndex = 5;
            this.gbConn.TabStop = false;
            this.gbConn.Text = "链接配置";
            // 
            // gbBuild
            // 
            this.gbBuild.Controls.Add(this.txtEntitySuffix);
            this.gbBuild.Controls.Add(this.label9);
            this.gbBuild.Controls.Add(this.label8);
            this.gbBuild.Controls.Add(this.chbMapName);
            this.gbBuild.Controls.Add(this.chbForTwoOnly);
            this.gbBuild.Controls.Add(this.chbValueTypeNullable);
            this.gbBuild.Controls.Add(this.btnOpenProjectFolder);
            this.gbBuild.Controls.Add(this.txtNameSpace);
            this.gbBuild.Controls.Add(this.btnOpenFolder);
            this.gbBuild.Controls.Add(this.btnBuild);
            this.gbBuild.Controls.Add(this.txtProjectPath);
            this.gbBuild.Controls.Add(this.label4);
            this.gbBuild.Controls.Add(this.chbMutilDatabase);
            this.gbBuild.Controls.Add(this.label7);
            this.gbBuild.Controls.Add(this.ddlBuildMode);
            this.gbBuild.Controls.Add(this.label5);
            this.gbBuild.Location = new System.Drawing.Point(3, 118);
            this.gbBuild.Name = "gbBuild";
            this.gbBuild.Size = new System.Drawing.Size(517, 179);
            this.gbBuild.TabIndex = 6;
            this.gbBuild.TabStop = false;
            this.gbBuild.Text = "生成代码文件";
            // 
            // txtEntitySuffix
            // 
            this.txtEntitySuffix.Location = new System.Drawing.Point(104, 50);
            this.txtEntitySuffix.Name = "txtEntitySuffix";
            this.txtEntitySuffix.Size = new System.Drawing.Size(97, 21);
            this.txtEntitySuffix.TabIndex = 11;
            this.txtEntitySuffix.Text = "Bean";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 10;
            this.label9.Text = "实体类后缀：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(314, 88);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(125, 12);
            this.label8.TabIndex = 9;
            this.label8.Text = "{0} - 代表数据库名称";
            // 
            // chbForTwoOnly
            // 
            this.chbForTwoOnly.AutoSize = true;
            this.chbForTwoOnly.Checked = true;
            this.chbForTwoOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbForTwoOnly.Location = new System.Drawing.Point(329, 55);
            this.chbForTwoOnly.Name = "chbForTwoOnly";
            this.chbForTwoOnly.Size = new System.Drawing.Size(84, 16);
            this.chbForTwoOnly.TabIndex = 8;
            this.chbForTwoOnly.Text = "兼容vs2005";
            this.chbForTwoOnly.UseVisualStyleBackColor = true;
            this.chbForTwoOnly.Visible = false;
            // 
            // chbValueTypeNullable
            // 
            this.chbValueTypeNullable.AutoSize = true;
            this.chbValueTypeNullable.Location = new System.Drawing.Point(207, 55);
            this.chbValueTypeNullable.Name = "chbValueTypeNullable";
            this.chbValueTypeNullable.Size = new System.Drawing.Size(96, 16);
            this.chbValueTypeNullable.TabIndex = 8;
            this.chbValueTypeNullable.Text = "值类型可Null";
            this.chbValueTypeNullable.UseVisualStyleBackColor = true;
            this.chbValueTypeNullable.Visible = false;
            // 
            // btnOpenProjectFolder
            // 
            this.btnOpenProjectFolder.Location = new System.Drawing.Point(466, 118);
            this.btnOpenProjectFolder.Name = "btnOpenProjectFolder";
            this.btnOpenProjectFolder.Size = new System.Drawing.Size(45, 23);
            this.btnOpenProjectFolder.TabIndex = 7;
            this.btnOpenProjectFolder.Text = "open";
            this.btnOpenProjectFolder.UseVisualStyleBackColor = true;
            this.btnOpenProjectFolder.Click += new System.EventHandler(this.btnOpenProjectFolder_Click);
            // 
            // txtNameSpace
            // 
            this.txtNameSpace.Location = new System.Drawing.Point(104, 85);
            this.txtNameSpace.Name = "txtNameSpace";
            this.txtNameSpace.Size = new System.Drawing.Size(199, 21);
            this.txtNameSpace.TabIndex = 2;
            this.txtNameSpace.Text = "Web.Enum.{0}";
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Location = new System.Drawing.Point(417, 119);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(42, 23);
            this.btnOpenFolder.TabIndex = 6;
            this.btnOpenFolder.Text = "...";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(205, 146);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(75, 23);
            this.btnBuild.TabIndex = 5;
            this.btnBuild.Text = "生成文件";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "默认名称空间：";
            // 
            // ddlBuildMode
            // 
            this.ddlBuildMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlBuildMode.FormattingEnabled = true;
            this.ddlBuildMode.Items.AddRange(new object[] {
            "枚举型（MAction/MProc）- 推荐",
            "实体型（ORM操作方式）",
            "纯实体类"});
            this.ddlBuildMode.Location = new System.Drawing.Point(104, 20);
            this.ddlBuildMode.Name = "ddlBuildMode";
            this.ddlBuildMode.Size = new System.Drawing.Size(199, 20);
            this.ddlBuildMode.TabIndex = 0;
            this.ddlBuildMode.SelectedIndexChanged += new System.EventHandler(this.ddlBuildMode_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "编码模式：";
            // 
            // lnkGotoUrl
            // 
            this.lnkGotoUrl.AutoSize = true;
            this.lnkGotoUrl.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkGotoUrl.LinkColor = System.Drawing.Color.Red;
            this.lnkGotoUrl.Location = new System.Drawing.Point(443, 306);
            this.lnkGotoUrl.Name = "lnkGotoUrl";
            this.lnkGotoUrl.Size = new System.Drawing.Size(77, 12);
            this.lnkGotoUrl.TabIndex = 7;
            this.lnkGotoUrl.TabStop = true;
            this.lnkGotoUrl.Text = "源码下载地址";
            this.lnkGotoUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGotoUrl_LinkClicked);
            // 
            // lnkOpenFolder
            // 
            this.lnkOpenFolder.AutoSize = true;
            this.lnkOpenFolder.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkOpenFolder.LinkColor = System.Drawing.Color.Blue;
            this.lnkOpenFolder.Location = new System.Drawing.Point(360, 306);
            this.lnkOpenFolder.Name = "lnkOpenFolder";
            this.lnkOpenFolder.Size = new System.Drawing.Size(77, 12);
            this.lnkOpenFolder.TabIndex = 7;
            this.lnkOpenFolder.TabStop = true;
            this.lnkOpenFolder.Text = "打开软件目录";
            this.lnkOpenFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOpenFolder_LinkClicked);
            // 
            // lnkCopyPath
            // 
            this.lnkCopyPath.AutoSize = true;
            this.lnkCopyPath.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkCopyPath.LinkColor = System.Drawing.Color.BlueViolet;
            this.lnkCopyPath.Location = new System.Drawing.Point(275, 306);
            this.lnkCopyPath.Name = "lnkCopyPath";
            this.lnkCopyPath.Size = new System.Drawing.Size(77, 12);
            this.lnkCopyPath.TabIndex = 8;
            this.lnkCopyPath.TabStop = true;
            this.lnkCopyPath.Text = "复制完整路径";
            this.lnkCopyPath.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCopyPath_LinkClicked);
            // 
            // chbMapName
            // 
            this.chbMapName.AutoSize = true;
            this.chbMapName.Checked = true;
            this.chbMapName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbMapName.Location = new System.Drawing.Point(417, 55);
            this.chbMapName.Name = "chbMapName";
            this.chbMapName.Size = new System.Drawing.Size(102, 16);
            this.chbMapName.TabIndex = 8;
            this.chbMapName.Text = "去除‘_’符号";
            this.toolTip1.SetToolTip(this.chbMapName, "按Pascal大小写规范格式化");
            this.chbMapName.UseVisualStyleBackColor = true;
            this.chbMapName.Visible = false;
            // 
            // OpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 326);
            this.Controls.Add(this.lnkCopyPath);
            this.Controls.Add(this.lnkOpenFolder);
            this.Controls.Add(this.lnkGotoUrl);
            this.Controls.Add(this.gbBuild);
            this.Controls.Add(this.gbConn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "OpForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CYQ.Data 项目配置";
            this.Load += new System.EventHandler(this.OpForm_Load);
            this.gbConn.ResumeLayout(false);
            this.gbConn.PerformLayout();
            this.gbBuild.ResumeLayout(false);
            this.gbBuild.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ddlDBType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtConn;
        private System.Windows.Forms.Button btnTestConn;
        private System.Windows.Forms.ComboBox ddlName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chbMutilDatabase;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtProjectPath;
        private System.Windows.Forms.GroupBox gbConn;
        private System.Windows.Forms.GroupBox gbBuild;
        private System.Windows.Forms.ComboBox ddlBuildMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.TextBox txtNameSpace;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel lnkGotoUrl;
        private System.Windows.Forms.LinkLabel lnkOpenFolder;
        private System.Windows.Forms.Button btnOpenProjectFolder;
        private System.Windows.Forms.LinkLabel lnkCopyPath;
        private System.Windows.Forms.CheckBox chbValueTypeNullable;
        private System.Windows.Forms.CheckBox chbForTwoOnly;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtEntitySuffix;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chbMapName;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

