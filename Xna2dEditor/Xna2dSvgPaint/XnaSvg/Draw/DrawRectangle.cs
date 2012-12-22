#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       DrawRectangle.cs
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
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Forms;

    using SVGLib;
    using Microsoft.Xna.Framework;
    using Fyri2dEditor.Xna2dDrawingLibrary;

    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    public class XnaDrawRectangle : XnaDrawObject
    {
        #region Fields

        private const string Tag = "rect";

        private Rectangle rectangle;

        #endregion Fields

        #region Constructors

        public XnaDrawRectangle()
        {
            SetRectangle(0, 0, 1,1);
            Initialize();
        }

        public XnaDrawRectangle(float x, float y, float width, float height)
        {
            rectangle.X = (int)x;
            rectangle.Y = (int)y;
            rectangle.Width = (int)width;
            rectangle.Height = (int)height;
            Initialize();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Get number of handles
        /// </summary>
        public override int HandleCount
        {
            get
            {
                return 8;
            }
        }

        public Rectangle Rect
        {
            get
            {
                var rect= new Rectangle();
                rect.X = (int)(rectangle.X / Zoom) ;
                rect.Y = (int)(rectangle.Y / Zoom);
                rect.Width = (int)(rectangle.Width / Zoom);
                rect.Height = (int)(rectangle.Height / Zoom);
                return rect;
            }
            set
            {
                rectangle = value;
            }
        }

        protected int Height
        {
            get
            {
                return rectangle.Height;
            }
            set
            {
                rectangle.Height = value;
            }
        }

        protected Rectangle Rectangle
        {
            get
            {
                return rectangle;
            }
            set
            {
                rectangle = value;
            }
        }

        protected int Width
        {
            get
            {
                return rectangle.Width;
            }
            set
            {
                rectangle.Width = value;
            }
        }

        #endregion Properties

        #region Methods

        public static XnaDrawRectangle Create(SvgRect svg)
        {
            try
            {
                var dobj = new XnaDrawRectangle(ParseSize(svg.X,Dpi.X),
                    ParseSize(svg.Y,Dpi.Y),
                    ParseSize(svg.Width,Dpi.X),
                    ParseSize(svg.Height,Dpi.Y));
                dobj.SetStyleFromSvg(svg);
                dobj.Name = svg.ShapeName;

                return dobj;
            }
            catch (Exception ex)
            {
                ErrH.Log("DrawRectangle", "CreateRectangle:", ex.ToString(), ErrH._LogPriority.Info);
                return null;
            }
        }

        public static Rectangle GetNormalizedRectangle(float x1, float y1, float x2, float y2)
        {
            if ( x2 < x1 )
            {
                float tmp = x2;
                x2 = x1;
                x1 = tmp;
            }

            if ( y2 < y1 )
            {
                float tmp = y2;
                y2 = y1;
                y1 = tmp;
            }

            return new Rectangle((int)x1, (int)y1, (int)(x2 - x1), (int)(y2 - y1));
        }

        public static Rectangle GetNormalizedRectangle(Point p1, Point p2)
        {
            return GetNormalizedRectangle(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static Rectangle GetNormalizedRectangle(Rectangle r)
        {
            return GetNormalizedRectangle(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
        }

        public static string GetRectangleXmlStr(Color stroke,bool isFill,Color fill,float strokewidth,Rectangle rect,Point scale, String shapeName)
        {
            string s = "<";
            s += Tag;
            s += GetStringStyle(stroke,fill,strokewidth,scale);//GetStrStyle(scale);
            s += GetRectStringXml(rect,scale, shapeName);
            s += " />" + "\r\n";
            return s;
        }

        public static string GetRectStringXml(Rectangle rect,  Point scale, String shapeName)
        {
            string s = "";
            float x = rect.X/scale.X;
            float y = rect.Y/scale.Y;
            float w = rect.Width/scale.X;
            float h = rect.Height/scale.Y;

            s += " x = \""+x.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " y = \""+y.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " width = \""+w.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " height = \""+h.ToString(CultureInfo.InvariantCulture)+"\"";
            //Added by Ajay
            s += " ShapeName = \"" + shapeName + "\"";
            return s;
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(XnaDrawingContext g)
        {

            try
            {
                Rectangle r = GetNormalizedRectangle(Rectangle);
                if (Fill != Color.Transparent)
                {
                    //Brush brush = new SolidBrush(Fill);
                    g.DrawFilledRectangle(r, Fill);
                }
                //Pen pen = new Pen(Stroke, StrokeWidth);
                //TODO change Draw Rectangle to include strokewidth
                g.DrawRectangle(r.X,r.Y,r.Width,r.Height, Stroke);

                //pen.Dispose();
            }
            catch (Exception ex)
            {
                ErrH.Log("DrawRectangle", "Draw", ex.ToString(), ErrH._LogPriority.Info);
            }
        }

        public override void Dump()
        {
            base.Dump ();

            Trace.WriteLine("rectangle.X = " + rectangle.X.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Y = " + rectangle.Y.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Width = " + rectangle.Width.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Height = " + rectangle.Height.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Point GetHandle(int handleNumber)
        {
            float x, xCenter, yCenter;

            xCenter = rectangle.X + rectangle.Width/2;
            yCenter = rectangle.Y + rectangle.Height/2;
            x = rectangle.X;
            float y = rectangle.Y;

            switch ( handleNumber )
            {
                case 1:
                    x = rectangle.X;
                    y = rectangle.Y;
                    break;
                case 2:
                    x = xCenter;
                    y = rectangle.Y;
                    break;
                case 3:
                    x = rectangle.Right;
                    y = rectangle.Y;
                    break;
                case 4:
                    x = rectangle.Right;
                    y = yCenter;
                    break;
                case 5:
                    x = rectangle.Right;
                    y = rectangle.Bottom;
                    break;
                case 6:
                    x = xCenter;
                    y = rectangle.Bottom;
                    break;
                case 7:
                    x = rectangle.X;
                    y = rectangle.Bottom;
                    break;
                case 8:
                    x = rectangle.X;
                    y = yCenter;
                    break;
            }
            return new Point((int)x, (int)y);
        }

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch ( handleNumber )
            {
                case 1:
                    return Cursors.SizeNWSE;
                case 2:
                    return Cursors.SizeNS;
                case 3:
                    return Cursors.SizeNESW;
                case 4:
                    return Cursors.SizeWE;
                case 5:
                    return Cursors.SizeNWSE;
                case 6:
                    return Cursors.SizeNS;
                case 7:
                    return Cursors.SizeNESW;
                case 8:
                    return Cursors.SizeWE;
                default:
                    return Cursors.Default;
            }
        }

        public override string GetXmlStr(Point scale)
        {
            //  <rect x="1" y="1" width="1198" height="398"
            //		style="fill:none; stroke:blue"/>

            string s = "<";
            s += Tag;
            s += GetStrStyle(scale);
            s += GetRectStringXml(Rectangle,scale, Name);
            s += " />" + "\r\n";
            return s;
        }

        /// <summary>
        /// Hit test.
        /// Return value: -1 - no hit
        ///                0 - hit anywhere
        ///                > 1 - handle number
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override int HitTest(Point point)
        {
            if ( Selected )
            {
                for ( int i = 1; i <= HandleCount; i++ )
                {
                    if ( GetHandleRectangle(i).Contains(point) )
                        return i;
                }
            }

            if ( PointInObject(point) )
                return 0;

            return -1;
        }

        public override bool IntersectsWith(Rectangle rect)
        {
            try
            {
                return Rectangle.Intersects(rect);
            }
            catch(Exception ex)
            {
                ErrH.Log("DrawRectangle", "Intersect", ex.ToString(), ErrH._LogPriority.Info);
                return false;
            }
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public override void Move(float deltaX, float deltaY)
        {
            rectangle.X += (int)deltaX;
            rectangle.Y += (int)deltaY;
        }

        /// <summary>
        /// Move handle to new point (resizing)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber"></param>
        public override void MoveHandleTo(Point point, int handleNumber)
        {
            float left = Rectangle.Left;
            float top = Rectangle.Top;
            float right = Rectangle.Right;
            float bottom = Rectangle.Bottom;

            switch ( handleNumber )
            {
                case 1:
                    left = point.X;
                    top = point.Y;
                    break;
                case 2:
                    top = point.Y;
                    break;
                case 3:
                    right = point.X;
                    top = point.Y;
                    break;
                case 4:
                    right = point.X;
                    break;
                case 5:
                    right = point.X;
                    bottom = point.Y;
                    break;
                case 6:
                    bottom = point.Y;
                    break;
                case 7:
                    left = point.X;
                    bottom = point.Y;
                    break;
                case 8:
                    left = point.X;
                    break;
            }

            SetRectangle(left, top, right - left, bottom - top);
        }

        /// <summary>
        /// Normalize rectangle
        /// </summary>
        public override void Normalize()
        {
            rectangle = GetNormalizedRectangle(rectangle);
        }

        public override void Resize(Point newscale,Point oldscale)
        {
            Point p = RecalcPoint(Rectangle.Location, newscale,oldscale);
            var ps = new Point(Rectangle.Width, Rectangle.Height);
            ps = RecalcPoint(ps,newscale,oldscale);
            Rectangle = new Rectangle(p.X,p.Y,ps.X,ps.Y);
            RecalcStrokeWidth(newscale,oldscale);
        }

        protected override bool PointInObject(Point point)
        {
            return rectangle.Contains(point);
        }

        protected void SetRectangle(float x, float y, float width, float height)
        {
            rectangle.X = (int)x;
            rectangle.Y = (int)y;
            rectangle.Width = (int)width;
            rectangle.Height = (int)height;
        }

        #endregion Methods
    }
}