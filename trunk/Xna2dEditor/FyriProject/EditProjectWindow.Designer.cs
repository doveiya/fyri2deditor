namespace Fyri2dEditor
{
    partial class EditProjectWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.ProjNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SaveFileTextBox = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CanclButton = new System.Windows.Forms.Button();
            this.ContentBrowse = new System.Windows.Forms.Button();
            this.ContentFolderTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Project Name:";
            // 
            // ProjNameTextBox
            // 
            this.ProjNameTextBox.Location = new System.Drawing.Point(92, 22);
            this.ProjNameTextBox.Name = "ProjNameTextBox";
            this.ProjNameTextBox.Size = new System.Drawing.Size(180, 20);
            this.ProjNameTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Save File:";
            // 
            // SaveFileTextBox
            // 
            this.SaveFileTextBox.Location = new System.Drawing.Point(92, 48);
            this.SaveFileTextBox.Name = "SaveFileTextBox";
            this.SaveFileTextBox.ReadOnly = true;
            this.SaveFileTextBox.Size = new System.Drawing.Size(180, 20);
            this.SaveFileTextBox.TabIndex = 3;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(215, 74);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(57, 23);
            this.BrowseButton.TabIndex = 4;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(152, 175);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(57, 23);
            this.SaveButton.TabIndex = 5;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CanclButton
            // 
            this.CanclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CanclButton.Location = new System.Drawing.Point(215, 175);
            this.CanclButton.Name = "CanclButton";
            this.CanclButton.Size = new System.Drawing.Size(57, 23);
            this.CanclButton.TabIndex = 6;
            this.CanclButton.Text = "Cancel";
            this.CanclButton.UseVisualStyleBackColor = true;
            this.CanclButton.Click += new System.EventHandler(this.CanclButton_Click);
            // 
            // ContentBrowse
            // 
            this.ContentBrowse.Location = new System.Drawing.Point(215, 129);
            this.ContentBrowse.Name = "ContentBrowse";
            this.ContentBrowse.Size = new System.Drawing.Size(57, 23);
            this.ContentBrowse.TabIndex = 9;
            this.ContentBrowse.Text = "Browse";
            this.ContentBrowse.UseVisualStyleBackColor = true;
            this.ContentBrowse.Click += new System.EventHandler(this.ContentBrowse_Click);
            // 
            // ContentFolderTB
            // 
            this.ContentFolderTB.Location = new System.Drawing.Point(92, 103);
            this.ContentFolderTB.Name = "ContentFolderTB";
            this.ContentFolderTB.ReadOnly = true;
            this.ContentFolderTB.Size = new System.Drawing.Size(180, 20);
            this.ContentFolderTB.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Content Folder:";
            // 
            // EditProjectWindow
            // 
            this.AcceptButton = this.SaveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CanclButton;
            this.ClientSize = new System.Drawing.Size(284, 210);
            this.Controls.Add(this.ContentBrowse);
            this.Controls.Add(this.ContentFolderTB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CanclButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.SaveFileTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ProjNameTextBox);
            this.Controls.Add(this.label1);
            this.Name = "EditProjectWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EditProjectWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ProjNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SaveFileTextBox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button CanclButton;
        private System.Windows.Forms.Button ContentBrowse;
        private System.Windows.Forms.TextBox ContentFolderTB;
        private System.Windows.Forms.Label label3;
    }
}