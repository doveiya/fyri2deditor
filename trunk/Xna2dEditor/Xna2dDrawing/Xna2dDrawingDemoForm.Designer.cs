namespace Fyri2dEditor
{
    partial class Xna2dDrawingDemoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Xna2dDrawingDemoForm));
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
            this.editorToolStrip = new System.Windows.Forms.ToolStrip();
            this.selectTSB = new System.Windows.Forms.ToolStripButton();
            this.lineTSB = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.modelTabPage = new System.Windows.Forms.TabPage();
            this.xna2dDrawingDemo3VC = new Fyri2dEditor.Xna2dDrawingDemo3VC();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.editorToolStrip.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.editorToolStrip);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(940, 549);
            this.splitContainer1.SplitterDistance = 65;
            this.splitContainer1.TabIndex = 1;
            // 
            // editorToolStrip
            // 
            this.editorToolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectTSB,
            this.lineTSB});
            this.editorToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.editorToolStrip.Location = new System.Drawing.Point(0, 0);
            this.editorToolStrip.Name = "editorToolStrip";
            this.editorToolStrip.Size = new System.Drawing.Size(65, 549);
            this.editorToolStrip.TabIndex = 0;
            this.editorToolStrip.Text = "editorToolStrip";
            // 
            // selectTSB
            // 
            this.selectTSB.Image = ((System.Drawing.Image)(resources.GetObject("selectTSB.Image")));
            this.selectTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectTSB.Name = "selectTSB";
            this.selectTSB.Size = new System.Drawing.Size(63, 35);
            this.selectTSB.Text = "Select";
            this.selectTSB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // lineTSB
            // 
            this.lineTSB.Image = ((System.Drawing.Image)(resources.GetObject("lineTSB.Image")));
            this.lineTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lineTSB.Name = "lineTSB";
            this.lineTSB.Size = new System.Drawing.Size(63, 35);
            this.lineTSB.Text = "Line";
            this.lineTSB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.modelTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(871, 549);
            this.tabControl1.TabIndex = 0;
            // 
            // modelTabPage
            // 
            this.modelTabPage.Controls.Add(this.xna2dDrawingDemo3VC);
            this.modelTabPage.Location = new System.Drawing.Point(4, 22);
            this.modelTabPage.Name = "modelTabPage";
            this.modelTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.modelTabPage.Size = new System.Drawing.Size(863, 523);
            this.modelTabPage.TabIndex = 0;
            this.modelTabPage.Text = "RoundLineDemo";
            this.modelTabPage.UseVisualStyleBackColor = true;
            // 
            // xna2dDrawingDemo3VC
            // 
            this.xna2dDrawingDemo3VC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xna2dDrawingDemo3VC.DrawingBatch = null;
            this.xna2dDrawingDemo3VC.DrawingContext = null;
            this.xna2dDrawingDemo3VC.Effect = null;
            this.xna2dDrawingDemo3VC.LineBatch = null;
            this.xna2dDrawingDemo3VC.Location = new System.Drawing.Point(3, 3);
            this.xna2dDrawingDemo3VC.Name = "xna2dDrawingDemo3VC";
            this.xna2dDrawingDemo3VC.Size = new System.Drawing.Size(857, 517);
            this.xna2dDrawingDemo3VC.SpriteFont = null;
            this.xna2dDrawingDemo3VC.TabIndex = 0;
            this.xna2dDrawingDemo3VC.Text = "xna2dDrawingDemo3VC";
            this.xna2dDrawingDemo3VC.Texture = null;
            this.xna2dDrawingDemo3VC.ContextChanged += new Fyri2dEditor.ContextEventHandler(this.xna2dDrawingDemo3VC_ContextChanged);
            // 
            // Xna2dDrawingDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 573);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Xna2dDrawingDemoForm";
            this.Text = "WinForms Content Loading";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.editorToolStrip.ResumeLayout(false);
            this.editorToolStrip.PerformLayout();
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
        private System.Windows.Forms.ToolStrip editorToolStrip;
        private System.Windows.Forms.ToolStripButton selectTSB;
        private System.Windows.Forms.ToolStripButton lineTSB;
        private Xna2dDrawingDemo3VC xna2dDrawingDemo3VC;

    }
}

