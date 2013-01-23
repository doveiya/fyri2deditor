#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       ToolText.cs
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
    /// Ellipse tool
    /// </summary>
    public class XnaToolText : ToolRectangle
    {
        #region Constructors

        public XnaToolText()
        {
            //Cursor = new Cursor(GetType(), "Text.cur");
            Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("Xna2dEditor.Xna2dSvgPaint.XnaSvg.Resources.Text.cur"));
            MinSize = new System.Drawing.Size(40, 20);
        }

        #endregion Constructors

        #region Methods

        public override void OnMouseDown(XnaToolUser drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new XnaDrawText(e.X, e.Y));
        }

        protected override void adjustForMinimumSize(XnaToolUser drawArea)
        {
            var objectAdded = (XnaDrawText)drawArea.GraphicsList[0];
            Rectangle rect;

            rect = objectAdded.Rect;

            if (MinSize.Width > 0)
            {
                if (objectAdded.Rect.Width < MinSize.Width)
                {
                    rect.Width = (int)(MinSize.Width * XnaDrawObject.Zoom);
                }
            }
            if (MinSize.Height > 0)
            {
                if (objectAdded.Rect.Height < MinSize.Height)
                {
                    rect.Height = (int)(MinSize.Height * XnaDrawObject.Zoom);
                }
            }

            objectAdded.Rect = rect;
        }

        #endregion Methods
    }
}