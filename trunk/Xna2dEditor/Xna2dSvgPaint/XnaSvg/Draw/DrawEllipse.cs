#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       DrawEllipse.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

#endregion Header

namespace Draw
{
    using System;
    using System.Globalization;

    using SVGLib;
    using Fyri2dEditor.Xna2dDrawingLibrary;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Xna2dEditor;

    /// <summary>
    /// Ellipse graphic object
    /// </summary>
    public sealed class DrawEllipse : XnaDrawRectangle
    {
        #region Fields

        private const string Tag = "ellipse";

        #endregion Fields

        #region Constructors

        public DrawEllipse()
        {
            SetRectangle(0, 0, 1, 1);
            Initialize();
        }

        public DrawEllipse(float x, float y, float width, float height)
        {
            Rectangle = new Rectangle((int)x, (int)y, (int)width, (int)height);
            Initialize();
        }

        #endregion Constructors

        #region Methods

        public static DrawEllipse Create(SvgEllipse svg)
        {
            try
            {
                float cx = ParseSize(svg.CX,Dpi.X);
                float cy = ParseSize(svg.CY,Dpi.Y);
                float rx = ParseSize(svg.RX,Dpi.X);
                float ry = ParseSize(svg.RY,Dpi.Y);
                var dobj = new DrawEllipse(cx-rx,cy-ry,rx*2,ry*2);
                dobj.SetStyleFromSvg(svg);
                return dobj;
            }
            catch (Exception ex)
            {
                ErrH.Log("DrawEllipse", "CreateRectangle", ex.ToString(), ErrH._LogPriority.Info);
                return null;
            }
        }

        public override void Draw(SpriteBatch g)
        {
            Rectangle r = GetNormalizedRectangle(Rectangle);
            if (Fill.ToColor() != Color.Transparent)
            {
                //Brush brush = new SolidBrush(Fill);
                XnaDrawing.DrawFilledEllipse(r, Fill.ToColor());
            }
            //var pen = new Pen(Stroke, StrokeWidth);
            XnaDrawing.DrawEllipse(r, Stroke);
            //pen.Dispose();
        }

        public override string GetXmlStr(Point scale)
        {
            string s = "<";
            s += Tag;
            s += GetStrStyle(scale);
            float cx = (Rectangle.X+Rectangle.Width/2)/scale.X;
            float cy = (Rectangle.Y+Rectangle.Height/2)/scale.Y;
            float rx = (Rectangle.Width/2)/scale.X;
            float ry = ((Rectangle.Height/2))/scale.Y;

            s += " cx = \""+cx.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " cy = \""+cy.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " rx = \""+rx.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " ry = \""+ry.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " />" + "\r\n";
            return s;
        }

        #endregion Methods
    }
}