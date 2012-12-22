#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       WorkSpace.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

#endregion Header

namespace Xna2dEditor.UserControls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml;
    using Fyri2dEditor;

    public partial class WorkSpace : UserControl
    {
        public XnaItems XnaItems { get; set; }

        #region Constructors

        public WorkSpace(XnaItems xnaItems)
        {
            InitializeComponent();
            drawArea.ToolDone += ResetToolSelection;
            drawArea.MousePan += DrawAreaMousePan;
            drawArea.ItemsSelected += DrawAreaItemsSelected;
            //drawArea.DrawingContext = XnaItems.DrawingContext;
            drawArea.Initialize(xnaItems, this);
            ResizeDrawArea();
        }

        #endregion Constructors

        #region Delegates

        public delegate void OnGridOptionChanged(object sender, EventArgs e);

        public delegate void OnItemsInSelection(object sender, MouseEventArgs e);

        public delegate void OnMousePan(object sender, MouseEventArgs e);

        public delegate void OnScrollMade(object sender, ScrollEventArgs e);

        public delegate void OnToolDone(object sender, EventArgs e);

        public delegate void OnZoomDone(object sender, EventArgs e);

        #endregion Delegates

        #region Events

        public event OnGridOptionChanged GridChange;

        public event OnItemsInSelection ItemsSelected;

        public event OnMousePan MousePan;

        public event OnScrollMade ScrollMade;

        public event WorkArea.OnToolDone ToolDone;

        public event OnZoomDone ZoomDone;

        #endregion Events

        #region Methods

        public void BringShapelToFront()
        {
            drawArea.GraphicsList.MoveSelectionToFront();
            drawArea.Refresh();
        }

        public bool CheckDirty()
        {
            return drawArea.Dirty;
        }

        public void Copy()
        {
            drawArea.GraphicsList.CopySelection();
            drawArea.Refresh();
        }

        public void Cut()
        {
            drawArea.GraphicsList.CutSelection();
            drawArea.Refresh();
        }

        public void Delete()
        {
            drawArea.GraphicsList.DeleteSelection();
            drawArea.Refresh();
        }

        public Fyri2dEditor.Xna2dDrawArea.Xna2dDrawToolType GetCurrentTool()
        {
            return drawArea.ActiveTool;
        }

        public float GetCurrentZoom()
        {
            return drawArea.ScaleDraw.X;
        }

        public bool GetGridOption()
        {
            return drawArea.DrawGrid;
        }

        public int GetMinorGrids()
        {
            return (int)drawArea.Xdivs;
        }

        public String GetSvgDescription()
        {
            return drawArea.Description;
        }

        public Size GetWorkAreaSize()
        {
            return new Size(drawArea.Size.Width -1, drawArea.Size.Height -1);
        }

        public void GridOptionChanged(object sender, EventArgs e)
        {
            if (GridChange != null)
                GridChange(sender, e);

            if (sender.GetType().ToString() == "System.Windows.Forms.CheckBox")
            {
                var option = (CheckBox)sender;
                drawArea.DrawGrid = option.Checked;
            }
            else if (sender.GetType().ToString() == "System.Windows.Forms.NumericUpDown")
            {
                drawArea.Xdivs = (float)(((NumericUpDown)sender).Value);
                drawArea.Ydivs = (float)(((NumericUpDown)sender).Value);
            }

            drawArea.Refresh();
        }

        public void OpenFile(String fileName)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(fileName);
                drawArea.LoadFromXml(reader);
                drawArea.FileName = fileName;
                ResizeDrawArea();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public void Paste()
        {
            drawArea.GraphicsList.PasteSelection();
            drawArea.Refresh();
        }

        public void PropertyChanged(GridItem itemChanged, object oldVal)
        {
            drawArea.PropertyChanged(itemChanged, oldVal);
            drawArea.Refresh();
        }

        public void Redo()
        {
            drawArea.GraphicsList.Redo();
            drawArea.Refresh();
        }

        public void ResizeDrawArea()
        {
            drawArea.Top = ClientRectangle.Top - VerticalScroll.Value;
            drawArea.Left = ClientRectangle.Left - HorizontalScroll.Value;
            drawArea.Width = (int)drawArea.SizePicture.X + 1;
            drawArea.Height = (int)drawArea.SizePicture.Y + 1;
        }

        public void RestoreScroll()
        {
            HorizontalScroll.Value = _scrollPersistance.X;
            VerticalScroll.Value = _scrollPersistance.Y;
        }

        public void SaveFile(String fileName)
        {
            System.IO.StreamWriter writer = null;

            if (String.IsNullOrEmpty(fileName))
            {
                //I've opened this file. So i know the file name
                if (!(String.IsNullOrEmpty(drawArea.FileName)))
                {
                    fileName = drawArea.FileName;
                }
                else
                {
                    var dlgSaveFileDialog = new SaveFileDialog();
                    dlgSaveFileDialog.Filter = @"SVG files (*.svg)|*.svg|All files (*.*)|*.*";

                    if (dlgSaveFileDialog.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }
                    fileName = dlgSaveFileDialog.FileName;
                }
            }

            try
            {
                writer = new System.IO.StreamWriter(fileName);
                drawArea.SaveToXml(writer);
                MessageBox.Show(@"Save Done");
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }

        public void SelectAll()
        {
            drawArea.GraphicsList.SelectAll();
            drawArea.Refresh();
        }

        public void SendShapeToBack()
        {
            drawArea.GraphicsList.MoveSelectionToBack();
            drawArea.Refresh();
        }

        public void SetDrawAreaProperties(Size size, String desc)
        {
            drawArea.Description = desc;
            drawArea.Size = size;
            drawArea.SizePicture = new Microsoft.Xna.Framework.Point(size.Width, size.Height);
            drawArea.Refresh();
        }

        public void SetGridDivs(float x, float y)
        {
            drawArea.Xdivs = x;
            drawArea.Ydivs = y;
            drawArea.Refresh();
        }

        public void SetTool(String tool)
        {
            switch (tool)
            {
                case "Pointer":
                    drawArea.ActiveTool = Fyri2dEditor.Xna2dDrawArea.Xna2dDrawToolType.Pointer;
                    break;

                case "Rectangle":
                    drawArea.ActiveTool = Fyri2dEditor.Xna2dDrawArea.Xna2dDrawToolType.Rectangle;
                    break;

                case "Ellipse":
                    drawArea.ActiveTool = Fyri2dEditor.Xna2dDrawArea.Xna2dDrawToolType.Ellipse;
                    break;

                case "Line":
                    drawArea.ActiveTool = Fyri2dEditor.Xna2dDrawArea.Xna2dDrawToolType.Line;
                    break;

                case "Pan":
                    drawArea.ActiveTool = Fyri2dEditor.Xna2dDrawArea.Xna2dDrawToolType.Pan;
                    break;

                case "Pencil":
                    drawArea.ActiveTool = Fyri2dEditor.Xna2dDrawArea.Xna2dDrawToolType.Polygon;
                    break;

                case "Text":
                    drawArea.ActiveTool = Fyri2dEditor.Xna2dDrawArea.Xna2dDrawToolType.Text;
                    break;

                case "Path":
                    drawArea.ActiveTool = Fyri2dEditor.Xna2dDrawArea.Xna2dDrawToolType.Path;
                    break;

                case "Image":
                    drawArea.ActiveTool = Fyri2dEditor.Xna2dDrawArea.Xna2dDrawToolType.Bitmap;
                    break;

                default:
                    drawArea.ActiveTool = Fyri2dEditor.Xna2dDrawArea.Xna2dDrawToolType.Pointer;
                    break;
            }
        }

        public void SetZoom(float zoom)
        {
            drawArea.DoScaling(new Microsoft.Xna.Framework.Point((int)zoom, (int)zoom));
            ResizeDrawArea();
            drawArea.Refresh();
            ZoomDone((object)zoom, new EventArgs());
        }

        public void Undo()
        {
            drawArea.GraphicsList.Undo();
            drawArea.Refresh();
        }

        public void UnSelectAll()
        {
            drawArea.GraphicsList.UnselectAll();
            drawArea.Refresh();
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            _scrollPersistance.X = HorizontalScroll.Value;
            _scrollPersistance.Y = VerticalScroll.Value;

               if (ScrollMade != null)
                ScrollMade(this, se);
        }

        protected override Point ScrollToControl(Control activeControl)
        {
            return DisplayRectangle.Location;
        }

        void DrawAreaItemsSelected(object sender, MouseEventArgs e)
        {
            if(ItemsSelected != null)
                ItemsSelected(sender, e);
        }

        private void DrawAreaMousePan(object sender, MouseEventArgs e)
        {
            if (MousePan != null)
                MousePan(sender, e);
        }

        private void ResetToolSelection(object sender, EventArgs e)
        {
            if(ToolDone!= null)
                ToolDone(sender, e);
        }

        #endregion Methods
    }
}