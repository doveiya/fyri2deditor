using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Xna2dEditor;
using Microsoft.Xna.Framework.Content;
using Fyri2dEditor.Xna2dDrawingLibrary;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Reflection;

namespace Fyri2dEditor
{
    [Serializable()]
    public class FyriProject : ISerializable
    {
        public int ProjectID;
        public string ProjectName;
        public string ProjectFileName;
        public string ProjectContentFolder;
        public string OriginalContentFolder;

        public bool IsDirty { get; set; }
        public bool IsInitialized { get; set; }

        public List<FyriModel> LoadedModels = new List<FyriModel>();
        public List<FyriTexture2d> LoadedTexture2ds = new List<FyriTexture2d>();
        public List<FyriFont> LoadedFonts = new List<FyriFont>();
        public List<FyriEffect> LoadedEffects = new List<FyriEffect>();
        public List<FyriSpriter> LoadedSpriters = new List<FyriSpriter>();

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
        public XnaModelManager ModelManager
        {
            get { return modelManager; }
        }

        XnaTexture2dManager texture2dManager;
        public XnaTexture2dManager Texture2dManager
        {
            get { return texture2dManager; }
        }

        XnaFontManager fontManager;
        public XnaFontManager FontManager
        {
            get { return fontManager; }
        }

        XnaEffectManager effectManager;
        public XnaEffectManager EffectManager
        {
            get { return effectManager; }
        }

        RoundLineManager roundLineManager;
        public RoundLineManager RoundLineManager
        {
            get { return roundLineManager; }
        }

        XnaLine2dBatch lineBatch;
        public XnaLine2dBatch LineBatch
        {
            get { return lineBatch; }
        }

        XnaDrawingContext drawingContext;
        public XnaDrawingContext DrawingContext
        {
            get { return drawingContext; }
        }

        XnaSpriterManager spriterManager;
        public XnaSpriterManager SpriterManager
        {
            get { return spriterManager; }
        }

        ContentBuilder contentBuilder;
        public ContentBuilder ContentBuilder
        {
            get { return contentBuilder; }
        }

        ContentManager contentManager;
        public ContentManager ContentManager
        {
            get { return contentManager; }
        }

        TreeView contentTreeView;
        public TreeView ContentTreeView
        {
            get { return contentTreeView; }
            //set { contentTreeView = value; }
        }

        TreeNode ProjectNameNode;
        TreeNode AssetsNode;
        TreeNode ModelNode;
        TreeNode Texture2dNode;
        TreeNode FontNode;
        TreeNode EffectNode;
        TreeNode SpriterNode;

        public FyriProject()
        {
            ProjectID = 0;
            ProjectName = null;
            ProjectFileName = null;
            ProjectContentFolder = null;
            OriginalContentFolder = null;

            IsDirty = false;
            IsInitialized = false;
        }

        //Deserialization constructor.
        public FyriProject(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            ProjectID = (int)info.GetValue("ProjectID", typeof(int));
            ProjectName = (String)info.GetValue("ProjectName", typeof(string));
            ProjectFileName = (String)info.GetValue("ProjectFileName", typeof(string));
            ProjectContentFolder = (String)info.GetValue("ProjectContentFolder", typeof(string));

            OriginalContentFolder = ProjectContentFolder + "\\Original";
            if (!Directory.Exists(OriginalContentFolder))
                Directory.CreateDirectory(OriginalContentFolder);

            LoadedModels = (List<FyriModel>)info.GetValue("ModelList", typeof(List<FyriModel>));
            LoadedTexture2ds = (List<FyriTexture2d>)info.GetValue("Texture2dList", typeof(List<FyriTexture2d>));
            LoadedFonts = (List<FyriFont>)info.GetValue("FontList", typeof(List<FyriFont>));
            LoadedEffects = (List<FyriEffect>)info.GetValue("EffectList", typeof(List<FyriEffect>));


            IsDirty = false;
            IsInitialized = false;

            ProjectNameNode = new TreeNode();
            ProjectNameNode.Text = ProjectName;

            contentTreeView = new TreeView();
            contentTreeView.Nodes.Add(ProjectNameNode);
        }
        
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //You can use any custom name for your name-value pair. But make sure you
            // read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
            // then you should read the same with "EmployeeId"
            info.AddValue("ProjectID", ProjectID);
            info.AddValue("ProjectName", ProjectName);
            info.AddValue("ProjectFileName", ProjectFileName);
            info.AddValue("ProjectContentFolder", ProjectContentFolder);
            info.AddValue("ModelList", LoadedModels);
            info.AddValue("Texture2dList", LoadedTexture2ds);
            info.AddValue("FontList", LoadedFonts);
            info.AddValue("EffectList", LoadedEffects);
        }


        public void Initialize(IntPtr handle, int width, int height)
        {
            graphicsDeviceService = GraphicsDeviceService.AddRef(handle,
                                                                     width,
                                                                     height);

            // Register the service, so components like ContentManager can find it.
            services.AddService<IGraphicsDeviceService>(graphicsDeviceService);

            contentBuilder = new ContentBuilder(ProjectContentFolder, false);

            contentManager = new ContentManager(this.Services,
                                                contentBuilder.OutputDirectory);

            modelManager = new XnaModelManager(contentBuilder, contentManager, OriginalContentFolder);

            texture2dManager = new XnaTexture2dManager(contentBuilder, contentManager, OriginalContentFolder);

            fontManager = new XnaFontManager(contentBuilder, contentManager, OriginalContentFolder);

            effectManager = new XnaEffectManager(contentBuilder, contentManager, OriginalContentFolder);

            spriterManager = new XnaSpriterManager(contentBuilder, contentManager, OriginalContentFolder);

            RefreshLists();

            FyriEffect roundlineEffect = effectManager.GetEffect("RoundLine");
            FyriFont roundlineFont = fontManager.GetFont("SpriteFont");

            roundLineManager = new RoundLineManager();
            roundLineManager.Init(this.graphicsDeviceService.GraphicsDevice, roundlineEffect.Effect);

            lineBatch = new XnaLine2dBatch();
            lineBatch.Init(this.graphicsDeviceService.GraphicsDevice, roundlineEffect.Effect);

            drawingContext = new XnaDrawingContext(this.graphicsDeviceService.GraphicsDevice);

            IsInitialized = true;
        }

        public void RefreshLists()
        {
            ProjectNameNode.Text = this.ProjectName;

            ProjectNameNode.Nodes.Clear();
            contentTreeView.Nodes.Clear();
            contentTreeView.Nodes.Add(ProjectNameNode);

            ModelNode = null;
            Texture2dNode = null;
            FontNode = null;
            EffectNode = null;

            if (LoadedModels.Count > 0)
            {
                LoadedModels = modelManager.RefreshList(LoadedModels);

                foreach (FyriModel model in LoadedModels)
                {
                    AddModelToTreeView(model);
                }
            }
            else
            {
                //Load default Sphere
                string defSphereFileName = Directory.GetCurrentDirectory() + "\\Default\\Sphere.fbx";
                if (File.Exists(defSphereFileName))
                    LoadModelToProject(defSphereFileName);
            }

            if (LoadedTexture2ds.Count > 0)
            {
                LoadedTexture2ds = texture2dManager.RefreshList(LoadedTexture2ds);

                foreach (FyriTexture2d texture in LoadedTexture2ds)
                {
                    AddTexture2dToTreeView(texture);
                }
            }

            if (LoadedFonts.Count > 0)
            {
                LoadedFonts = fontManager.RefreshList(LoadedFonts);

                foreach (FyriFont font in LoadedFonts)
                {
                    AddFontToTreeView(font);
                }
            }
            else
            {
                //Load default Sphere
                string defSpriteFontFileName = Directory.GetCurrentDirectory() + "\\Default\\SpriteFont.spritefont";
                if (File.Exists(defSpriteFontFileName))
                    LoadFontToProject(defSpriteFontFileName);
            }

            if (LoadedEffects.Count > 0)
            {
                LoadedEffects = effectManager.RefreshList(LoadedEffects);

                foreach (FyriEffect effect in LoadedEffects)
                {
                    AddEffectToTreeView(effect);
                }
            }
            else
            {
                //Load default Sphere
                string defRLEffectFileName = Directory.GetCurrentDirectory() + "\\Default\\RoundLine.fx";
                if (File.Exists(defRLEffectFileName))
                    LoadEffectToProject(defRLEffectFileName);
            }
        }

        #region Models

        /// <summary>
        /// Event handler for the Load Model menu option.
        /// </summary>
        public void LoadModelClicked(object sender, EventArgs e)
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
            //Cursor = Cursors.WaitCursor;

            FyriModel loadedModel = modelManager.LoadModel(fileName);

            if (loadedModel != null)
            {
                if (!LoadedModels.Exists(p => p.OriginalFileName == fileName))
                    LoadedModels.Add(loadedModel);

                //modelViewerControl.Model = loadedModel.Model;
                AddModelToTreeView(loadedModel);
            }
            //Cursor = Cursors.Arrow;

            return loadedModel;
        }

        void AddModelToTreeView(FyriModel modelToAdd)
        {
            if (ModelNode == null)
            {
                if (AssetsNode == null)
                {
                    AssetsNode = new TreeNode();
                    AssetsNode.Text = "Assets";
                    ProjectNameNode.Nodes.Add(AssetsNode);
                }
                ModelNode = new TreeNode();
                ModelNode.Text = "Models";
                AssetsNode.Nodes.Add(ModelNode);
            }

            TreeNode nodeToAdd = new TreeNode();
            nodeToAdd.Text = modelToAdd.ModelName;
            nodeToAdd.Tag = modelToAdd;

            ModelNode.Nodes.Add(nodeToAdd);

            contentTreeView.CollapseAll();
            nodeToAdd.EnsureVisible();
            contentTreeView.Refresh();
        }

        #endregion

        #region Textures

        public void LoadTexture2dClick(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            // Default to the directory which contains our content files.
            //string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            //string relativePath = Path.Combine(assemblyLocation, "../../../../Content");
            string contentPath = OriginalContentFolder;

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
            //Cursor = Cursors.WaitCursor;

            FyriTexture2d loadedTexture = texture2dManager.LoadTexture2d(fileName);

            if (loadedTexture != null)
            {
                if (!LoadedTexture2ds.Exists(p => p.OriginalFileName == fileName))
                    LoadedTexture2ds.Add(loadedTexture);

                //texture2dViewerControl.Texture = loadedTexture.Texture;
                AddTexture2dToTreeView(loadedTexture);
            }
            //Cursor = Cursors.Arrow;

            return loadedTexture;
        }

        void AddTexture2dToTreeView(FyriTexture2d textureToAdd)
        {
            if (Texture2dNode == null)
            {
                if (AssetsNode == null)
                {
                    AssetsNode = new TreeNode();
                    AssetsNode.Text = "Assets";
                    ProjectNameNode.Nodes.Add(AssetsNode);
                }
                Texture2dNode = new TreeNode();
                Texture2dNode.Text = "Textures";
                AssetsNode.Nodes.Add(Texture2dNode);
            }

            TreeNode nodeToAdd = new TreeNode();
            nodeToAdd.Text = textureToAdd.TextureName;
            nodeToAdd.Tag = textureToAdd;

            Texture2dNode.Nodes.Add(nodeToAdd);

            contentTreeView.CollapseAll();
            nodeToAdd.EnsureVisible();
            contentTreeView.Refresh();
        }

        #endregion

        #region Fonts

        public void LoadFontClick(object sender, EventArgs e)
        {
            BitmapFontExporter fileDialog = new BitmapFontExporter(this);

            // Default to the directory which contains our content files.
            //string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            //string relativePath = Path.Combine(assemblyLocation, "../../../../Content");
            string contentPath = OriginalContentFolder;

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
            //Cursor = Cursors.WaitCursor;

            FyriFont loadedFont = fontManager.LoadFont(fontName);

            if (loadedFont != null)
            {
                if (!LoadedFonts.Exists(p => p.FontName == fontName))
                    LoadedFonts.Add(loadedFont);

                //fontViewerControl.SpriteFont = loadedFont.Font;
                AddFontToTreeView(loadedFont);
            }
            //Cursor = Cursors.Arrow;

            return loadedFont;
        }

        void AddFontToTreeView(FyriFont fontToAdd)
        {
            if (FontNode == null)
            {
                if (AssetsNode == null)
                {
                    AssetsNode = new TreeNode();
                    AssetsNode.Text = "Assets";
                    ProjectNameNode.Nodes.Add(AssetsNode);
                }
                FontNode = new TreeNode();
                FontNode.Text = "Fonts";
                AssetsNode.Nodes.Add(FontNode);
            }

            TreeNode nodeToAdd = new TreeNode();
            nodeToAdd.Text = fontToAdd.FontName;
            nodeToAdd.Tag = fontToAdd;

            FontNode.Nodes.Add(nodeToAdd);

            contentTreeView.CollapseAll();
            nodeToAdd.EnsureVisible();
            contentTreeView.Refresh();
        }

        #endregion

        #region Effects

        public void LoadEffectClick(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            // Default to the directory which contains our content files.
            //string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            //string relativePath = Path.Combine(assemblyLocation, "../../../../Content");
            string contentPath = OriginalContentFolder;

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
            //Cursor = Cursors.WaitCursor;

            FyriEffect loadedEffect = effectManager.LoadEffect(effectName);

            if (loadedEffect != null)
            {
                if (!LoadedFonts.Exists(p => p.FontName == effectName))
                    LoadedEffects.Add(loadedEffect);

                //effectViewerControl.Effect = loadedEffect.Effect;
                AddEffectToTreeView(loadedEffect);
            }
            //Cursor = Cursors.Arrow;

            return loadedEffect;
        }

        void AddEffectToTreeView(FyriEffect effectToAdd)
        {
            if (EffectNode == null)
            {
                if (AssetsNode == null)
                {
                    AssetsNode = new TreeNode();
                    AssetsNode.Text = "Assets";
                    ProjectNameNode.Nodes.Add(AssetsNode);
                }
                EffectNode = new TreeNode();
                EffectNode.Text = "Effects";
                AssetsNode.Nodes.Add(EffectNode);
            }

            TreeNode nodeToAdd = new TreeNode();
            nodeToAdd.Text = effectToAdd.EffectName;
            nodeToAdd.Tag = effectToAdd;

            EffectNode.Nodes.Add(nodeToAdd);

            contentTreeView.CollapseAll();
            nodeToAdd.EnsureVisible();
            contentTreeView.Refresh();
        }

        #endregion

        #region Spriter

        public void LoadSpriterClick(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            // Default to the directory which contains our content files.
            //string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            //string relativePath = Path.Combine(assemblyLocation, "../../../../Content");
            string contentPath = OriginalContentFolder;

            fileDialog.InitialDirectory = contentPath;

            fileDialog.Title = "Load Spriter File";

            fileDialog.Filter = "Spriter Files (*.scml)|*.scml|" +
                                "All Files (*.*)|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadSpriterToProject(fileDialog.FileName);
            }
        }

        /// <summary>
        /// Loads a new 3D model file into the ModelViewerControl.
        /// </summary>
        FyriSpriter LoadSpriterToProject(string spriterName)
        {
            //Cursor = Cursors.WaitCursor;

            FyriSpriter loadedSpriter = spriterManager.LoadSpriter(spriterName);

            if (loadedSpriter != null)
            {
                if (!LoadedSpriters.Exists(p => p.SpriterName == spriterName))
                    LoadedSpriters.Add(loadedSpriter);

                //spriterViewerControl.Character = loadedSpriter.CharacterData.GetCharacterAnimator();
                AddSpriterToTreeView(loadedSpriter);
            }
            //Cursor = Cursors.Arrow;

            return loadedSpriter;
        }

        void AddSpriterToTreeView(FyriSpriter spriterToAdd)
        {
            if (SpriterNode == null)
            {
                SpriterNode = new TreeNode();
                SpriterNode.Text = "Spriters";
                ProjectNameNode.Nodes.Add(SpriterNode);
            }

            TreeNode nodeToAdd = new TreeNode();
            nodeToAdd.Text = spriterToAdd.SpriterName;
            nodeToAdd.Tag = spriterToAdd;

            SpriterNode.Nodes.Add(nodeToAdd);

            contentTreeView.CollapseAll();
            nodeToAdd.EnsureVisible();
            contentTreeView.Refresh();
        }

        #endregion
    }
}
