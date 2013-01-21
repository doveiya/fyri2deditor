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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Xna2dEditorForm));
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.leftRulerControl = new Lyquidity.UtilityLibrary.Controls.RulerControl();
            this.topRulerControl = new Lyquidity.UtilityLibrary.Controls.RulerControl();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.editorToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.loadToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1245, 24);
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
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1245, 623);
            this.splitContainer1.SplitterDistance = 85;
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
            this.editorToolStrip.Size = new System.Drawing.Size(85, 623);
            this.editorToolStrip.TabIndex = 0;
            this.editorToolStrip.Text = "editorToolStrip";
            // 
            // selectTSB
            // 
            this.selectTSB.Image = ((System.Drawing.Image)(resources.GetObject("selectTSB.Image")));
            this.selectTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectTSB.Name = "selectTSB";
            this.selectTSB.Size = new System.Drawing.Size(83, 35);
            this.selectTSB.Text = "Select";
            this.selectTSB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // lineTSB
            // 
            this.lineTSB.Image = ((System.Drawing.Image)(resources.GetObject("lineTSB.Image")));
            this.lineTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lineTSB.Name = "lineTSB";
            this.lineTSB.Size = new System.Drawing.Size(83, 35);
            this.lineTSB.Text = "Line";
            this.lineTSB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel1);
            this.splitContainer2.Panel1.Controls.Add(this.leftRulerControl);
            this.splitContainer2.Panel1.Controls.Add(this.topRulerControl);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(1156, 623);
            this.splitContainer2.SplitterDistance = 828;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainer3.Size = new System.Drawing.Size(324, 623);
            this.splitContainer3.SplitterDistance = 309;
            this.splitContainer3.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 625);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1245, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(90, 17);
            this.toolStripStatusLabel1.Text = "My Status Label";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(28, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 600);
            this.panel1.TabIndex = 3;
            // 
            // leftRulerControl
            // 
            this.leftRulerControl.ActualSize = true;
            this.leftRulerControl.DivisionMarkFactor = 5;
            this.leftRulerControl.Divisions = 10;
            this.leftRulerControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftRulerControl.ForeColor = System.Drawing.Color.Black;
            this.leftRulerControl.Location = new System.Drawing.Point(0, 23);
            this.leftRulerControl.MajorInterval = 100;
            this.leftRulerControl.MiddleMarkFactor = 3;
            this.leftRulerControl.MouseTrackingOn = true;
            this.leftRulerControl.Name = "leftRulerControl";
            this.leftRulerControl.Orientation = Lyquidity.UtilityLibrary.Controls.enumOrientation.orVertical;
            this.leftRulerControl.RulerAlignment = Lyquidity.UtilityLibrary.Controls.enumRulerAlignment.raMiddle;
            this.leftRulerControl.ScaleMode = Lyquidity.UtilityLibrary.Controls.enumScaleMode.smPoints;
            this.leftRulerControl.Size = new System.Drawing.Size(28, 600);
            this.leftRulerControl.StartValue = 0D;
            this.leftRulerControl.TabIndex = 2;
            this.leftRulerControl.Text = "rulerControl1";
            this.leftRulerControl.VerticalNumbers = false;
            this.leftRulerControl.ZoomFactor = 1D;
            // 
            // topRulerControl
            // 
            this.topRulerControl.ActualSize = true;
            this.topRulerControl.DivisionMarkFactor = 5;
            this.topRulerControl.Divisions = 10;
            this.topRulerControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.topRulerControl.ForeColor = System.Drawing.Color.Black;
            this.topRulerControl.Location = new System.Drawing.Point(0, 0);
            this.topRulerControl.MajorInterval = 100;
            this.topRulerControl.MiddleMarkFactor = 3;
            this.topRulerControl.MouseTrackingOn = true;
            this.topRulerControl.Name = "topRulerControl";
            this.topRulerControl.Orientation = Lyquidity.UtilityLibrary.Controls.enumOrientation.orHorizontal;
            this.topRulerControl.RulerAlignment = Lyquidity.UtilityLibrary.Controls.enumRulerAlignment.raBottomOrRight;
            this.topRulerControl.ScaleMode = Lyquidity.UtilityLibrary.Controls.enumScaleMode.smPoints;
            this.topRulerControl.Size = new System.Drawing.Size(828, 23);
            this.topRulerControl.StartValue = 0D;
            this.topRulerControl.TabIndex = 1;
            this.topRulerControl.Text = "rulerControl1";
            this.topRulerControl.VerticalNumbers = true;
            this.topRulerControl.ZoomFactor = 1D;
            // 
            // Xna2dEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1245, 647);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Xna2dEditorForm";
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
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem loadSpriteFontToolStripMenuItem;
        private System.Windows.Forms.ToolStrip editorToolStrip;
        private System.Windows.Forms.ToolStripButton selectTSB;
        private System.Windows.Forms.ToolStripButton lineTSB;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private Lyquidity.UtilityLibrary.Controls.RulerControl topRulerControl;
        private Lyquidity.UtilityLibrary.Controls.RulerControl leftRulerControl;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel1;

    }
}

