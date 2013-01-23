using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fyri2dEditor;
using Microsoft.Xna.Framework;
using Draw;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using SVGLib;

namespace Xna2dEditor
{
    public class XnaToolUser : GraphicsDeviceControl
    {
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

        private IContainer components;

        protected Xna2dDrawToolType _activeTool; // active drawing tool
        protected XnaTool[] _tools; // array of tools
        protected string _mDescription = "Svg picture";
        protected Point _mOriginalSize = new Point(800, 600);
        protected XnaGraphicsList _graphicsList; // list of draw objects
        protected Point _mScale = new Point((int)1.0f, (int)1.0f);
        protected Point _mSizePicture = new Point(800, 600);

        public Boolean Dirty;

        private ContextMenuStrip _contextMenuStrip;

        private ToolStripMenuItem _bringToFrontToolStripMenuItem;
        private ToolStripMenuItem _copyToolStripMenuItem;
        private ToolStripMenuItem _cutToolStripMenuItem;
        private ToolStripMenuItem _deleteToolStripMenuItem;
        private ToolStripMenuItem _pasteToolStripMenuItem;
        private ToolStripMenuItem _selectAllToolStripMenuItem;
        private ToolStripMenuItem _sendToBackToolStripMenuItem;
        private ToolStripSeparator _toolStripSeparator1;
        private ToolStripSeparator _toolStripSeparator2;
        private ToolStripSeparator _toolStripSeparator3;

        public int MyMouseX { get; set; }

        public int MyMouseY { get; set; }

        protected XnaToolBox _toolbox;
        public XnaToolBox ToolBox
        {
            get { return _toolbox; }
            set { _toolbox = value; }
        }

        protected PropertyGrid _shapeProperties;
        public PropertyGrid ShapeProperties
        {
            get { return _shapeProperties; }
            set { _shapeProperties = value; }
        }

        internal void SetDirty()
        {
            Dirty = true;
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

        protected void XnaToolUser_MouseDown(object sender, MouseEventArgs e)
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

            //base.OnMouseDown(e);
        }

        protected void XnaToolUser_MouseMove(object sender, MouseEventArgs e)
        {
            //base.OnMouseMove(e);
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

        protected void XnaToolUser_MouseUp(object sender, MouseEventArgs e)
        {
            //base.OnMouseUp(e);

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

        private void ResetToolSelection(object sender, EventArgs e)
        {
            //Set to pointer
            if (_toolbox != null)
            {
                _toolbox.SetToolSelection(Xna2dDrawToolType.Pointer);
                if (((XnaToolUser)sender).GraphicsList.SelectionCount == 0)
                {
                    _shapeProperties.SelectedObject = null;
                }
            }
        }

        protected override void Initialize()
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
            //AutoScroll = true;
            //AutoSize = true;
            //Name = "DrawArea";

            MouseMove += XnaToolUser_MouseMove;
            MouseDown += XnaToolUser_MouseDown;
            MouseUp += XnaToolUser_MouseUp;
            ToolDone += ResetToolSelection;

            _contextMenuStrip.ResumeLayout(false);

            // set default tool
            _activeTool = Xna2dDrawToolType.Pointer;

            // create list of graphic objects
            _graphicsList = new XnaGraphicsList();

            // create array of drawing tools
            _tools = new XnaTool[(int)Xna2dDrawToolType.NumberOfDrawTools];
            _tools[(int)Xna2dDrawToolType.Pointer] = new XnaToolPointer();
            _tools[(int)Xna2dDrawToolType.Rectangle] = new ToolRectangle();
            _tools[(int)Xna2dDrawToolType.Ellipse] = new ToolEllipse();
            _tools[(int)Xna2dDrawToolType.Line] = new ToolLine();
            _tools[(int)Xna2dDrawToolType.Polygon] = new ToolPolygon();
            _tools[(int)Xna2dDrawToolType.Text] = new XnaToolText();
            _tools[(int)Xna2dDrawToolType.Bitmap] = new ToolImage();
            _tools[(int)Xna2dDrawToolType.Pan] = new ToolPan();
            _tools[(int)Xna2dDrawToolType.Path] = new XnaToolPath();

            XnaDrawObject.Dpi = new Point((int)1, (int)1);
        }

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

        protected override void Draw()
        {
            
        }
    }
}
