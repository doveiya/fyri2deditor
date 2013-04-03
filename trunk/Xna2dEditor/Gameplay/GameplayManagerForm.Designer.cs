namespace Fyri2dEditor
{
    partial class GameplayManagerForm
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
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Project Name");
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.screenTreeView = new System.Windows.Forms.TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.xnaGPScreenControl = new Fyri2dEditor.XnaGPScreenControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.projectContentTV = new System.Windows.Forms.TreeView();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.newGameTSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGameTSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openGameTSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeGameTSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadModelToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTexture2dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadEffectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSpriterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
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
            this.menuStrip1.Size = new System.Drawing.Size(1314, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameTSMenuItem,
            this.openGameTSMenuItem,
            this.saveGameTSMenuItem,
            this.closeGameTSMenuItem,
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
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.screenTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1314, 632);
            this.splitContainer1.SplitterDistance = 209;
            this.splitContainer1.TabIndex = 1;
            // 
            // screenTreeView
            // 
            this.screenTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.screenTreeView.Location = new System.Drawing.Point(0, 0);
            this.screenTreeView.Name = "screenTreeView";
            this.screenTreeView.Size = new System.Drawing.Size(209, 632);
            this.screenTreeView.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(1101, 632);
            this.splitContainer2.SplitterDistance = 814;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(814, 632);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.xnaGPScreenControl);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(806, 606);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // xnaGPScreenControl
            // 
            this.xnaGPScreenControl.ActiveTool = Xna2dEditor.XnaToolUser.Xna2dDrawToolType.Pointer;
            this.xnaGPScreenControl.Description = "Svg picture";
            this.xnaGPScreenControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xnaGPScreenControl.DrawGrid = false;
            this.xnaGPScreenControl.DrawingBatch = null;
            this.xnaGPScreenControl.DrawingContext = null;
            this.xnaGPScreenControl.DrawNetRectangle = false;
            this.xnaGPScreenControl.Effect = null;
            this.xnaGPScreenControl.GraphicsList = null;
            this.xnaGPScreenControl.LineBatch = null;
            this.xnaGPScreenControl.Location = new System.Drawing.Point(3, 3);
            this.xnaGPScreenControl.MyMouseX = 0;
            this.xnaGPScreenControl.MyMouseY = 0;
            this.xnaGPScreenControl.Name = "xnaGPScreenControl";
            this.xnaGPScreenControl.NetRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0);
            this.xnaGPScreenControl.OldScale = new Microsoft.Xna.Framework.Point(1, 1);
            this.xnaGPScreenControl.OriginalSize = new Microsoft.Xna.Framework.Point(800, 600);
            this.xnaGPScreenControl.Owner = null;
            this.xnaGPScreenControl.Project = null;
            this.xnaGPScreenControl.RoundLineManager = null;
            this.xnaGPScreenControl.ScaleDraw = new Microsoft.Xna.Framework.Point(1, 1);
            this.xnaGPScreenControl.ShapeProperties = null;
            this.xnaGPScreenControl.Size = new System.Drawing.Size(800, 600);
            this.xnaGPScreenControl.SizePicture = new Microsoft.Xna.Framework.Point(800, 600);
            this.xnaGPScreenControl.SpriteFont = null;
            this.xnaGPScreenControl.TabIndex = 0;
            this.xnaGPScreenControl.Text = "xnaGPScreenControl";
            this.xnaGPScreenControl.TextureManager = null;
            this.xnaGPScreenControl.ToolBox = null;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(806, 606);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.projectContentTV);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer3.Size = new System.Drawing.Size(283, 632);
            this.splitContainer3.SplitterDistance = 291;
            this.splitContainer3.TabIndex = 0;
            // 
            // projectContentTV
            // 
            this.projectContentTV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectContentTV.Location = new System.Drawing.Point(0, 0);
            this.projectContentTV.Name = "projectContentTV";
            treeNode2.Name = "ProjectNameNode";
            treeNode2.Text = "Project Name";
            this.projectContentTV.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.projectContentTV.Size = new System.Drawing.Size(283, 291);
            this.projectContentTV.TabIndex = 0;
            this.projectContentTV.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.projectContentTV_AfterSelect);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(283, 337);
            this.propertyGrid1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 634);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1314, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // newGameTSMenuItem
            // 
            this.newGameTSMenuItem.Name = "newGameTSMenuItem";
            this.newGameTSMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newGameTSMenuItem.Text = "New Game";
            // 
            // saveGameTSMenuItem
            // 
            this.saveGameTSMenuItem.Name = "saveGameTSMenuItem";
            this.saveGameTSMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveGameTSMenuItem.Text = "Save Game";
            // 
            // openGameTSMenuItem
            // 
            this.openGameTSMenuItem.Name = "openGameTSMenuItem";
            this.openGameTSMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openGameTSMenuItem.Text = "Open Game";
            // 
            // closeGameTSMenuItem
            // 
            this.closeGameTSMenuItem.Name = "closeGameTSMenuItem";
            this.closeGameTSMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeGameTSMenuItem.Text = "Close Game";
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
            // GameplayManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1314, 656);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "GameplayManagerForm";
            this.Text = "GamePlay Manager";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
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
        private System.Windows.Forms.TreeView projectContentTV;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TreeView screenTreeView;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private XnaGPScreenControl xnaGPScreenControl;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem newGameTSMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openGameTSMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveGameTSMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeGameTSMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadModelToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem loadTexture2dToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadEffectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSpriterToolStripMenuItem;

    }
}

