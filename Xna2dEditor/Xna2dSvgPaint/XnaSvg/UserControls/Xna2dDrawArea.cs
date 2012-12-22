#region File Description
//-----------------------------------------------------------------------------
// Xna2dEditorControl.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Fyri2dEditor.Xna2dDrawingLibrary;
using System.Collections;
using System.ComponentModel;
using Draw;
using System.Xml;
using SVGLib;
using System.IO;
using System.Globalization;
using Xna2dEditor.UserControls;
#endregion

namespace Fyri2dEditor
{
    public class Xna2dDrawArea : GraphicsDeviceUserControl
    {
        #region Constructors

        public Xna2dDrawArea()
        {
            _height = 500;
            _width = 400;
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        #endregion Constructors

        #region Draw Area Fields

        public ArrayList ChartValues = new ArrayList();
        public Boolean Dirty;
        public float ScaleX, ScaleY;
        public string Title = "Default Title";
        public float Xdivs = 2, Ydivs = 2, MajorIntervals = 100;
        public float Xorigin, Yorigin;

        private IContainer components;

        // (instances of XnaDrawObject-derived classes)
        private Xna2dDrawToolType _activeTool; // active drawing tool
        private ToolStripMenuItem _bringToFrontToolStripMenuItem;
        private ContextMenuStrip _contextMenuStrip;
        private ToolStripMenuItem _copyToolStripMenuItem;
        private ToolStripMenuItem _cutToolStripMenuItem;
        private ToolStripMenuItem _deleteToolStripMenuItem;
        private XnaGraphicsList _graphicsList; // list of draw objects
        private string _mDescription = "Svg picture";
        private Point _mOriginalSize = new Point(500, 400);

        // group selection rectangle
        // Information about owner form
        private Point _mScale = new Point((int)1.0f, (int)1.0f);
        private Point _mSizePicture = new Point(500, 400);
        private ToolStripMenuItem _pasteToolStripMenuItem;
        private ToolStripMenuItem _selectAllToolStripMenuItem;
        private ToolStripMenuItem _sendToBackToolStripMenuItem;
        private Tool[] _tools; // array of tools
        private ToolStripSeparator _toolStripSeparator1;
        private ToolStripSeparator _toolStripSeparator2;
        private ToolStripSeparator _toolStripSeparator3;
        private int _width, _height;

        public String FileName { get; set; }

        #endregion Fields        

        #region Draw Area Enumerations

        public enum Xna2dDrawToolType
        {
            Pointer,
            Rectangle,
            Ellipse,
            Line,
            Polygon,
            Bitmap,
            Text,
            Pan,
            Path,
            NumberOfDrawTools
        }

        #endregion Enumerations

        #region Draw Area Delegates

        public delegate void OnItemsInSelection(object sender, MouseEventArgs e);

        public delegate void OnMousePan(object sender, MouseEventArgs e);

        public delegate void OnMouseSelectionDone(object sender, EventArgs e);

        #endregion Delegates

        #region Draw Area Events

        public event OnItemsInSelection ItemsSelected;

        public event OnMousePan MousePan;

        public event OnMouseSelectionDone ToolDone;

        #endregion Events

        #region Draw Area Properties

        /// <summary>
        /// Active drawing tool.
        /// </summary>
        public Xna2dDrawToolType ActiveTool
        {
            get
            {
                return _activeTool;
            }
            set
            {
                if (_tools != null)
                    _tools[(int)_activeTool].ToolActionCompleted();
                _activeTool = value;
            }
        }

        public string Description
        {
            get
            {
                return _mDescription;
            }
            set
            {
                _mDescription = value;
            }
        }

        /// <summary>
        /// Decide whether to display grid
        /// </summary>
        public bool DrawGrid
        {
            get;
            set;
        }

        /// <summary>
        /// Flas is set to true if group selection rectangle should be drawn.
        /// </summary>
        public bool DrawNetRectangle
        {
            get;
            set;
        }

        /// <summary>
        /// List of graphics objects.
        /// </summary>
        [CLSCompliant(false)]
        public XnaGraphicsList GraphicsList
        {
            get
            {
                return _graphicsList;
            }
            set
            {
                _graphicsList = value;
            }
        }

        /// <summary>
        /// Group selection rectangle. Used for drawing.
        /// </summary>
        public Rectangle NetRectangle
        {
            get;
            set;
        }

        public Point OldScale
        {
            get
            {
                return _mScale;
            }
            set
            {
                _mScale = value;
            }
        }

        public Point OriginalSize
        {
            get
            {
                return _mOriginalSize;
            }
            set
            {
                _mOriginalSize = value;
            }
        }

        /// <summary>
        /// Reference to the owner form
        /// </summary>
        public Control Owner
        {
            get;
            set;
        }

        public Point ScaleDraw
        {
            get
            {
                return _mScale;
            }
            set
            {
                _mScale = value;
            }
        }

        public Point SizePicture
        {
            get
            {
                return _mSizePicture;
            }
            set
            {
                _mSizePicture = value;
            }
        }

        #endregion Properties

        #region Xna Events 
        public event ContextEventHandler ContextChanged;
        protected virtual void OnContextChanged(ContextEventArgs e)
        {
            ContextChanged(this, e);
        }
        #endregion

        #region Update Fields
        // Timer controls the rotation speed.
        Stopwatch timer;
        SpriteBatch spriteBatch;
        GameTime gameTimer;
        GameTime previousGameTimer;
        #endregion

        #region Xna General Fields

        string[] roundLineTechniqueNames;

        DrawingTexture drawingTexture;

        #endregion

        #region Xna General Properties

        public Texture2D Texture
        {
            get { return texture; }

            set
            {
                texture = value;
            }
        }

        private Texture2D texture;

        public XnaDrawingContext DrawingContext
        {
            get { return drawingContext; }

            set
            {
                XnaDrawingContext oldValue = drawingContext;
                drawingContext = value;
                if(oldValue != drawingContext)
                    OnContextChanged(new ContextEventArgs(oldValue, drawingContext));
                //if (lineBatch != null)
                //roundLineTechniqueNames = lineBatch.TechniqueNames;
            }
        }

        private XnaDrawingContext drawingContext;

        public XnaDrawingBatch DrawingBatch
        {
            get { return drawingBatch; }

            set
            {
                drawingBatch = value;
                //if (lineBatch != null)
                    //roundLineTechniqueNames = lineBatch.TechniqueNames;
            }
        }

        private XnaDrawingBatch drawingBatch;

        /// <summary>
        /// Gets or sets the current model.
        /// </summary>
        public XnaLine2dBatch LineBatch
        {
            get { return lineBatch; }

            set
            {
                lineBatch = value;
                if (lineBatch != null)
                    roundLineTechniqueNames = lineBatch.TechniqueNames;
            }
        }

        XnaLine2dBatch lineBatch;

        /// <summary>
        /// Gets or sets the current model.
        /// </summary>
        public SpriteFont SpriteFont
        {
            get { return font; }

            set
            {
                font = value;
            }
        }

        SpriteFont font;

        /// <summary>
        /// Gets or sets the current model.
        /// </summary>
        public Effect Effect
        {
            get { return effect; }

            set
            {
                effect = value;
            }
        }

        Effect effect;

        #endregion

        #region Draw Area Methods

        public void DoScaling(Point sc)
        {
            XnaDrawObject.Zoom = sc.Y;
            _graphicsList.Resize(sc, _mScale);
            _mScale = sc;
            _mSizePicture = new Point(_mScale.X * OriginalSize.X,
                _mScale.Y * OriginalSize.Y);
        }

        public void Draw(XnaDrawingContext g)
        {
            //var brush = new SolidBrush(System.Drawing.Color.FromArgb(255, 255, 255));
            var brushColor = Color.FromNonPremultiplied(255, 255, 255, 255);
            Rectangle clientRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            g.DrawFilledRectangle(clientRectangle, brushColor);
            // draw rect svg size
            var penColor = Color.FromNonPremultiplied(0, 0, 255, 255);
            //var pen = new Pen(System.Drawing.Color.FromArgb(0, 0, 255), 1);
            g.DrawRectangle(0, 0, SizePicture.X, SizePicture.Y, penColor);
            if (_graphicsList != null)
            {
                _graphicsList.Draw(g);
            }
            //brush.Dispose();
        }

        /// <summary>
        ///  Draw group selection rectangle
        /// </summary>
        /// <param name="g"></param>
        public void DrawNetSelection(XnaDrawingContext g)
        {
            if (!DrawNetRectangle)
                return;
            //var r = new System.Drawing.Rectangle(Convert.ToInt32(NetRectangle.X), Convert.ToInt32(NetRectangle.Y),
            //    Convert.ToInt32(NetRectangle.Width), Convert.ToInt32(NetRectangle.Height));
            g.DrawRectangle(NetRectangle, Color.Black);
            //ControlPaint.DrawFocusRectangle(g, r, System.Drawing.Color.Black, System.Drawing.Color.Transparent);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="owner"></param>
        public void Initialize(XnaItems xnaItems, Control owner)
        {
            //SetStyle(ControlStyles.AllPaintingInWmPaint |
            //    ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            // Keep reference to owner form
            Owner = owner;

            drawingContext = xnaItems.DrawingContext;
            lineBatch = xnaItems.LineBatch;
            this.SpriteFont = xnaItems.FontManager.GetFont("SpriteFont").Font;

            // set default tool
            _activeTool = Xna2dDrawToolType.Pointer;

            // create list of graphic objects
            _graphicsList = new XnaGraphicsList();

            // create array of drawing tools
            _tools = new Tool[(int)Xna2dDrawToolType.NumberOfDrawTools];
            _tools[(int)Xna2dDrawToolType.Pointer] = new ToolPointer();
            _tools[(int)Xna2dDrawToolType.Rectangle] = new ToolRectangle();
            _tools[(int)Xna2dDrawToolType.Ellipse] = new ToolEllipse();
            _tools[(int)Xna2dDrawToolType.Line] = new ToolLine();
            _tools[(int)Xna2dDrawToolType.Polygon] = new ToolPolygon();
            _tools[(int)Xna2dDrawToolType.Text] = new XnaToolText();
            _tools[(int)Xna2dDrawToolType.Bitmap] = new ToolImage();
            _tools[(int)Xna2dDrawToolType.Pan] = new ToolPan();
            _tools[(int)Xna2dDrawToolType.Path] = new XnaToolPath();

            System.Drawing.Graphics g = Owner.CreateGraphics();
            XnaDrawObject.Dpi = new Point((int)1, (int)1);

            Create2DProjectionMatrix();
        }

        /// <summary>
        /// Create a simple 2D projection matrix
        /// </summary>
        public void Create2DProjectionMatrix()
        {
            // Projection matrix ignores Z and just squishes X or Y to balance the upcoming viewport stretch
            float projScaleX;
            float projScaleY;
            float width = this.GraphicsDevice.Viewport.Width;
            float height = this.GraphicsDevice.Viewport.Height;
            if (width > height)
            {
                // Wide window
                projScaleX = height / width;
                projScaleY = 1.0f;
            }
            else
            {
                // Tall window
                projScaleX = 1.0f;
                projScaleY = width / height;
            }
            projMatrix = Matrix.CreateScale(projScaleX, projScaleY, 0.0f);
            projMatrix.M43 = 0.5f;
        }

        public bool LoadFromXml(XmlTextReader reader)
        {
            ErrH.Log("DrawArea", "LoadFromXML", "", ErrH._LogPriority.Info);
            _graphicsList.Clear();
            var svg = new SvgDoc();
            if (!svg.LoadFromFile(reader))
                return false;
            SvgRoot root = svg.GetSvgRoot();

            if (root == null)
                return false;
            try
            {
                SizePicture = new Point((int)XnaDrawObject.ParseSize(root.Width, XnaDrawObject.Dpi.X),
                    (int)XnaDrawObject.ParseSize(root.Height, XnaDrawObject.Dpi.Y));
            }
            catch
            {
            }
            _mOriginalSize = SizePicture;
            SvgElement ele = root.getChild();
            _mScale = new Point(1, 1);
            if (ele != null)
                _graphicsList.AddFromSvg(ele);

            Description = _graphicsList.Description;
            return true;
        }

        public void MkResize()
        {
            Point oldscale = _mScale;
            _mScale.X = _width / _mOriginalSize.X;
            _mScale.Y = _height / _mOriginalSize.Y;
            _graphicsList.Resize(_mScale, oldscale);
            SizePicture = new Point((int)XnaDrawObject.RecalcFloat(SizePicture.X, _mScale.X, oldscale.X),
                (int)XnaDrawObject.RecalcFloat(SizePicture.Y, _mScale.Y, oldscale.Y));
        }

        public void RestoreScale()
        {
            _graphicsList.Resize(new Point(1, 1), _mScale);
            _mScale = new Point(1, 1);
        }

        public bool SaveToXml(StreamWriter sw)
        {
            try
            {
                const string mSXmlDeclaration = "<?xml version=\"1.0\" standalone=\"no\"?>";
                const string mSXmlDocType = "<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.0//EN\" \"http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd\">";

                string sXml = mSXmlDeclaration + "\r\n";
                sXml += mSXmlDocType + "\r\n";
                sXml += "<svg width=\"" + _mOriginalSize.X.ToString(CultureInfo.InvariantCulture) +
                    "\" height=\"" + _mOriginalSize.Y.ToString(CultureInfo.InvariantCulture) + "\">" + "\r\n";
                sXml += "<desc>" + Description + "</desc>" + "\r\n";
                sXml += _graphicsList.GetXmlString(_mScale);
                sXml += "</svg>" + "\r\n";
                sw.Write(sXml);
                sw.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Set dirty flag (file is changed after last save operation)
        /// </summary>
        public void SetDirty()
        {
            Dirty = true;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Mouse down.
        /// Left button down event is passed to active tool.
        /// Right button down event is handled in this class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_tools[(int)_activeTool].IsComplete)
            {
                _tools[(int)_activeTool].OnMouseDown(this, e);
                if (e.Button == MouseButtons.Right)
                {
                    if (_tools[(int)_activeTool].IsComplete)
                    {
                        _activeTool = Xna2dDrawToolType.Pointer;
                        ToolDone(sender, e);
                        Refresh();
                    }
                }
                return;
            }
            if (e.Button == MouseButtons.Left)
                _tools[(int)_activeTool].OnMouseDown(this, e);
            else if (e.Button == MouseButtons.Right)
                OnContextMenu(e);

            if (_graphicsList.IsAnythingSelected() && (!_tools[(int)_activeTool].IsComplete))
            {
                if (ItemsSelected != null)
                    ItemsSelected(_graphicsList.GetAllSelected(), e);
            }
        }

        /// <summary>
        /// Mouse move.
        /// Moving without button pressed or with left button pressed
        /// is passed to active tool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawArea_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left || e.Button == MouseButtons.None)
                {
                    if (_activeTool == Xna2dDrawToolType.Pan)
                    {
                        if (MousePan != null)
                        {
                            MousePan(sender, e);
                        }
                    }

                    var ind = (int)_activeTool;
                    _tools[ind].OnMouseMove(this, e);
                }
                else
                    Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                ErrH.Log("DrawArea", "DrawArea_MouseMove", ex.ToString(), ErrH._LogPriority.Info);
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Mouse up event.
        /// Left button up event is passed to active tool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawArea_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _tools[(int)_activeTool].OnMouseUp(this, e);
                bool res = _tools[(int)_activeTool].IsComplete;

                // if (activeTool != DrawToolType.Pan)
                if (res)
                {
                    ToolDone(sender, e);
                    ActiveTool = Xna2dDrawToolType.Pointer;
                }
                else
                {
                    Refresh();
                }
            }

            if (_graphicsList.GetAllSelected().Count > 0)
            {
                if (ItemsSelected != null)
                    ItemsSelected(_graphicsList.GetAllSelected(), e);
            }
        }

        /// <summary>
        /// Draw graphic objects and 
        /// group selection rectangle (optionally)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawArea_Paint(object sender, PaintEventArgs e)
        {
            //var brush = new SolidBrush(System.Drawing.Color.FromArgb(255, 255, 255));
            //e.Graphics.FillRectangle(brush,
            //    ClientRectangle);
            //// draw rect svg size

            //if (DrawGrid)
            //    DrawGridsAndScale(e.Graphics);

            //if (_graphicsList != null)
            //    _graphicsList.Draw(e.Graphics);

            //DrawNetSelection(e.Graphics);
            //brush.Dispose();
        }

        List<XnaLine2d> majorLines = new List<XnaLine2d>();
        List<XnaLine2d> minorLines = new List<XnaLine2d>();

        Matrix viewMatrix;
        Matrix projMatrix;
        float cameraX = 0;
        float cameraY = 0;
        float cameraZoom = 300;

        private void DrawGridsAndScale()
        {
            Matrix viewProjMatrix = viewMatrix * projMatrix;

            //var majorlinesPen = new Pen(System.Drawing.Color.Wheat, 1);
            //var minorlinesPen = new Pen(System.Drawing.Color.LightGray, 1);
            majorLines.Clear();
            minorLines.Clear();

            Xorigin = Yorigin = 0;
            ScaleX = _width;
            ScaleY = _height;

            _width = (int)(SizePicture.X);
            _height = (int)(SizePicture.Y);

            var xMajorLines = (int)(_width / MajorIntervals / ScaleDraw.X);
            var yMajorLines = (int)(_height / MajorIntervals / ScaleDraw.Y);

            try
            {
                //draw X Axis major lines
                for (int i = 0; i <= xMajorLines; i++)
                {
                    float x = i * (_width / xMajorLines);
                    minorLines.Add(new XnaLine2d(x, 0.0f, x, _height, 4.0f, Color.LightGray));

                    //draw X Axis minor lines
                    for (int i1 = 1; i1 <= Xdivs; i1++)
                    {
                        float x1 = i1 * MajorIntervals / (Xdivs) * ScaleDraw.X;
                        majorLines.Add(new XnaLine2d(x + x1, 0, x + x1, _height, 4.0f, Color.Wheat));
                    }
                }

                //draw Y Axis major lines
                for (int i = 0; i <= yMajorLines; i++)
                {
                    //y = i * (Height / (yMajorLines));
                    float y = i * MajorIntervals * ScaleDraw.Y;
                    minorLines.Add(new XnaLine2d(0, y, _width, y, 4.0f, Color.LightGray));

                    //draw Y Axis minor lines
                    for (int i1 = 1; i1 <= Ydivs; i1++)
                    {
                        float y1 = i1 * MajorIntervals / (Ydivs) * ScaleDraw.Y;
                        majorLines.Add(new XnaLine2d(0, y + y1, _width, y + y1, 4.0f, Color.Wheat));
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            float time = (float)gameTimer.TotalGameTime.TotalSeconds;
            string curTechniqueName = roundLineTechniqueNames[0];

            if (LineBatch != null)
            {
                lineBatch.Draw(minorLines, viewProjMatrix, time, curTechniqueName);
                lineBatch.Draw(majorLines, viewProjMatrix, time, curTechniqueName);
            }
        }

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
            _contextMenuStrip = new ContextMenuStrip(components);
            _selectAllToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator1 = new ToolStripSeparator();
            _bringToFrontToolStripMenuItem = new ToolStripMenuItem();
            _sendToBackToolStripMenuItem = new ToolStripMenuItem();
            _deleteToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator2 = new ToolStripSeparator();
            _cutToolStripMenuItem = new ToolStripMenuItem();
            _copyToolStripMenuItem = new ToolStripMenuItem();
            _pasteToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator3 = new ToolStripSeparator();
            _contextMenuStrip.SuspendLayout();
            SuspendLayout();
            //
            // contextMenuStrip
            //
            _contextMenuStrip.Items.AddRange(new ToolStripItem[] {
            _selectAllToolStripMenuItem,
            _toolStripSeparator1,
            _bringToFrontToolStripMenuItem,
            _sendToBackToolStripMenuItem,
            _toolStripSeparator3,
            _deleteToolStripMenuItem,
            _toolStripSeparator2,
            _cutToolStripMenuItem,
            _copyToolStripMenuItem,
            _pasteToolStripMenuItem});
            _contextMenuStrip.Name = @"_contextMenuStrip";
            _contextMenuStrip.Size = new System.Drawing.Size(153, 198);
            //
            // selectAllToolStripMenuItem
            //
            _selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            _selectAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            _selectAllToolStripMenuItem.Text = @"Select All";
            _selectAllToolStripMenuItem.Click += SelectAllToolStripMenuItemClick;
            //
            // toolStripSeparator1
            //
            _toolStripSeparator1.Name = "toolStripSeparator1";
            _toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            //
            // bringToFrontToolStripMenuItem
            //
            _bringToFrontToolStripMenuItem.Name = "bringToFrontToolStripMenuItem";
            _bringToFrontToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            _bringToFrontToolStripMenuItem.Text = @"Bring to Front";
            _bringToFrontToolStripMenuItem.Click += BringToFrontToolStripMenuItemClick;
            //
            // sendToBackToolStripMenuItem
            //
            _sendToBackToolStripMenuItem.Name = "sendToBackToolStripMenuItem";
            _sendToBackToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            _sendToBackToolStripMenuItem.Text = @"Send to Back";
            _sendToBackToolStripMenuItem.Click += SendToBackToolStripMenuItemClick;
            //
            // deleteToolStripMenuItem
            //
            _deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            _deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            _deleteToolStripMenuItem.Text = @"Delete";
            _deleteToolStripMenuItem.Click += DeleteToolStripMenuItemClick;
            //
            // toolStripSeparator2
            //
            _toolStripSeparator2.Name = "toolStripSeparator2";
            _toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            //
            // cutToolStripMenuItem
            //
            _cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            _cutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            _cutToolStripMenuItem.Text = @"Cut";
            _cutToolStripMenuItem.Click += CutToolStripMenuItemClick;
            //
            // copyToolStripMenuItem
            //
            _copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            _copyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            _copyToolStripMenuItem.Text = @"Copy";
            _copyToolStripMenuItem.Click += CopyToolStripMenuItemClick;
            //
            // pasteToolStripMenuItem
            //
            _pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            _pasteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            _pasteToolStripMenuItem.Text = @"Paste";
            _pasteToolStripMenuItem.Click += PasteToolStripMenuItemClick;
            //
            // toolStripSeparator3
            //
            _toolStripSeparator3.Name = "toolStripSeparator3";
            _toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
            //
            // DrawArea
            //
            AutoScroll = true;
            AutoSize = true;
            Name = "DrawArea";
            Size = new System.Drawing.Size(153, 136);
            //Paint += DrawArea_Paint;
            MouseMove += DrawArea_MouseMove;
            MouseDown += DrawArea_MouseDown;
            MouseUp += DrawArea_MouseUp;
            _contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        /// <summary>
        /// Right-click handler
        /// </summary>
        /// <param name="e"></param>
        private void OnContextMenu(MouseEventArgs e)
        {
            // Change current selection if necessary

            var point = new Point(e.X, e.Y);

            int n = GraphicsList.Count;
            XnaDrawObject o = null;

            for (int i = 0; i < n; i++)
            {
                if (GraphicsList[i].HitTest(point) == 0)
                {
                    o = GraphicsList[i];
                    break;
                }
            }

            if (o != null)
            {
                if (!o.Selected)
                    GraphicsList.UnselectAll();

                // Select clicked object
                o.Selected = true;
                _bringToFrontToolStripMenuItem.Enabled = true;
                _sendToBackToolStripMenuItem.Enabled = true;
                _cutToolStripMenuItem.Enabled = true;
                _copyToolStripMenuItem.Enabled = true;
                _deleteToolStripMenuItem.Enabled = true;
            }
            else
            {
                _bringToFrontToolStripMenuItem.Enabled = false;
                _sendToBackToolStripMenuItem.Enabled = false;
                _cutToolStripMenuItem.Enabled = false;
                _copyToolStripMenuItem.Enabled = false;
                _deleteToolStripMenuItem.Enabled = false;
                GraphicsList.UnselectAll();
            }

            _pasteToolStripMenuItem.Enabled = GraphicsList.AreItemsInMemory();
            _contextMenuStrip.Show(MousePosition);
            Refresh();
        }

        private void CutToolStripMenuItemClick(object sender, EventArgs e)
        {
            _graphicsList.CutSelection();
            Refresh();
        }

        private void CopyToolStripMenuItemClick(object sender, EventArgs e)
        {
            _graphicsList.CopySelection();
            Refresh();
        }

        private void PasteToolStripMenuItemClick(object sender, EventArgs e)
        {
            _graphicsList.PasteSelection();
            Refresh();
        }

        private void SelectAllToolStripMenuItemClick(object sender, EventArgs e)
        {
            _graphicsList.SelectAll();
            Refresh();
        }

        private void SendToBackToolStripMenuItemClick(object sender, EventArgs e)
        {
            _graphicsList.MoveSelectionToBack();
            Refresh();
        }


        private void BringToFrontToolStripMenuItemClick(object sender, EventArgs e)
        {
            _graphicsList.MoveSelectionToFront();
            Refresh();
        }

        private void DeleteToolStripMenuItemClick(object sender, EventArgs e)
        {
            _graphicsList.DeleteSelection();
            Refresh();
        }

        public void MoveCommand(ArrayList movedItemsList, Point delta)
        {
            _graphicsList.Move(movedItemsList, delta);
            Refresh();
        }

        public void PropertyChanged(GridItem itemChanged, object oldVal)
        {
            _graphicsList.PropertyChanged(itemChanged, oldVal);
        }

        public void ResizeCommand(XnaDrawObject resizedItems, Point old, Point newP, int handle)
        {
            _graphicsList.ResizeCommand(resizedItems, old, newP, handle);
            Refresh();
        }
        #endregion Methods

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            // Start the animation timer.
            timer = Stopwatch.StartNew();

            gameTimer = new GameTime();

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };

            spriteBatch = new SpriteBatch(this.GraphicsDevice);
        }

        public void LoadContent()
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            int bufferWidth = pp.BackBufferWidth;
            int bufferHeight = pp.BackBufferHeight;
            drawingTexture = new DrawingTexture(GraphicsDevice, bufferWidth, bufferHeight);
            drawingTexture.Clear(Microsoft.Xna.Framework.Color.White);

            XnaDrawingContext drawingContext = drawingTexture.DrawingContext;
            drawingContext.Begin();
            drawingContext.DrawLine(10, 20, 100, 20, Microsoft.Xna.Framework.Color.Red);
            drawingContext.DrawRectangle(120, 10, 100, 20, Microsoft.Xna.Framework.Color.Blue);
            drawingContext.DrawTriangle(240, 10, 240, 60, 200, 60, Microsoft.Xna.Framework.Color.Black);
            drawingContext.DrawEllipse(310, 10, 50, 50, Microsoft.Xna.Framework.Color.Green);
            drawingContext.DrawTexture(texture, new Vector2(10, 300), Microsoft.Xna.Framework.Color.White);
            drawingContext.DrawPolyline(new Vector2[] { new Vector2(410, 10), new Vector2(440, 10), new Vector2(420, 20), new Vector2(440, 40), new Vector2(410, 60) }, Microsoft.Xna.Framework.Color.Aqua);
            drawingContext.DrawFilledRectangle(120, 110, 50, 50, Microsoft.Xna.Framework.Color.Blue);
            drawingContext.DrawFilledTriangle(240, 110, 240, 160, 200, 160, Microsoft.Xna.Framework.Color.Brown);
            drawingContext.DrawFilledEllipse(310, 110, 80, 40, Microsoft.Xna.Framework.Color.Green);
            drawingContext.DrawText(SpriteFont, "Hello World!", new Vector2(120, 300), Microsoft.Xna.Framework.Color.Black);
            drawingContext.End();
        }

        int MouseX = 0;
        int MouseY = 0;

        public int MyMouseX = 0;
        public int MyMouseY = 0;

        public Rectangle myMouseRect = new Rectangle();

        int XAdjust = 0;
        int YAdjust = 0;

        protected void Update()
        {
            MouseState mouseState = Mouse.GetState();

            MouseX = mouseState.X;
            MouseY = mouseState.Y;

            System.Drawing.Point mousep = PointToClient(new System.Drawing.Point(mouseState.X, mouseState.Y));
            MyMouseX = mousep.X;
            MyMouseY = mousep.Y;

            KeyboardState keyboardState = Keyboard.GetState();
            //if(keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            //    YAdjust = YAdjust + 1;

            //if(keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            //    YAdjust = YAdjust - 1;

            //if(keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            //    XAdjust = XAdjust + 1;

            //if(keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            //    XAdjust = XAdjust - 1;


            myMouseRect = new Rectangle(MyMouseX + XAdjust, MyMouseY + YAdjust, 20, 25);

            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            float leftX = gamePadState.ThumbSticks.Left.X;
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                leftX -= 1.0f;
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                leftX += 1.0f;

            float leftY = gamePadState.ThumbSticks.Left.Y;
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                leftY += 1.0f;
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                leftY -= 1.0f;

            float dx = leftX * 0.01f * cameraZoom;
            float dy = leftY * 0.01f * cameraZoom;

            bool zoomIn = keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Z);
            bool zoomOut = keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.X);

            cameraX += dx;
            cameraY += dy;
            if (zoomIn)
                cameraZoom /= 0.995f;
            if (zoomOut)
                cameraZoom *= 0.995f;

            viewMatrix = Matrix.CreateTranslation(-cameraX, -cameraY, 0) * Matrix.CreateScale(1.0f / cameraZoom, 1.0f / cameraZoom, 1.0f);
        }

        public void UpdateTimer()
        {
            TimeSpan elapsedTime = new TimeSpan();

            if (previousGameTimer != null)
                elapsedTime = gameTimer.TotalGameTime - previousGameTimer.TotalGameTime;

            gameTimer = new GameTime(timer.Elapsed, elapsedTime);

            previousGameTimer = gameTimer;
        }
        
        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            UpdateTimer();
            Update();

            // Clear to the default control background color.
            //Color backColor = new Color(BackColor.R, BackColor.G, BackColor.B);
            Microsoft.Xna.Framework.Color backColor = Microsoft.Xna.Framework.Color.White;

            GraphicsDevice.Clear(backColor);

            if (DrawingContext != null && SpriteFont != null)
            {
                drawingContext.Begin();

                drawingContext.DrawText(SpriteFont, "X: " + MouseX.ToString(), new Vector2(1, 10), Microsoft.Xna.Framework.Color.Black);
                drawingContext.DrawText(SpriteFont, "Y: " + MouseY.ToString(), new Vector2(1, 30), Microsoft.Xna.Framework.Color.Black);
                drawingContext.DrawText(SpriteFont, "X: " + MyMouseX.ToString(), new Vector2(1, 50), Microsoft.Xna.Framework.Color.Black);
                drawingContext.DrawText(SpriteFont, "Y: " + MyMouseY.ToString(), new Vector2(1, 70), Microsoft.Xna.Framework.Color.Black);

                drawingContext.DrawText(SpriteFont, "Top: " + NetRectangle.Top.ToString() + " Right: " + NetRectangle.Right.ToString(), new Vector2(80, 10), Microsoft.Xna.Framework.Color.Black);
                drawingContext.DrawText(SpriteFont, "Left: " + NetRectangle.Left.ToString() + " Bottom: " + NetRectangle.Bottom.ToString(), new Vector2(80, 30), Microsoft.Xna.Framework.Color.Black);

                drawingContext.DrawText(SpriteFont, "X: " + myMouseRect.X.ToString() + " Width: " + myMouseRect.Width.ToString(), new Vector2(80, 70), Microsoft.Xna.Framework.Color.Black);
                drawingContext.DrawText(SpriteFont, "Y: " + myMouseRect.Y.ToString() + " Height: " + myMouseRect.Height.ToString(), new Vector2(80, 90), Microsoft.Xna.Framework.Color.Black);

                drawingContext.DrawText(SpriteFont, "X: " + this.Bounds.X.ToString() + " Width: " + this.Bounds.Width.ToString(), new Vector2(80, 120), Microsoft.Xna.Framework.Color.Black);
                drawingContext.DrawText(SpriteFont, "Y: " + this.Bounds.Y.ToString() + " Height: " + this.Bounds.Height.ToString(), new Vector2(80, 140), Microsoft.Xna.Framework.Color.Black);

                drawingContext.DrawText(SpriteFont, "X Adjust: " + XAdjust.ToString(), new Vector2(250, 70), Microsoft.Xna.Framework.Color.Black);
                drawingContext.DrawText(SpriteFont, "Y Adjust: " + YAdjust.ToString(), new Vector2(250, 90), Microsoft.Xna.Framework.Color.Black);

                drawingContext.DrawFilledRectangle(myMouseRect, Color.HotPink);

                drawingContext.DrawLine(myMouseRect.X - 5, myMouseRect.Y, myMouseRect.X + 5, myMouseRect.Y, Color.Black);
                drawingContext.DrawLine(myMouseRect.X, myMouseRect.Y - 5, myMouseRect.X, myMouseRect.Y + 5, Color.Black);
                //var brush = new SolidBrush(System.Drawing.Color.FromArgb(255, 255, 255));
                //Microsoft.Xna.Framework.Rectangle clientRect = new Microsoft.Xna.Framework.Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
                //drawingContext.DrawFilledRectangle( clientRect, Microsoft.Xna.Framework.Color.FromNonPremultiplied(255, 255, 255, 0)
                //    );
                //// draw rect svg size

                if (DrawGrid)
                    DrawGridsAndScale();

                if (_graphicsList != null)
                    _graphicsList.Draw(DrawingContext);

                DrawNetSelection(DrawingContext);

                drawingContext.End();
                //brush.Dispose();
            }
        }       
    }
}
