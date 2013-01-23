/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       ToolImage.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

using System.Windows.Forms;
using Draw;
using Xna2dEditor;

namespace Fyri2dEditor
{
	/// <summary>
	/// Ellipse tool
	/// </summary>
	public class ToolImage : ToolRectangle
	{
		public ToolImage()
		{
            //Cursor = new Cursor(GetType(), "Bitmap.cur");
            Cursor = new Cursor(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Xna2dEditor.Xna2dSvgPaint.XnaSvg.Resources.Text.cur"));
		}

        public override void OnMouseDown(XnaToolUser drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawImage(e.X, e.Y));
        }

	}
}