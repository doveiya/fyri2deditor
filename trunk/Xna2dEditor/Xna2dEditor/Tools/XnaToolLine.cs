#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       ToolLine.cs
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
    using Xna2dEditor;

    /// <summary>
    /// Line tool
    /// </summary>
    public class ToolLine : ToolObject
    {
        #region Constructors

        public ToolLine()
        {
            //Cursor = new Cursor(GetType(), "Line.cur");
            Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("Xna2dEditor.Xna2dSvgPaint.XnaSvg.Resources.Line.cur"));
        }

        #endregion Constructors

        #region Methods

        public override void OnMouseDown(XnaToolUser drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new XnaDrawLine(e.X, e.Y, e.X + 1, e.Y + 1));
            IsComplete = true;
        }

        public override void OnMouseMove(XnaToolUser drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;
            if ( e.Button == MouseButtons.Left )
            {
                var point = new Point(e.X, e.Y);
                drawArea.GraphicsList[0].MoveHandleTo(point, 2);
                drawArea.Refresh();
            }
        }

        #endregion Methods
    }
}