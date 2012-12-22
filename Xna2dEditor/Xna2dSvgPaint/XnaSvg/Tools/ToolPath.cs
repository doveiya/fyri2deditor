#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       ToolPath.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

#endregion Header

namespace Fyri2dEditor
{
    using System.Reflection;
    using System.Windows.Forms;

    using Draw;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Polygon tool
    /// </summary>
    public class XnaToolPath : ToolObject
    {
        #region Fields

        private XnaDrawPath _newPath;
        bool _startPathDraw = true;

        #endregion Fields

        #region Constructors

        public XnaToolPath()
        {
            //Cursor = new Cursor(GetType(), "Pencil.cur");
            Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("Xna2dEditor.Xna2dSvgPaint.XnaSvg.Resources.Pencil.cur"));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Left nouse button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(Xna2dDrawArea drawArea, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ToolActionCompleted();
                return;
            }

            // Create new polygon, add it to the list
            // and keep reference to it
            if (_startPathDraw)
            {
                _newPath = new XnaDrawPath(e.X, e.Y);
                AddNewObject(drawArea, _newPath);
                _startPathDraw = false;
                IsComplete = false;
            }
            else
            {
                Point loc = new Point(e.Location.X, e.Location.Y);
                _newPath.AddPoint(loc);
            }
        }

        /// <summary>
        /// Mouse move - resize new polygon
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(Xna2dDrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;
            if (e.Button == MouseButtons.Left)
            {
                var point = new Point(e.X, e.Y);
                _newPath.MoveHandleTo(point, _newPath.HandleCount);
                drawArea.Refresh();
            }
        }

        public override void OnMouseUp(Xna2dDrawArea drawArea, MouseEventArgs e)
        {
        }

        public override void ToolActionCompleted()
        {
            if(_newPath != null)
            _newPath.CloseFigure();
            _startPathDraw = true;
            IsComplete = true;
            _newPath = null;
        }

        #endregion Methods
    }
}