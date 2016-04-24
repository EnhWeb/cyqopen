namespace AdKiller
{
    partial class DetailForm
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("aaaa");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("bbbb");
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvUrlDetail = new System.Windows.Forms.ListView();
            this.State = new System.Windows.Forms.ColumnHeader();
            this.Protocol = new System.Windows.Forms.ColumnHeader();
            this.Host = new System.Windows.Forms.ColumnHeader();
            this.Url = new System.Windows.Forms.ColumnHeader();
            this.Length = new System.Windows.Forms.ColumnHeader();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvUrlDetail);
            this.splitContainer1.Size = new System.Drawing.Size(837, 443);
            this.splitContainer1.SplitterDistance = 279;
            this.splitContainer1.TabIndex = 0;
            // 
            // lvUrlDetail
            // 
            this.lvUrlDetail.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.State,
            this.Protocol,
            this.Host,
            this.Url,
            this.Length});
            this.lvUrlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvUrlDetail.GridLines = true;
            listViewGroup1.Header = "ListViewGroup";
            listViewGroup1.Name = "listViewGroup1";
            listViewGroup2.Header = "ListViewGroup";
            listViewGroup2.Name = "listViewGroup2";
            listViewGroup3.Header = "ListViewGroup";
            listViewGroup3.Name = "listViewGroup3";
            this.lvUrlDetail.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.lvUrlDetail.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.lvUrlDetail.Location = new System.Drawing.Point(0, 0);
            this.lvUrlDetail.Name = "lvUrlDetail";
            this.lvUrlDetail.ShowGroups = false;
            this.lvUrlDetail.Size = new System.Drawing.Size(279, 443);
            this.lvUrlDetail.TabIndex = 0;
            this.lvUrlDetail.UseCompatibleStateImageBehavior = false;
            this.lvUrlDetail.View = System.Windows.Forms.View.Details;
            // 
            // State
            // 
            this.State.Text = "状态";
            // 
            // Protocol
            // 
            this.Protocol.Text = "协议";
            // 
            // Host
            // 
            this.Host.Text = "主机";
            // 
            // Url
            // 
            this.Url.Text = "Url";
            // 
            // Length
            // 
            this.Length.Text = "大小";
            // 
            // DetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 443);
            this.Controls.Add(this.splitContainer1);
            this.Name = "DetailForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "URL明细";
            this.Load += new System.EventHandler(this.DetailForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lvUrlDetail;
        private System.Windows.Forms.ColumnHeader State;
        private System.Windows.Forms.ColumnHeader Protocol;
        private System.Windows.Forms.ColumnHeader Host;
        private System.Windows.Forms.ColumnHeader Url;
        private System.Windows.Forms.ColumnHeader Length;
    }
}