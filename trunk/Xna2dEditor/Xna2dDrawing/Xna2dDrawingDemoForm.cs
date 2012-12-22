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
#endregion

namespace Fyri2dEditor
{
    /// <summary>
    /// Custom form provides the main user interface for the program.
    /// In this sample we used the designer to fill the entire form with a
    /// Xna2dEditorControl, except for the menu bar which provides the
    /// "File / Open..." option.
    /// </summary>
    public partial class Xna2dDrawingDemoForm : Form
    {
        // However many GraphicsDeviceControl instances you have, they all share
        // the same underlying GraphicsDevice, managed by this helper service.
        GraphicsDeviceService graphicsDeviceService;

        /// <summary>
        /// Gets an IServiceProvider containing our IGraphicsDeviceService.
        /// This can be used with components such as the ContentManager,
        /// which use this service to look up the GraphicsDevice.
        /// </summary>
        public ServiceContainer Services
        {
            get { return services; }
        }

        ServiceContainer services = new ServiceContainer();

        XnaModelManager modelManager;
        XnaTexture2dManager texture2dManager;
        XnaFontManager fontManager;
        XnaEffectManager effectManager;

        XnaLine2dBatch lineBatch;
        XnaDrawingBatch drawingBatch;
        XnaDrawingContext drawingContext;

        ContentBuilder contentBuilder;
        ContentManager contentManager;

        FyriProject currentProject;

        TreeNode ProjectNameNode;
        TreeNode ModelNode;
        TreeNode Texture2dNode;
        TreeNode FontNode;
        TreeNode EffectNode;

        /// <summary>
        /// Constructs the main form.
        /// </summary>
        public Xna2dDrawingDemoForm()
        {
            InitializeComponent();     
       
            // Don't initialize the graphics device if we are running in the designer.
            if (!DesignMode)
            {
                graphicsDeviceService = GraphicsDeviceService.AddRef(Handle,
                                                                     ClientSize.Width,
                                                                     ClientSize.Height);

                // Register the service, so components like ContentManager can find it.
                services.AddService<IGraphicsDeviceService>(graphicsDeviceService);

                //OpenDefaultProject();
                //RefreshProject();
            }

            //ProjectNameNode = projectContentTV.Nodes["ProjectNameNode"];
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

        public void RefreshProject()
        {
            //ProjectNameNode.Text = "Project Name";

            //ProjectNameNode.Nodes.Clear();

            xna2dDrawingDemo3VC.DrawingBatch = null;
            xna2dDrawingDemo3VC.DrawingContext = null;
            xna2dDrawingDemo3VC.LineBatch = null;
            xna2dDrawingDemo3VC.SpriteFont = null;
            xna2dDrawingDemo3VC.Effect = null;
            xna2dDrawingDemo3VC.Texture = null;

            if (drawingContext != null)
            {
                drawingContext = null;
            }

            if (drawingBatch != null)
            {
                drawingBatch = null;
            }

            if (lineBatch != null)
            {
                lineBatch = null;
            }

            if (effectManager != null)
            {
                effectManager = null;
            }

            if (fontManager != null)
            {
                fontManager = null;
            }

            if (texture2dManager != null)
            {
                texture2dManager = null;
            }

            if (modelManager != null)
            {
                modelManager = null;
            }

            if (contentManager != null)
            {
                contentManager.Unload();
                contentManager.Dispose();
                contentManager = null;
            }

            if (contentBuilder != null)
            {
                contentBuilder.Clear();
                contentBuilder.Dispose();
                contentBuilder = null;
            }

            if (currentProject != null)
            {
                //ProjectNameNode.Text = currentProject.ProjectName;

                contentBuilder = new ContentBuilder(currentProject.ProjectContentFolder, false);

                contentManager = new ContentManager(this.Services,
                                                    contentBuilder.OutputDirectory);

                modelManager = new XnaModelManager(contentBuilder, contentManager, currentProject.OriginalContentFolder);

                texture2dManager = new XnaTexture2dManager(contentBuilder, contentManager, currentProject.OriginalContentFolder);

                fontManager = new XnaFontManager(contentBuilder, contentManager, currentProject.OriginalContentFolder);

                effectManager = new XnaEffectManager(contentBuilder, contentManager, currentProject.OriginalContentFolder);

                
                //roundLineTechniqueNames = roundLineManager.TechniqueNames;

                //ProjectNameNode.Nodes.Clear();
                ModelNode = null;
                Texture2dNode = null;
                FontNode = null;
                EffectNode = null;

                //effectViewerControl.Effect = null;

                if (currentProject.LoadedModels.Count > 0)
                {
                    currentProject.LoadedModels = modelManager.RefreshList(currentProject.LoadedModels);

                    //foreach (FyriModel model in currentProject.LoadedModels)
                    //{
                    //    AddModelToTreeView(model);
                    //}
                }
                else
                {
                    //Load default Sphere
                    string defSphereFileName = Directory.GetCurrentDirectory() + "\\Default\\Sphere.fbx";
                    if(File.Exists(defSphereFileName))
                        LoadModelToProject(defSphereFileName);
                }

                if (currentProject.LoadedTexture2ds.Count > 0)
                {
                    currentProject.LoadedTexture2ds = texture2dManager.RefreshList(currentProject.LoadedTexture2ds);

                    //foreach (FyriTexture2d texture in currentProject.LoadedTexture2ds)
                    //{
                    //    AddTexture2dToTreeView(texture);
                    //}
                }
                else
                {
                    //Load default Sphere
                    string defTulipsPngFileName = Directory.GetCurrentDirectory() + "\\Default\\Tulips.png";
                    if (File.Exists(defTulipsPngFileName))
                        LoadTexture2dToProject(defTulipsPngFileName);
                }

                if (currentProject.LoadedFonts.Count > 0)
                {
                    currentProject.LoadedFonts = fontManager.RefreshList(currentProject.LoadedFonts);

                    //foreach (FyriFont font in currentProject.LoadedFonts)
                    //{
                    //    AddFontToTreeView(font);
                    //}
                }
                else
                {
                    //Load default Sphere
                    string defSpriteFontFileName = Directory.GetCurrentDirectory() + "\\Default\\SpriteFont.spritefont";
                    if (File.Exists(defSpriteFontFileName))
                        LoadFontToProject(defSpriteFontFileName);

                    string defSegoeFontFileName = Directory.GetCurrentDirectory() + "\\Default\\SegoeUI.spritefont";
                    if (File.Exists(defSegoeFontFileName))
                        LoadFontToProject(defSegoeFontFileName);
                }

                if (currentProject.LoadedEffects.Count > 0)
                {
                    currentProject.LoadedEffects = effectManager.RefreshList(currentProject.LoadedEffects);

                    //foreach (FyriEffect effect in currentProject.LoadedEffects)
                    //{
                    //    AddEffectToTreeView(effect);
                    //}
                }
                else
                {
                    //Load default Sphere
                    string defRLEffectFileName = Directory.GetCurrentDirectory() + "\\Default\\RoundLine.fx";
                    if (File.Exists(defRLEffectFileName))
                        LoadEffectToProject(defRLEffectFileName);
                }

                FyriEffect roundlineEffect = effectManager.GetEffect("RoundLine");
                FyriFont roundlineFont = fontManager.GetFont("SpriteFont");
                FyriFont segoeUIFont = fontManager.GetFont("SegoeUI");
                FyriTexture2d tulipsPng = texture2dManager.GetTexture2d("Tulips");

                lineBatch = new XnaLine2dBatch();
                lineBatch.Init(this.graphicsDeviceService.GraphicsDevice, roundlineEffect.Effect);

                drawingBatch = new XnaDrawingBatch(this.graphicsDeviceService.GraphicsDevice);
                drawingContext = new XnaDrawingContext(this.graphicsDeviceService.GraphicsDevice);

                xna2dDrawingDemo3VC.SpriteFont = segoeUIFont.Font;
                xna2dDrawingDemo3VC.Effect = roundlineEffect.Effect;
                xna2dDrawingDemo3VC.Texture = tulipsPng.Texture;
                xna2dDrawingDemo3VC.DrawingBatch = drawingBatch;
                xna2dDrawingDemo3VC.DrawingContext = drawingContext;
                xna2dDrawingDemo3VC.LineBatch = lineBatch;
                
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
        #endregion

        #region Models

        /// <summary>
        /// Event handler for the Load Model menu option.
        /// </summary>
        void LoadModelClicked(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            // Default to the directory which contains our content files.
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string relativePath = Path.Combine(assemblyLocation, "../../../../Content");
            string contentPath = Path.GetFullPath(relativePath);

            fileDialog.InitialDirectory = contentPath;

            fileDialog.Title = "Load Model";

            fileDialog.Filter = "Model Files (*.fbx;*.x)|*.fbx;*.x|" +
                                "FBX Files (*.fbx)|*.fbx|" +
                                "X Files (*.x)|*.x|" +
                                "All Files (*.*)|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadModelToProject(fileDialog.FileName);
            }
        }

        /// <summary>
        /// Loads a new 3D model file into the ModelViewerControl.
        /// </summary>
        FyriModel LoadModelToProject(string fileName)
        {
            if (currentProject != null)
            {
                Cursor = Cursors.WaitCursor;

                FyriModel loadedModel = modelManager.LoadModel(fileName);
                
                if (loadedModel != null)
                {
                    if(!currentProject.LoadedModels.Exists(p => p.OriginalFileName == fileName))
                        currentProject.LoadedModels.Add(loadedModel);

                    //modelViewerControl.Model = loadedModel.Model;
                    //AddModelToTreeView(loadedModel);
                }
                Cursor = Cursors.Arrow;

                return loadedModel;
            }

            return null;
        }

        void AddModelToTreeView(FyriModel modelToAdd)
        {
            if (ModelNode == null)
            {
                ModelNode = new TreeNode();
                ModelNode.Text = "Models";
                //ProjectNameNode.Nodes.Add(ModelNode);
            }

            TreeNode nodeToAdd = new TreeNode();
            nodeToAdd.Text = modelToAdd.ModelName;
            nodeToAdd.Tag = modelToAdd;

            ModelNode.Nodes.Add(nodeToAdd);

            //projectContentTV.CollapseAll();
            nodeToAdd.EnsureVisible();
            //projectContentTV.Refresh();
        }

        #endregion

        #region Textures

        private void loadTexture2dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            // Default to the directory which contains our content files.
            //string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            //string relativePath = Path.Combine(assemblyLocation, "../../../../Content");
            string contentPath = currentProject.OriginalContentFolder;

            fileDialog.InitialDirectory = contentPath;

            fileDialog.Title = "Load Texture2d";

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            string sep = string.Empty;

            foreach (var c in codecs)
            {
                string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                fileDialog.Filter = String.Format("{0}{1}{2} ({3})|{3}", fileDialog.Filter, sep, codecName, c.FilenameExtension);
                sep = "|";
            }

            fileDialog.Filter = String.Format("{0}{1}{2} ({3})|{3}", fileDialog.Filter, sep, "All Files", "*.*");

            fileDialog.DefaultExt = ".png"; // Default file extension 

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadTexture2dToProject(fileDialog.FileName);
            }
        }

        /// <summary>
        /// Loads a new 3D model file into the ModelViewerControl.
        /// </summary>
        FyriTexture2d LoadTexture2dToProject(string fileName)
        {
            if (currentProject != null)
            {
                Cursor = Cursors.WaitCursor;

                FyriTexture2d loadedTexture = texture2dManager.LoadTexture2d(fileName);

                if (loadedTexture != null)
                {
                    if (!currentProject.LoadedTexture2ds.Exists(p => p.OriginalFileName == fileName))
                        currentProject.LoadedTexture2ds.Add(loadedTexture);

                    //texture2dViewerControl.Texture = loadedTexture.Texture;
                    //AddTexture2dToTreeView(loadedTexture);
                }
                Cursor = Cursors.Arrow;

                return loadedTexture;
            }

            return null;
        }

        void AddTexture2dToTreeView(FyriTexture2d textureToAdd)
        {
            if (Texture2dNode == null)
            {
                Texture2dNode = new TreeNode();
                Texture2dNode.Text = "Textures";
                //ProjectNameNode.Nodes.Add(Texture2dNode);
            }

            TreeNode nodeToAdd = new TreeNode();
            nodeToAdd.Text = textureToAdd.TextureName;
            nodeToAdd.Tag = textureToAdd;

            Texture2dNode.Nodes.Add(nodeToAdd);

            //projectContentTV.CollapseAll();
            nodeToAdd.EnsureVisible();
            //projectContentTV.Refresh();
        }

        #endregion

        #region Fonts

        private void loadFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BitmapFontExporter fileDialog = new BitmapFontExporter(currentProject);

            // Default to the directory which contains our content files.
            //string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            //string relativePath = Path.Combine(assemblyLocation, "../../../../Content");
            string contentPath = currentProject.OriginalContentFolder;

            if (fileDialog.ShowDialog() == DialogResult.OK && fileDialog.FontImageFileName != null)
            {
                LoadFontToProject(fileDialog.FontImageFileName);
            }
        }

        private void loadSpriteFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            // Default to the directory which contains our content files.
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string relativePath = Path.Combine(assemblyLocation, "../../../../Content");
            string contentPath = Path.GetFullPath(relativePath);

            fileDialog.InitialDirectory = contentPath;

            fileDialog.Title = "Load SpriteFont";

            fileDialog.Filter = "SpriteFont Files (*.spritefont)|*.spritefont)";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadFontToProject(fileDialog.FileName);
            }
        }

        /// <summary>
        /// Loads a new 3D model file into the ModelViewerControl.
        /// </summary>
        FyriFont LoadFontToProject(string fontName)
        {
            if (currentProject != null)
            {
                Cursor = Cursors.WaitCursor;

                FyriFont loadedFont = fontManager.LoadFont(fontName);

                if (loadedFont != null)
                {
                    if (!currentProject.LoadedFonts.Exists(p => p.FontName == fontName))
                        currentProject.LoadedFonts.Add(loadedFont);

                    //fontViewerControl.SpriteFont = loadedFont.Font;
                    //AddFontToTreeView(loadedFont);
                }
                Cursor = Cursors.Arrow;

                return loadedFont;
            }

            return null;
        }

        void AddFontToTreeView(FyriFont fontToAdd)
        {
            if (FontNode == null)
            {
                FontNode = new TreeNode();
                FontNode.Text = "Fonts";
                //ProjectNameNode.Nodes.Add(FontNode);
            }

            TreeNode nodeToAdd = new TreeNode();
            nodeToAdd.Text = fontToAdd.FontName;
            nodeToAdd.Tag = fontToAdd;

            FontNode.Nodes.Add(nodeToAdd);

            //projectContentTV.CollapseAll();
            nodeToAdd.EnsureVisible();
            //projectContentTV.Refresh();
        }

        #endregion

        #region Effects

        private void loadEffectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            // Default to the directory which contains our content files.
            //string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            //string relativePath = Path.Combine(assemblyLocation, "../../../../Content");
            string contentPath = currentProject.OriginalContentFolder;

            fileDialog.InitialDirectory = contentPath;

            fileDialog.Title = "Load Effect";

            fileDialog.Filter = "Effect Files (*.fx)|*.fx|" +
                                "All Files (*.*)|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadEffectToProject(fileDialog.FileName);
            }
        }

        /// <summary>
        /// Loads a new 3D model file into the ModelViewerControl.
        /// </summary>
        FyriEffect LoadEffectToProject(string effectName)
        {
            if (currentProject != null)
            {
                Cursor = Cursors.WaitCursor;

                FyriEffect loadedEffect = effectManager.LoadEffect(effectName);

                if (loadedEffect != null)
                {
                    if (!currentProject.LoadedFonts.Exists(p => p.FontName == effectName))
                        currentProject.LoadedEffects.Add(loadedEffect);

                    //effectViewerControl.Effect = loadedEffect.Effect;
                    //AddEffectToTreeView(loadedEffect);
                }
                Cursor = Cursors.Arrow;

                return loadedEffect;
            }

            return null;
        }

        void AddEffectToTreeView(FyriEffect effectToAdd)
        {
            if (EffectNode == null)
            {
                EffectNode = new TreeNode();
                EffectNode.Text = "Effects";
                //ProjectNameNode.Nodes.Add(EffectNode);
            }

            TreeNode nodeToAdd = new TreeNode();
            nodeToAdd.Text = effectToAdd.EffectName;
            nodeToAdd.Tag = effectToAdd;

            EffectNode.Nodes.Add(nodeToAdd);

            //projectContentTV.CollapseAll();
            nodeToAdd.EnsureVisible();
            //projectContentTV.Refresh();
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

        private void xna2dDrawingDemo3VC_ContextChanged(object sender, ContextEventArgs e)
        {
            if (drawingContext != null)
                xna2dDrawingDemo3VC.LoadContent();
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
