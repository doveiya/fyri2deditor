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
using Fyri2dEditor.Xna2dDrawingLibrary;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
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
    public partial class FyriMainForm : Form
    {
        

        FyriProject currentProject;

        ContentManagerForm contentManagerForm;
        GameplayManagerForm gameplayManagerForm;

        //Xna2dShapeControl xna2dShapeControl1;
        //private PropertyValueChangedEventHandler propertyGrid1_PropertyValueChanged;

        void propertyGrid1_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            //xna2dShapeControl1.PropertyChanged(e.ChangedItem, e.OldValue);
            //xna2dShapeControl1.Refresh();
        }

        /// <summary>
        /// Constructs the main form.
        /// </summary>
        public FyriMainForm()
        {
            InitializeComponent();     
       
            // Don't initialize the graphics device if we are running in the designer.
            if (!DesignMode)
            {
                

                //xna2dShapeControl1 = new Xna2dShapeControl();
                //xna2dShapeControl1.Size = new System.Drawing.Size(800, 600);
                //xna2dShapeControl1.Dock = DockStyle.Fill;
                //panel1.Controls.Add(xna2dShapeControl1);

                //Mouse.WindowHandle = xna2dShapeControl1.Handle;

                //ProjectNameNode = projectContentTV.Nodes["ProjectNameNode"];

                OpenDefaultProject();
                RefreshProject();
            }

            /// Automatically bring up the "Load Model" dialog when we are first shown.
            //this.Shown += OpenMenuClicked;
        }

        protected override void OnResize(EventArgs e)
        {
            //toolStripStatusLabel1.Text = "X: " + this.Size.Width + " Y: " + this.Size.Height;
            base.OnResize(e);
        }


        /// <summary>
        /// Event handler for the Exit menu option.
        /// </summary>
        void ExitMenuClicked(object sender, EventArgs e)
        {
            Close();
        }

        void ItemsSelected(object sender, MouseEventArgs e)
        {
            int i = 0;
            var selectedItems = (List<Draw.XnaDrawObject>)sender;
            var obj = new object[selectedItems.Count];
            foreach (Draw.XnaDrawObject dob in selectedItems)
            {
                obj[i++] = dob;
            }

            if (selectedItems.Count > 0)
            {
                //propertyGrid1.SelectedObjects = obj;
            }
        }

        public void SetTool(object sender, EventArgs e)
        {
            //if (xna2dShapeControl1 == null)
            //    return;

            //switch ((String)sender)
            //{
            //    case "Pointer":
            //        xna2dShapeControl1.ActiveTool = Xna2dEditor.XnaToolUser.Xna2dDrawToolType.Pointer;
            //        break;

            //    case "Rectangle":
            //        xna2dShapeControl1.ActiveTool = Xna2dEditor.XnaToolUser.Xna2dDrawToolType.Rectangle;
            //        break;

            //    case "Ellipse":
            //        xna2dShapeControl1.ActiveTool = Xna2dEditor.XnaToolUser.Xna2dDrawToolType.Ellipse;
            //        break;

            //    case "Line":
            //        xna2dShapeControl1.ActiveTool = Xna2dEditor.XnaToolUser.Xna2dDrawToolType.Line;
            //        break;

            //    case "Pan":
            //        xna2dShapeControl1.ActiveTool = Xna2dEditor.XnaToolUser.Xna2dDrawToolType.Pan;
            //        break;

            //    case "Pencil":
            //        xna2dShapeControl1.ActiveTool = Xna2dEditor.XnaToolUser.Xna2dDrawToolType.Polygon;
            //        break;

            //    case "Text":
            //        xna2dShapeControl1.ActiveTool = Xna2dEditor.XnaToolUser.Xna2dDrawToolType.Text;
            //        break;

            //    case "Path":
            //        xna2dShapeControl1.ActiveTool = Xna2dEditor.XnaToolUser.Xna2dDrawToolType.Path;
            //        break;

            //    case "Image":
            //        xna2dShapeControl1.ActiveTool = Xna2dEditor.XnaToolUser.Xna2dDrawToolType.Bitmap;
            //        break;

            //    default:
            //        xna2dShapeControl1.ActiveTool = Xna2dEditor.XnaToolUser.Xna2dDrawToolType.Pointer;
            //        break;
            //}
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

        public void RefreshProject()
        {
            if (currentProject != null)
            {
                this.projectContentTV.Nodes.Clear();
                TreeViewHelper.Copy(currentProject.ContentTreeView, this.projectContentTV);
                this.projectContentTV.Refresh();
            }
        }

        //public static T DeepTreeCopy<T>(T obj)
        //{
        //    object result = null;
        //    using (var ms = new MemoryStream())
        //    {
        //        var formatter = new BinaryFormatter();
        //        formatter.Serialize(ms, obj);
        //        ms.Position = 0;
        //        result = (T)formatter.Deserialize(ms); ms.Close();
        //    }
        //    return (T)result;
        //} 

        

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

            if(!currentProject.IsInitialized)
                currentProject.Initialize(this.Handle, this.Width, this.Height);
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

        private void toggleGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //xna2dShapeControl1.DrawGrid = toggleGridToolStripMenuItem.Checked;
        }

        private void assetsButton_Click(object sender, EventArgs e)
        {
            if (contentManagerForm == null)
            {
                contentManagerForm = new ContentManagerForm(currentProject);
                contentManagerForm.Show(this);
            }
        }

        

        private void gamePlayButton_Click(object sender, EventArgs e)
        {
            if (gameplayManagerForm == null)
            {
                gameplayManagerForm = new GameplayManagerForm(currentProject);
                gameplayManagerForm.Show(this);
            }
        }

        //private void projectContentTV_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    TreeNode selectedNode = projectContentTV.SelectedNode;
        //    if (selectedNode != null)
        //    {
        //        if (selectedNode.Tag != null)
        //        {
        //            if (selectedNode.Tag is FyriModel)
        //            {
        //                FyriModel model = selectedNode.Tag as FyriModel;
        //                //modelViewerControl.Model = model.Model;
        //            }
        //        }
        //    }
        //}

        #endregion
    }
}
