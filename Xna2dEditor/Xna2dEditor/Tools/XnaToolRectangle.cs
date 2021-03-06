/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       ToolRectangle.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

using System.Windows.Forms;

using Draw;
using System.Reflection;
using Microsoft.Xna.Framework;
using Xna2dEditor;

namespace Fyri2dEditor
{
	/// <summary>
	/// Rectangle tool
	/// </summary>
	public class ToolRectangle : ToolObject
	{

		public ToolRectangle()
        {
            Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("Xna2dEditor.Xna2dSvgPaint.XnaSvg.Resources.Rectangle.cur"));
		}

        public override void OnMouseDown(XnaToolUser drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new XnaDrawRectangle(e.X, e.Y, 1, 1));
        }

        public override void OnMouseMove(XnaToolUser drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;

            if (e.Button == MouseButtons.Left && drawArea.GraphicsList.Count > 0)
            {
                var point = new Point(e.X, e.Y);
                drawArea.GraphicsList[0].MoveHandleTo(point, 5);
                drawArea.Refresh();
            }
        }
	}
}