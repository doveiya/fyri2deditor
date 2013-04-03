namespace Fyri2dEditor
{
    partial class ContentManagerForm
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
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadModelToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTexture2dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadEffectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSpriterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.projectContentTV = new System.Windows.Forms.TreeView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.modelTabPage = new System.Windows.Forms.TabPage();
            this.texture2dTabPage = new System.Windows.Forms.TabPage();
            this.fontTabPage = new System.Windows.Forms.TabPage();
            this.effectTabPage = new System.Windows.Forms.TabPage();
            this.spriterTabPage = new System.Windows.Forms.TabPage();
            this.modelViewerControl = new Fyri2dEditor.ModelViewerControl();
            this.texture2dViewerControl = new Fyri2dEditor.Texture2dViewerControl();
            this.fontViewerControl = new Fyri2dEditor.FontViewerControl();
            this.effectViewerControl = new Fyri2dEditor.EffectViewerControl();
            this.spriterViewerControl = new Fyri2dEditor.SpriterViewerControl();
            this.closeProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.modelTabPage.SuspendLayout();
            this.texture2dTabPage.SuspendLayout();
            this.fontTabPage.SuspendLayout();
            this.effectTabPage.SuspendLayout();
            this.spriterTabPage.SuspendLayout();
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
            this.loadSpriterToolStripMenuItem});
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // loadModelToolStripMenuItem1
            // 
            this.loadModelToolStripMenuItem1.Name = "loadModelToolStripMenuItem1";
            this.loadModelToolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
            this.loadModelToolStripMenuItem1.Text = "Load Model...";
            this.loadModelToolStripMenuItem1.Click += new System.EventHandler(this.loadModelToolStripMenuItem1_Click);
            // 
            // loadTexture2dToolStripMenuItem
            // 
            this.loadTexture2dToolStripMenuItem.Name = "loadTexture2dToolStripMenuItem";
            this.loadTexture2dToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.loadTexture2dToolStripMenuItem.Text = "Load Texture2d...";
            this.loadTexture2dToolStripMenuItem.Click += new System.EventHandler(this.loadTexture2dToolStripMenuItem_Click);
            // 
            // loadFontToolStripMenuItem
            // 
            this.loadFontToolStripMenuItem.Name = "loadFontToolStripMenuItem";
            this.loadFontToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.loadFontToolStripMenuItem.Text = "Load Font...";
            this.loadFontToolStripMenuItem.Click += new System.EventHandler(this.loadFontToolStripMenuItem_Click);
            // 
            // loadEffectToolStripMenuItem
            // 
            this.loadEffectToolStripMenuItem.Name = "loadEffectToolStripMenuItem";
            this.loadEffectToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.loadEffectToolStripMenuItem.Text = "Load Effect...";
            this.loadEffectToolStripMenuItem.Click += new System.EventHandler(this.loadEffectToolStripMenuItem_Click);
            // 
            // loadSpriterToolStripMenuItem
            // 
            this.loadSpriterToolStripMenuItem.Name = "loadSpriterToolStripMenuItem";
            this.loadSpriterToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.loadSpriterToolStripMenuItem.Text = "Load Spriter...";
            this.loadSpriterToolStripMenuItem.Click += new System.EventHandler(this.loadSpriterToolStripMenuItem_Click);
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
            this.tabControl1.Controls.Add(this.texture2dTabPage);
            this.tabControl1.Controls.Add(this.fontTabPage);
            this.tabControl1.Controls.Add(this.effectTabPage);
            this.tabControl1.Controls.Add(this.spriterTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(682, 549);
            this.tabControl1.TabIndex = 0;
            // 
            // modelTabPage
            // 
            this.modelTabPage.Controls.Add(this.modelViewerControl);
            this.modelTabPage.Location = new System.Drawing.Point(4, 22);
            this.modelTabPage.Name = "modelTabPage";
            this.modelTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.modelTabPage.Size = new System.Drawing.Size(674, 523);
            this.modelTabPage.TabIndex = 0;
            this.modelTabPage.Text = "Model Viewer";
            this.modelTabPage.UseVisualStyleBackColor = true;
            // 
            // texture2dTabPage
            // 
            this.texture2dTabPage.Controls.Add(this.texture2dViewerControl);
            this.texture2dTabPage.Location = new System.Drawing.Point(4, 22);
            this.texture2dTabPage.Name = "texture2dTabPage";
            this.texture2dTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.texture2dTabPage.Size = new System.Drawing.Size(674, 523);
            this.texture2dTabPage.TabIndex = 1;
            this.texture2dTabPage.Text = "Texture Viewer";
            this.texture2dTabPage.UseVisualStyleBackColor = true;
            // 
            // fontTabPage
            // 
            this.fontTabPage.Controls.Add(this.fontViewerControl);
            this.fontTabPage.Location = new System.Drawing.Point(4, 22);
            this.fontTabPage.Name = "fontTabPage";
            this.fontTabPage.Size = new System.Drawing.Size(674, 523);
            this.fontTabPage.TabIndex = 2;
            this.fontTabPage.Text = "Font Viewer";
            this.fontTabPage.UseVisualStyleBackColor = true;
            // 
            // effectTabPage
            // 
            this.effectTabPage.Controls.Add(this.effectViewerControl);
            this.effectTabPage.Location = new System.Drawing.Point(4, 22);
            this.effectTabPage.Name = "effectTabPage";
            this.effectTabPage.Size = new System.Drawing.Size(674, 523);
            this.effectTabPage.TabIndex = 3;
            this.effectTabPage.Text = "Effect Viewer";
            this.effectTabPage.UseVisualStyleBackColor = true;
            // 
            // spriterTabPage
            // 
            this.spriterTabPage.Controls.Add(this.spriterViewerControl);
            this.spriterTabPage.Location = new System.Drawing.Point(4, 22);
            this.spriterTabPage.Name = "spriterTabPage";
            this.spriterTabPage.Size = new System.Drawing.Size(674, 523);
            this.spriterTabPage.TabIndex = 4;
            this.spriterTabPage.Text = "Spriter Viewer";
            this.spriterTabPage.UseVisualStyleBackColor = true;
            // 
            // modelViewerControl
            // 
            this.modelViewerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelViewerControl.Location = new System.Drawing.Point(3, 3);
            this.modelViewerControl.Model = null;
            this.modelViewerControl.Name = "modelViewerControl";
            this.modelViewerControl.Size = new System.Drawing.Size(668, 517);
            this.modelViewerControl.TabIndex = 0;
            this.modelViewerControl.Text = "modelViewerControl";
            // 
            // texture2dViewerControl
            // 
            this.texture2dViewerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texture2dViewerControl.Location = new System.Drawing.Point(3, 3);
            this.texture2dViewerControl.Name = "texture2dViewerControl";
            this.texture2dViewerControl.Size = new System.Drawing.Size(668, 517);
            this.texture2dViewerControl.TabIndex = 0;
            this.texture2dViewerControl.Text = "texture2dViewerControl1";
            this.texture2dViewerControl.Texture = null;
            // 
            // fontViewerControl
            // 
            this.fontViewerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fontViewerControl.Location = new System.Drawing.Point(0, 0);
            this.fontViewerControl.Name = "fontViewerControl";
            this.fontViewerControl.Size = new System.Drawing.Size(674, 523);
            this.fontViewerControl.SpriteFont = null;
            this.fontViewerControl.TabIndex = 0;
            this.fontViewerControl.Text = "fontViewerControl";
            // 
            // effectViewerControl
            // 
            this.effectViewerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.effectViewerControl.Effect = null;
            this.effectViewerControl.Location = new System.Drawing.Point(0, 0);
            this.effectViewerControl.Model = null;
            this.effectViewerControl.Name = "effectViewerControl";
            this.effectViewerControl.Size = new System.Drawing.Size(674, 523);
            this.effectViewerControl.TabIndex = 0;
            this.effectViewerControl.Text = "effectViewerControl";
            // 
            // spriterViewerControl
            // 
            this.spriterViewerControl.Character = null;
            this.spriterViewerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spriterViewerControl.Location = new System.Drawing.Point(0, 0);
            this.spriterViewerControl.Name = "spriterViewerControl";
            this.spriterViewerControl.Size = new System.Drawing.Size(674, 523);
            this.spriterViewerControl.TabIndex = 0;
            this.spriterViewerControl.Text = "spriterViewerControl";
            // 
            // closeProjectToolStripMenuItem
            // 
            this.closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
            this.closeProjectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeProjectToolStripMenuItem.Text = "Close Project";
            this.closeProjectToolStripMenuItem.Click += new System.EventHandler(this.closeProjectToolStripMenuItem_Click);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveProjectToolStripMenuItem.Text = "Save Project";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openProjectToolStripMenuItem.Text = "Open Project...";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newProjectToolStripMenuItem.Text = "New Project";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
            // 
            // ContentManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 573);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ContentManagerForm";
            this.Text = "WinForms Content Loading";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.modelTabPage.ResumeLayout(false);
            this.texture2dTabPage.ResumeLayout(false);
            this.fontTabPage.ResumeLayout(false);
            this.effectTabPage.ResumeLayout(false);
            this.spriterTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView projectContentTV;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage modelTabPage;
        private ModelViewerControl modelViewerControl;
        private System.Windows.Forms.TabPage texture2dTabPage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadModelToolStripMenuItem1;
        private Texture2dViewerControl texture2dViewerControl;
        private System.Windows.Forms.ToolStripMenuItem loadTexture2dToolStripMenuItem;
        private System.Windows.Forms.TabPage fontTabPage;
        private System.Windows.Forms.ToolStripMenuItem loadFontToolStripMenuItem;
        private FontViewerControl fontViewerControl;
        private System.Windows.Forms.TabPage effectTabPage;
        private EffectViewerControl effectViewerControl;
        private System.Windows.Forms.ToolStripMenuItem loadEffectToolStripMenuItem;
        private System.Windows.Forms.TabPage spriterTabPage;
        private System.Windows.Forms.ToolStripMenuItem loadSpriterToolStripMenuItem;
        private SpriterViewerControl spriterViewerControl;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeProjectToolStripMenuItem;

    }
}

