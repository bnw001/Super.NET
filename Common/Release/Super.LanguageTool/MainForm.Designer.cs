namespace Super.LanguageTool
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SetDir = new System.Windows.Forms.ToolStripMenuItem();
            this.BuildDotNetPack = new System.Windows.Forms.ToolStripMenuItem();
            this.BuildJavaPack = new System.Windows.Forms.ToolStripMenuItem();
            this.BuildPHPPack = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(930, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SetDir,
            this.BuildDotNetPack,
            this.BuildJavaPack,
            this.BuildPHPPack});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(58, 21);
            this.FileMenu.Text = "文件(&F)";
            // 
            // SetDir
            // 
            this.SetDir.Name = "SetDir";
            this.SetDir.Size = new System.Drawing.Size(181, 22);
            this.SetDir.Text = "设置目录(&S)";
            this.SetDir.Click += new System.EventHandler(this.SetDir_Click);
            // 
            // BuildDotNetPack
            // 
            this.BuildDotNetPack.Name = "BuildDotNetPack";
            this.BuildDotNetPack.Size = new System.Drawing.Size(181, 22);
            this.BuildDotNetPack.Text = "生成.NET语言包(&N)";
            // 
            // BuildJavaPack
            // 
            this.BuildJavaPack.Name = "BuildJavaPack";
            this.BuildJavaPack.Size = new System.Drawing.Size(181, 22);
            this.BuildJavaPack.Text = "生成Java语言包(&J)";
            // 
            // BuildPHPPack
            // 
            this.BuildPHPPack.Name = "BuildPHPPack";
            this.BuildPHPPack.Size = new System.Drawing.Size(181, 22);
            this.BuildPHPPack.Text = "生成PHP语言包(&P)";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 432);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "多语言辅助工具";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem SetDir;
        private System.Windows.Forms.ToolStripMenuItem BuildDotNetPack;
        private System.Windows.Forms.ToolStripMenuItem BuildJavaPack;
        private System.Windows.Forms.ToolStripMenuItem BuildPHPPack;
        private System.Windows.Forms.FolderBrowserDialog SelectFolder;
    }
}

