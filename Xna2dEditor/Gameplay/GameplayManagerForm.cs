#region File Description
//-----------------------------------------------------------------------------
// MainForm.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Fyri2dEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Imaging;
using Xna2dEditor;
#endregion

namespace Fyri2dEditor
{
    /// <summary>
    /// Custom form provides the main user interface for the program.
    /// In this sample we used the designer to fill the entire form with a
    /// Xna2dEditorControl, except for the menu bar which provides the
    /// "File / Open..." option.
    /// </summary>
    public partial class GameplayManagerForm : Form
    {
        FyriProject currentProject;

        /// <summary>
        /// Constructs the main form.
        /// </summary>
        public GameplayManagerForm(FyriProject project)
        {
            InitializeComponent();     
       
            // Don't initialize the graphics device if we are running in the designer.
            if (!DesignMode)
            {
                currentProject = project;
                if(!currentProject.IsInitialized)
                    currentProject.Initialize(this.Handle, this.Width, this.Height);
                RefreshProject();
            }

            /// Automatically bring up the "Load Model" dialog when we are first shown.
            //this.Shown += OpenMenuClicked;
        }


        /// <summary>
        /// Event handler for the Exit menu option.
        /// </summary>
        void ExitMenuClicked(object sender, EventArgs e)
        {
            Close();
        }


        #region File Methods

        #region Project

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentProject != null && currentProject.IsDirty)
            {
                OfferSaveAndClose();
            }

            EditProjectWindow epw = new EditProjectWindow(null);

            DialogResult result = epw.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                currentProject = epw.Project;
                currentProject.OriginalContentFolder = currentProject.ProjectContentFolder + "\\Original";
                if (!Directory.Exists(currentProject.OriginalContentFolder))
                    Directory.CreateDirectory(currentProject.OriginalContentFolder);
                RefreshProject();
            }
        }

        private void OpenDefaultProject()
        {
            Stream stream;
            string defaultProjectFilePath = "C:\\Users\\dovieya\\Desktop\\TestContentLoader\\testProject.ff";

            if (File.Exists(defaultProjectFilePath))
            {
                if ((stream = File.OpenRead(defaultProjectFilePath)) != null)
                {
                    BinaryFormatter bFormatter = new BinaryFormatter();
                    currentProject = (FyriProject)bFormatter.Deserialize(stream);
                    stream.Close();
                }
            }
        }

        public void RefreshProject()
        {
            if (currentProject != null)
            {
                this.projectContentTV.Nodes.Clear();
                TreeViewHelper.Copy(currentProject.ContentTreeView, this.projectContentTV);
                this.projectContentTV.Refresh();

                xnaGPScreenControl.RoundLineManager = currentProject.RoundLineManager;
                xnaGPScreenControl.LineBatch = currentProject.LineBatch;
                xnaGPScreenControl.DrawingContext = currentProject.DrawingContext;
                xnaGPScreenControl.TextureManager = currentProject.Texture2dManager;

                xnaGPScreenControl.Effect = currentProject.EffectManager.GetEffect("RoundLine").Effect;
                xnaGPScreenControl.SpriteFont = currentProject.FontManager.GetFont("SpriteFont").Font;
            }
        }

        private void OpenProject()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
            {
                Stream stream;

                if ((stream = ofd.OpenFile()) != null)
                {
                    BinaryFormatter bFormatter = new BinaryFormatter();
                    currentProject = (FyriProject)bFormatter.Deserialize(stream);
                    stream.Close();
                }
            }
        }
        #endregion        

        #endregion

        #region Other Methods

        private void OfferSaveAndClose()
        {
            DialogResult result = MessageBox.Show("Do you wish to save the current project?", "Save Current Project?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.OverwritePrompt = true;

                DialogResult sfdResult = sfd.ShowDialog();

                if (sfdResult == System.Windows.Forms.DialogResult.OK)
                {
                    Stream stream;

                    if ((stream = sfd.OpenFile()) != null)
                    {
                        BinaryFormatter bFormatter = new BinaryFormatter();
                        bFormatter.Serialize(stream, currentProject);
                        stream.Close();
                    }
                }
            }

            currentProject = null;
        }

        private void closeProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (modelViewerControl.Model != null)
            //    modelViewerControl.Model = null;

            if (currentProject != null && currentProject.IsDirty)
            {
                OfferSaveAndClose();
            }

            currentProject = null;

            RefreshProject();
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentProject != null && currentProject.IsDirty)
            {
                OfferSaveAndClose();
            }

            OpenProject();
            RefreshProject();
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream stream = File.Open(currentProject.ProjectFileName, FileMode.OpenOrCreate);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, currentProject);
            stream.Close();
        }

        private void projectContentTV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = projectContentTV.SelectedNode;
            if (selectedNode != null)
            {
                if (selectedNode.Tag != null)
                {
                    if (selectedNode.Tag is FyriModel)
                    {
                        //FyriModel model = selectedNode.Tag as FyriModel;
                        //modelViewerControl.Model = model.Model;
                        //tabControl1.SelectedTab = modelTabPage;
                    }
                }
            }
        }

        #endregion

        protected override void OnResize(EventArgs e)
        {
            toolStripStatusLabel1.Text = "Width: " + this.Width + " Height: " + this.Height; 
            base.OnResize(e);
        }

        private void loadModelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (currentProject != null)
                currentProject.LoadModelClicked(sender, e);
        }

        private void loadTexture2dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentProject != null)
                currentProject.LoadTexture2dClick(sender, e);
        }

        private void loadFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentProject != null)
                currentProject.LoadFontClick(sender, e);
        }

        private void loadEffectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentProject != null)
                currentProject.LoadEffectClick(sender, e);
        }

        private void loadSpriterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentProject != null)
                currentProject.LoadSpriterClick(sender, e);
        }

        
    }
}
