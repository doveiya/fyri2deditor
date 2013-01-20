/* 
 * Copyright (c) 2003-2005, University of Maryland
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided
 * that the following conditions are met:
 * 
 *		Redistributions of source code must retain the above copyright notice, this list of conditions
 *		and the following disclaimer.
 * 
 *		Redistributions in binary form must reproduce the above copyright notice, this list of conditions
 *		and the following disclaimer in the documentation and/or other materials provided with the
 *		distribution.
 * 
 *		Neither the name of the University of Maryland nor the names of its contributors may be used to
 *		endorse or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
 * PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR
 * TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 * Piccolo was written at the Human-Computer Interaction Laboratory www.cs.umd.edu/hcil by Jesse Grosjean
 * and ported to C# by Aaron Clamage under the supervision of Ben Bederson.  The Piccolo website is
 * www.cs.umd.edu/hcil/piccolo.
 */

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using XnaPiccolo;
using XnaPiccolo.Util;
using XnaPiccolo.Event;
using XnaPiccolo.Activities;
using XnaPiccoloX.Components;
using Fyri2dEditor;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Drawing.Imaging;
using Fyri2dEditor.Xna2dDrawingLibrary;
using Xna2dEditor;

namespace XnaPiccoloX 
{
	/// <summary>
	/// A delegate used to invoke the <see cref="PForm.Initialize">Initialize</see> method
	/// on the main event dispatch thread.
	/// </summary>
	public delegate void ProcessDelegate();

	/// <summary>
	/// <b>PForm</b> is meant to be subclassed by applications that just need a
	/// <see cref="PCanvas"/> in a <see cref="Form"/>.
	/// </summary>
	/// <remarks>
	/// PForm also provides full screen mode functionality.
	/// <para>
	/// <b>Notes to Inheritors:</b>  Subclasses should override the Initialize
	/// method and start adding their own code there.  Look in the
	/// XnaPiccoloExamples package to see some uses of PForm.
	/// </para>
	/// </remarks>
	public class PForm : System.Windows.Forms.Form 
    {
        #region Xna2dDrawing Fields
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
        XnaGraphics xnaGraphics;

        ContentBuilder contentBuilder;
        ContentManager contentManager;

        FyriProject currentProject;

        TreeNode ProjectNameNode;
        TreeNode ModelNode;
        TreeNode Texture2dNode;
        TreeNode FontNode;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newProjectToolStripMenuItem;
        private ToolStripMenuItem openProjectToolStripMenuItem;
        private ToolStripMenuItem saveProjectToolStripMenuItem;
        private ToolStripMenuItem closeProjectToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem loadModelToolStripMenuItem1;
        private ToolStripMenuItem loadTexture2dToolStripMenuItem;
        private ToolStripMenuItem loadFontToolStripMenuItem;
        private ToolStripMenuItem loadEffectToolStripMenuItem;
        private ToolStripMenuItem loadSpriteFontToolStripMenuItem;
        TreeNode EffectNode;

        #endregion

        #region Xna2dDrawing Methods



        #region File Methods

        #region Project
        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public void RefreshProject()
        {
            //ProjectNameNode.Text = "Project Name";

            //ProjectNameNode.Nodes.Clear();

            Canvas.XnaGraphics = null;
            Canvas.DrawingBatch = null;
            Canvas.DrawingContext = null;
            Canvas.LineBatch = null;
            Canvas.SpriteFont = null;
            Canvas.Effect = null;
            Canvas.Texture = null;

            if (xnaGraphics != null)
            {
                xnaGraphics = null;
            }

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
                    if (File.Exists(defSphereFileName))
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

                xnaGraphics = new XnaGraphics(this.graphicsDeviceService.GraphicsDevice, drawingContext);

                PDefaults.DefaultGraphics = xnaGraphics;
                PDefaults.DefaultSpriteFont = fontManager.GetFont("SpriteFont").Font;

                Canvas.SpriteFont = segoeUIFont.Font;
                Canvas.Effect = roundlineEffect.Effect;
                Canvas.Texture = tulipsPng.Texture;
                Canvas.DrawingBatch = drawingBatch;
                Canvas.DrawingContext = drawingContext;
                Canvas.LineBatch = lineBatch;
                Canvas.XnaGraphics = xnaGraphics;

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
                    if (!currentProject.LoadedModels.Exists(p => p.OriginalFileName == fileName))
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

        }

        private void loadSpriteFontToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
                Canvas.LoadContent();
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

        #endregion

		#region PForm Fields
		private PCanvas canvas;
		private PScrollableControl scrollableControl;
		private ProcessDelegate processDelegate;
		private Rectangle nonFullScreenBounds;
		private bool fullScreenMode;


		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a new PForm.
		/// </summary>
		public PForm() 
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

                InitializePiccolo(false, null);

                OpenDefaultProject();
                RefreshProject();
            }
		}

		/// <summary>
		/// Constructs a new PForm, with the given canvas, in full screen mode if
		/// specified.
		/// </summary>
		/// <param name="fullScreenMode">
		/// Determines whether this PForm starts in full screen mode.
		/// </param>
		/// <param name="aCanvas">The canvas to add to this PForm.</param>
		/// <remarks>
		/// A <c>null</c> value can be passed in for <c>aCanvas</c>, in which a case
		/// a new canvas will be created.
		/// </remarks>
		public PForm(bool fullScreenMode, PCanvas aCanvas) 
        {
			InitializeComponent();
			InitializePiccolo(fullScreenMode, aCanvas);
		}

		/// <summary>
		/// Sets up the form, sizing and anchoring the canvas.
		/// </summary>
		/// <param name="fullScreenMode">
		/// Indicates whether or not to start up in full screen mode.
		/// </param>
		/// <param name="aCanvas">
		/// The canvas to add to this PForm; can be null.
		/// </param>
		public void InitializePiccolo(bool fullScreenMode, PCanvas aCanvas) 
        {
			if (aCanvas == null) 
            {
				canvas = new PCanvas();
			} 
            else 
            {
				canvas = aCanvas;
			}

			canvas.Focus();
			BeforeInitialize();

			scrollableControl = new PScrollableControl(canvas);
			AutoScrollCanvas = false;

			//Note: If the main application form, generated by visual studio, is set to
			//extend PForm, the InitializeComponent will set the bounds after this statement
			Bounds = DefaultFormBounds;

			this.SuspendLayout();
			canvas.Size = ClientSize;
			scrollableControl.Size = ClientSize;
			this.Controls.Add(scrollableControl);

			scrollableControl.Anchor = 
				AnchorStyles.Bottom |
				AnchorStyles.Top |
				AnchorStyles.Left |
				AnchorStyles.Right;

			this.ResumeLayout(false);

			FullScreenMode = fullScreenMode;
		}
		#endregion

		#region Basic
		/// <summary>
		/// Gets this form's canvas.
		/// </summary>
		/// <value>This form's canvas.</value>
		public virtual PCanvas Canvas 
        {
			get { return canvas; }
		}

		/// <summary>
		/// Gets the scrollable control that contains the canvas.
		/// </summary>
		/// <value>The scrollable control that contains the canvas.</value>
		public virtual PScrollableControl ScrollControl 
        {
			get { return scrollableControl; }
		}

		/// <summary>
		/// Gets or sets a value that indicates whether or not this form's canvas is
		/// scrollable.
		/// </summary>
		public virtual bool AutoScrollCanvas 
        {
			get 
            {
				return scrollableControl.Scrollable;
			}
			set 
            {
				scrollableControl.Scrollable = value;
			}
		}

		/// <summary>
		/// Gets or sets the view position of the canvas, if it is scrollable.
		/// </summary>
		public virtual Point AutoScrollCanvasPosition 
        {
			get 
            {
				return new Point((int)scrollableControl.ViewPosition.X, (int)scrollableControl.ViewPosition.Y);
			}
			set 
            {
				scrollableControl.ViewPosition = new Xna2dEditor.PointFx(value.X, value.Y);
			}
		}

		/// <summary>
		/// Gets the default bounds to use for this form.
		/// </summary>
		public virtual Rectangle DefaultFormBounds 
        {
			get 
            {
				return new Rectangle(100, 100, 400, 400);
			}
		}

		/// <summary>
		/// Gets or sets the bounds this form should revert to when full screen mode is exited
		/// </summary>
		public virtual Rectangle NonFullScreenBounds 
        {
			get 
            {
				return nonFullScreenBounds;
			}
			set 
            {
				nonFullScreenBounds = value;
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether or not this form should be viewed
		/// in full screen mode.
		/// </summary>
		/// <value>
		/// A value that indicates whether or not this form should be viewed in full screen
		/// mode.
		/// </value>
		public virtual bool FullScreenMode 
        {
			get 
            {
				return fullScreenMode;
			}
			set 
            {
				if (fullScreenMode != value) 
                {
					fullScreenMode = value;

					if (fullScreenMode) 
                    {
						AddEscapeFullScreenModeHandler();
						FormBorderStyle = FormBorderStyle.None;
						this.NonFullScreenBounds = this.Bounds;
						this.Bounds = Screen.PrimaryScreen.Bounds;
						this.BringToFront();
					}
					else 
                    {
						RemoveEscapeFullScreenModeHandler();
						FormBorderStyle = FormBorderStyle.Sizable;
						this.Bounds = this.NonFullScreenBounds;
					}
				}
			}
		}

		/// <summary>
		/// This method adds a key event handler that will take this PForm out of full
		/// screen mode when the escape key is pressed.
		/// </summary>
		/// <remarks>
		/// This is called for you automatically when the form enters full screen mode.
		/// </remarks>
		public virtual void AddEscapeFullScreenModeHandler() 
        {
			canvas.KeyDown += new KeyEventHandler(canvas_KeyDown);
		}

		/// <summary>
		/// This method removes the escape full screen mode key event handler.
		/// </summary>
		/// <remarks>
		/// This is called for you automatically when full screen mode exits, but the
		/// method has been made public for applications that wish to use other methods
		/// for exiting full screen mode.
		/// </remarks>
		public virtual void RemoveEscapeFullScreenModeHandler() 
        {
			canvas.KeyDown -= new KeyEventHandler(canvas_KeyDown);
		}

		/// <summary>
		/// Exits full screen mode when the escape key is pressed.
		/// </summary>
		/// <param name="sender">The source of the KeyEvent.</param>
		/// <param name="e">A KeyEventArgs that contains the event data.</param>
		protected virtual void canvas_KeyDown(object sender, KeyEventArgs e) 
        {
			if (e.KeyCode == Keys.Escape) 
            {
				FullScreenMode = false;
			} 
            else if (e.KeyCode == Keys.M) 
            {
				FullScreenMode = false;
				this.WindowState = FormWindowState.Minimized;
			}
		}
		#endregion

		#region Initialize
		/// <summary>
		/// This method will be called before the <see cref="Initialize"/> method and will
		/// be called on the thread that is constructing this object.
		/// </summary>
		public virtual void BeforeInitialize() 
        {
		}

		/// <summary>
		/// Subclasses should override this method and add their Piccolo initialization code
		/// there.
		/// </summary>
		/// <remarks>
		/// This method will be called on the main event dispatch thread.  Note that the
		/// constructors of PForm subclasses may not be complete when this method is called.
		/// If you need to initailize some things in your class before this method is called
		/// place that code in <see cref="BeforeInitialize"/>.
		/// </remarks>
		public virtual void Initialize() 
        {
		}

		/// <summary>
		/// Overridden.  Invokes the initialize method on the event dispatch thread.
		/// </summary>
		/// <remarks>
		/// The handle of the canvas should exist at this point, so it is safe to call invoke.
		/// </remarks>
		protected override void OnCreateControl() 
        {
			base.OnCreateControl ();

			//Force visible
			this.Visible = true;
			this.Refresh();

			//Necessary to invalidate the bounds because the state will be incorrect since
			//the message loop did not exist when the inpts were scheduled.
			this.canvas.Root.InvalidateFullBounds();
			this.canvas.Root.InvalidatePaint();

			this.processDelegate = new ProcessDelegate(Initialize);
			canvas.Invoke(processDelegate);
		}
		#endregion

		#region Dispose
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
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
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.loadToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(292, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
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
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openProjectToolStripMenuItem.Text = "Open Project...";
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveProjectToolStripMenuItem.Text = "Save Project";
            // 
            // closeProjectToolStripMenuItem
            // 
            this.closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
            this.closeProjectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeProjectToolStripMenuItem.Text = "Close Project";
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
            // 
            // loadTexture2dToolStripMenuItem
            // 
            this.loadTexture2dToolStripMenuItem.Name = "loadTexture2dToolStripMenuItem";
            this.loadTexture2dToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.loadTexture2dToolStripMenuItem.Text = "Load Texture2d...";
            // 
            // loadFontToolStripMenuItem
            // 
            this.loadFontToolStripMenuItem.Name = "loadFontToolStripMenuItem";
            this.loadFontToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.loadFontToolStripMenuItem.Text = "Load Font...";
            // 
            // loadEffectToolStripMenuItem
            // 
            this.loadEffectToolStripMenuItem.Name = "loadEffectToolStripMenuItem";
            this.loadEffectToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.loadEffectToolStripMenuItem.Text = "Load Effect...";
            // 
            // loadSpriteFontToolStripMenuItem
            // 
            this.loadSpriteFontToolStripMenuItem.Name = "loadSpriteFontToolStripMenuItem";
            this.loadSpriteFontToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.loadSpriteFontToolStripMenuItem.Text = "Load SpriteFont...";
            // 
            // PForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.menuStrip1);
            this.Name = "PForm";
            this.Text = "PForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
	}
}
