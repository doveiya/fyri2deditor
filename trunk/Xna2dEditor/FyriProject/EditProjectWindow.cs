using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Fyri2dEditor
{
    public partial class EditProjectWindow : Form
    {
        FyriProject projectToEdit = new FyriProject();

        public FyriProject Project
        {
            get
            {
                return projectToEdit;
            }
        }

        public EditProjectWindow(FyriProject project)
        {
            InitializeComponent();

            if (project != null)
                projectToEdit = project;

            ProjNameTextBox.Text = projectToEdit.ProjectName;
            SaveFileTextBox.Text = projectToEdit.ProjectFileName;
            ContentFolderTB.Text = projectToEdit.ProjectContentFolder;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string error = string.Empty;

            if (ProjNameTextBox.Text == string.Empty)
                error += "Project Name Cannot Be Empty!" + Environment.NewLine;

            if (SaveFileTextBox.Text == string.Empty)
                error += "Save File Name Cannot Be Empty!" + Environment.NewLine;

            if (ContentFolderTB.Text == string.Empty)
                error += "Content Folder Name Cannot Be Empty!" + Environment.NewLine;

            if (error != string.Empty)
            {
                MessageBox.Show(error, "Please Correct Errors!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Project.ProjectName = ProjNameTextBox.Text;
            Project.ProjectFileName = SaveFileTextBox.Text;
            Project.ProjectContentFolder = ContentFolderTB.Text;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            this.Close();
        }

        private void CanclButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.Close();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.AddExtension = true;
            sfd.Filter = "Fyri Files (*.ff)|*.ff";

            DialogResult result = sfd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                SaveFileTextBox.Text = sfd.FileName;
            }
        }

        private void ContentBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog sfd = new FolderBrowserDialog();

            DialogResult result = sfd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ContentFolderTB.Text = sfd.SelectedPath;
            }
        }
    }
}
