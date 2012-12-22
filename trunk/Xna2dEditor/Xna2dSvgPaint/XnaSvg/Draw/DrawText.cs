#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       DrawText.cs
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
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Fyri2dEditor;
    using Fyri2dEditor.Xna2dDrawingLibrary;

    /// <summary>
    /// text graphic object
    /// </summary>
    public class XnaDrawText : XnaDrawRectangle
    {
        #region Fields

        public static System.Drawing.Font LastFontText = new System.Drawing.Font("Microsoft Sans Serif",12);
        public static string LastInputText = "";
        public static System.Drawing.StringFormat LastStringFormat = new System.Drawing.StringFormat();
        public System.Drawing.StringFormat TextAnchor;
        private const string Tag = "text";

        #endregion Fields

        #region Constructors

        public XnaDrawText()
        {
            Font = new System.Drawing.Font("Microsoft Sans Serif",9 * Zoom);
            Text = "";
            SetRectangle(0, 0, 1, 1);
            Initialize();
            TextAnchor = new System.Drawing.StringFormat();
        }

        public XnaDrawText(float x, float y)
        {
            Font = new System.Drawing.Font(LastFontText.FontFamily, LastFontText.Size * Zoom);
            Text = LastInputText;
            TextAnchor = new System.Drawing.StringFormat(XnaDrawText.LastStringFormat);
            Rectangle = new Rectangle((int)(x* Zoom ), (int)(y * Zoom), 0, 0);
            Initialize();
        }

        #endregion Constructors

        #region Properties

        public System.Drawing.Font Font { get; set; }

        public string Text
        {
            get; set;
        }

        public int Y
        {
            get
            {
                return Rectangle.Y;
            }
            set
            {
                Rectangle = new Rectangle(Rectangle.X,value,Rectangle.Width,Rectangle.Height);
            }
        }

        #endregion Properties

        #region Methods

        public static Rectangle CalcSize(string txt,SpriteFont fnt,float x,float y,System.Drawing.StringFormat fmt)
        {
            Vector2 vect = fnt.MeasureString(txt);
            Point rectNeed = new Point((int)vect.X, (int)vect.Y);
            var rect = new Rectangle((int)x,(int)y,rectNeed.X,rectNeed.Y);
            if (fmt.Alignment == System.Drawing.StringAlignment.Center)
                rect.X -= rect.Width/2;
            else if (fmt.Alignment == System.Drawing.StringAlignment.Far)
                rect.X -= rect.Width;
            return rect;
        }

        public static XnaDrawText Create(SvgText svg)
        {
            if (string.IsNullOrEmpty(svg.Value))
                return null;
            try
            {
                var dobj = new XnaDrawText(ParseSize(svg.X,Dpi.X),
                    ParseSize(svg.Y,Dpi.Y)) {Text = svg.Value};
                dobj.SetStyleFromSvg(svg);
                return dobj;
            }
            catch (Exception ex)
            {
                ErrH.Log("DrawText", "DrawText", ex.ToString(), ErrH._LogPriority.Info);
                return null;
            }
        }

        public static string GetXmlText(Rectangle rect,Color color,System.Drawing.Font font,string txt,Point scale,System.Drawing.StringFormat anchor)
        {
            //</text>
            Console.WriteLine(font);
            string s = "<";
            //s += Tag;
            //string sc = " style = \"fill:"+Color2String(color)+
            //    "; font-family:"+font.FontFamily.Name;
            //if (font.Bold)
            //    sc += "; font-weight:bold";
            //if (font.Italic)
            //    sc += "; font-style:italic";
            //float fs = font.Size/scale.Height;
            //sc += "; font-size:"+fs.ToString(CultureInfo.InvariantCulture)+"pt";
            //if (anchor.Alignment != System.Drawing.StringAlignment.Near)
            //{
            //    string sa = "";
            //    switch (anchor.Alignment)
            //    {
            //        case System.Drawing.StringAlignment.Center:
            //            sa = "middle";
            //            break;
            //        case StringAlignment.Far:
            //            sa = "end";
            //            break;
            //    }
            //    if (sa.Length>0)
            //        sc += "; text-anchor:"+sa;
            //}
            //sc += "\"";
            //s += sc;
            //RectangleF crect = rect;
            //if (anchor.Alignment == StringAlignment.Center)
            //{
            //    crect.X += crect.Width/2;
            //}
            //else if (anchor.Alignment == StringAlignment.Far)
            //{
            //    crect.X += crect.Width;
            //}
            //crect.Y += font.Height;
            //s += GetRectStringXml(crect,scale, "");
            //s += " >";
            //s += txt;
            //s += "</"+Tag+">";
            //s += "\r\n";
            return s;
        }

        public override void Draw(XnaDrawingContext g)
        {
            SpriteFont font = null;
            if (Rectangle.Width == 0 || Rectangle.Height == 0)
                Rectangle = CalcSize(Text,font,Rectangle.X,Rectangle.Y,TextAnchor);
            //Brush brush = new SolidBrush(Stroke);
            try
            {
                //g.DrawText(Font,Text,brush,Rectangle,TextAnchor);
            }
            catch(Exception ex)
            {
                ErrH.Log("DrawText", "Draw", ex.ToString(), ErrH._LogPriority.Info);
            }
        }

        public override string GetXmlStr(Point scale)
        {
            return GetXmlText(Rectangle,Stroke,Font,Text,scale,TextAnchor);
        }

        public override void Resize(Point newscale,Point oldscale)
        {
            base.Resize(newscale,oldscale);
            float newfw = RecalcFloat(Font.Size, newscale.X,oldscale.X);
            Font = new System.Drawing.Font(Font.FontFamily.Name,newfw,Font.Style);
        }

        [CLSCompliant(false)]
        public bool SetStyleFromSvg(SvgText svg)
        {
            try
            {
                float x = ParseSize(svg.X,Dpi.X);
                float y = ParseSize(svg.Y,Dpi.Y);
                float w = ParseSize(svg.Width,Dpi.X);
                float h = ParseSize(svg.Height,Dpi.Y);
                Text = svg.Value;
                //font
                Stroke = Color.FromNonPremultiplied(svg.Fill.R, svg.Fill.G, svg.Fill.B, svg.Fill.A);
                string family = svg.FontFamily;
                float size = ParseSize(svg.FontSize,Dpi.X);
                int fs = 0;
                if (svg.FontWeight.IndexOf("bold")>=0)
                    fs = 1;
                if (svg.FontStyle.IndexOf("italic")>=0)
                    fs = fs|2;
                Font = new System.Drawing.Font(family,size,(System.Drawing.FontStyle )fs);
                //				y -= font.Size;
                y -= Font.Height;
                Rectangle = new Rectangle((int)x,(int)y,(int)w,(int)h);
                if (svg.TextAnchor.Length > 0)
                {
                    switch (svg.TextAnchor)
                    {
                        case "start":
                            TextAnchor.Alignment = System.Drawing.StringAlignment.Near;
                            break;
                        case "end":
                            TextAnchor.Alignment = System.Drawing.StringAlignment.Far;
                            Rectangle = new Rectangle((int)(x-w),(int)y,(int)w,(int)h);
                            break;
                        case "middle":
                            TextAnchor.Alignment = System.Drawing.StringAlignment.Center;
                            Rectangle = new Rectangle((int)(x-w/2),(int)y,(int)w,(int)h);
                            break;
                    }
                }
                return true;
            }
            catch
            {
                ErrH.Log("DrawText", "SetStyleFromSvg", "SetStyleFromSvg", ErrH._LogPriority.Info);
                return false;
            }
        }

        #endregion Methods
    }
}