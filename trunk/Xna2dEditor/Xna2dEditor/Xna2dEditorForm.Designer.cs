namespace Fyri2dEditor
{
    partial class Xna2dEditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Project Name");
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadModelToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTexture2dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadEffectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSpriteFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.projectContentTV = new System.Windows.Forms.TreeView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.modelTabPage = new System.Windows.Forms.TabPage();
            this.line2dDemoViewerControl = new Fyri2dEditor.Line2dDemoViewerControl();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.modelTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.loadToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(940, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.openProjectToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.closeProjectToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newProjectToolStripMenuItem.Text = "New Project";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openProjectToolStripMenuItem.Text = "Open Project...";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveProjectToolStripMenuItem.Text = "Save Project";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
            // 
            // closeProjectToolStripMenuItem
            // 
            this.closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
            this.closeProjectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeProjectToolStripMenuItem.Text = "Close Project";
            this.closeProjectToolStripMenuItem.Click += new System.EventHandler(this.closeProjectToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenuClicked);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadModelToolStripMenuItem1,
            this.loadTexture2dToolStripMenuItem,
            this.loadFontToolStripMenuItem,
            this.loadEffectToolStripMenuItem,
            this.loadSpriteFontToolStripMenuItem});
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // loadModelToolStripMenuItem1
            // 
            this.loadModelToolStripMenuItem1.Name = "loadModelToolStripMenuItem1";
            this.loadModelToolStripMenuItem1.Size = new System.Drawing.Size(166, 22);
            this.loadModelToolStripMenuItem1.Text = "Load Model...";
            this.loadModelToolStripMenuItem1.Click += new System.EventHandler(this.LoadModelClicked);
            // 
            // loadTexture2dToolStripMenuItem
            // 
            this.loadTexture2dToolStripMenuItem.Name = "loadTexture2dToolStripMenuItem";
            this.loadTexture2dToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.loadTexture2dToolStripMenuItem.Text = "Load Texture2d...";
            this.loadTexture2dToolStripMenuItem.Click += new System.EventHandler(this.loadTexture2dToolStripMenuItem_Click);
            // 
            // loadFontToolStripMenuItem
            // 
            this.loadFontToolStripMenuItem.Name = "loadFontToolStripMenuItem";
            this.loadFontToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.loadFontToolStripMenuItem.Text = "Load Font...";
            this.loadFontToolStripMenuItem.Click += new System.EventHandler(this.loadFontToolStripMenuItem_Click);
            // 
            // loadEffectToolStripMenuItem
            // 
            this.loadEffectToolStripMenuItem.Name = "loadEffectToolStripMenuItem";
            this.loadEffectToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.loadEffectToolStripMenuItem.Text = "Load Effect...";
            this.loadEffectToolStripMenuItem.Click += new System.EventHandler(this.loadEffectToolStripMenuItem_Click);
            // 
            // loadSpriteFontToolStripMenuItem
            // 
            this.loadSpriteFontToolStripMenuItem.Name = "loadSpriteFontToolStripMenuItem";
            this.loadSpriteFontToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.loadSpriteFontToolStripMenuItem.Text = "Load SpriteFont...";
            this.loadSpriteFontToolStripMenuItem.Click += new System.EventHandler(this.loadSpriteFontToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.projectContentTV);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(940, 549);
            this.splitContainer1.SplitterDistance = 254;
            this.splitContainer1.TabIndex = 1;
            // 
            // projectContentTV
            // 
            this.projectContentTV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectContentTV.Location = new System.Drawing.Point(0, 0);
            this.projectContentTV.Name = "projectContentTV";
            treeNode1.Name = "ProjectNameNode";
            treeNode1.Text = "Project Name";
            this.projectContentTV.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.projectContentTV.Size = new System.Drawing.Size(254, 549);
            this.projectContentTV.TabIndex = 0;
            this.projectContentTV.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.projectContentTV_AfterSelect);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.modelTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(682, 549);
            this.tabControl1.TabIndex = 0;
            // 
            // modelTabPage
            // 
            this.modelTabPage.Controls.Add(this.line2dDemoViewerControl);
            this.modelTabPage.Location = new System.Drawing.Point(4, 22);
            this.modelTabPage.Name = "modelTabPage";
            this.modelTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.modelTabPage.Size = new System.Drawing.Size(674, 523);
            this.modelTabPage.TabIndex = 0;
            this.modelTabPage.Text = "RoundLineDemo";
            this.modelTabPage.UseVisualStyleBackColor = true;
            // 
            // line2dDemoViewerControl
            // 
            this.line2dDemoViewerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.line2dDemoViewerControl.Effect = null;
            this.line2dDemoViewerControl.LineBatch = null;
            this.line2dDemoViewerControl.Location = new System.Drawing.Point(3, 3);
            this.line2dDemoViewerControl.Name = "line2dDemoViewerControl";
            this.line2dDemoViewerControl.Size = new System.Drawing.Size(668, 517);
            this.line2dDemoViewerControl.SpriteFont = null;
            this.line2dDemoViewerControl.TabIndex = 0;
            this.line2dDemoViewerControl.Text = "line2dDemoViewerControl";
            // 
            // Line2dDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 573);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Line2dDemoForm";
            this.Text = "WinForms Content Loading";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.modelTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView projectContentTV;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadModelToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem loadTexture2dToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadEffectToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage modelTabPage;
        private System.Windows.Forms.ToolStripMenuItem loadSpriteFontToolStripMenuItem;
        private Line2dDemoViewerControl line2dDemoViewerControl;

    }
}

