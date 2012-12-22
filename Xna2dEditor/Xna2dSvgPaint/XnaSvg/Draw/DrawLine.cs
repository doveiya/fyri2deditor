#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       DrawLine.cs
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
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.Windows.Forms;
    using System.Xml;

    using SVGLib;
    using Microsoft.Xna.Framework;
    using Fyri2dEditor.Xna2dDrawingLibrary;

    /// <summary>
    /// Line graphic object
    /// </summary>
    public class XnaDrawLine : XnaDrawObject
    {
        #region Fields

        private const string Tag = "line";

        private Point _endPoint;
        private Point _startPoint;

        #endregion Fields

        #region Constructors

        public XnaDrawLine()
        {
            _startPoint.X = 0;
            _startPoint.Y = 0;
            _endPoint.X = 1;
            _endPoint.Y = 1;

            Initialize();
        }

        public XnaDrawLine(float x1, float y1, float x2, float y2)
        {
            _startPoint.X = (int)x1;
            _startPoint.Y = (int)y1;
            _endPoint.X = (int)x2;
            _endPoint.Y = (int)y2;

            Initialize();
        }

        #endregion Constructors

        #region Properties

        public override int HandleCount
        {
            get
            {
                return 2;
            }
        }

        protected GraphicsPath AreaPath
        {
            get; set;
        }

        protected System.Drawing.Pen AreaPen
        {
            get; set;
        }

        protected System.Drawing.Region AreaRegion
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        public static XnaDrawLine Create(SvgLine svg)
        {
            try
            {
                var dobj = new XnaDrawLine(ParseSize(svg.X1,Dpi.X),
                    ParseSize(svg.Y1,Dpi.Y),
                    ParseSize(svg.X2,Dpi.X),
                    ParseSize(svg.Y2,Dpi.Y));
                dobj.SetStyleFromSvg(svg);
                return dobj;
            }
            catch (Exception ex)
            {
                ErrH.Log("CreateLine", "Draw", ex.ToString(), ErrH._LogPriority.Info);
                return null;
            }
        }

        public override void Draw(XnaDrawingContext g)
        {
            try
            {
                //g.SmoothingMode = SmoothingMode.AntiAlias;
                //var pen = new Pen(Stroke, StrokeWidth);
                //TODO change DrawLine to include thickness
                g.DrawLine(_startPoint.X, _startPoint.Y, _endPoint.X, _endPoint.Y, Stroke);
                //pen.Dispose();
            }
            catch (Exception ex)
            {
                ErrH.Log("DrawLine", "Draw", ex.ToString(), ErrH._LogPriority.Info);
            }
        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Point GetHandle(int handleNumber)
        {
            if ( handleNumber == 1 )
                return _startPoint;

            return _endPoint;
        }

        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch ( handleNumber )
            {
                case 1:
                case 2:
                    return Cursors.SizeAll;
                default:
                    return Cursors.Default;
            }
        }

        public override string GetXmlStr(Point scale)
        {
            string s = "<";
            s += Tag;
            s += GetStrStyle(scale);
            float x1 = _startPoint.X/scale.X;
            float y1 = _startPoint.Y/scale.Y;
            float x2 = _endPoint.X/scale.X;
            float y2 = _endPoint.Y/scale.Y;

            s += " x1 = \""+x1.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " y1 = \""+y1.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " x2 = \""+x2.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " y2 = \""+y2.ToString(CultureInfo.InvariantCulture)+"\"";
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

            if (PointInObject(point))
                return 0;

            return -1;
        }

        public override bool IntersectsWith(Rectangle rectangle)
        {
            CreateObjects();
            System.Drawing.Rectangle rectToCheck = new System.Drawing.Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

            return AreaRegion.IsVisible(rectToCheck);
        }

        public override void Move(float deltaX, float deltaY)
        {
            _startPoint.X += (int)deltaX;
            _startPoint.Y += (int)deltaY;

            _endPoint.X += (int)deltaX;
            _endPoint.Y += (int)deltaY;

            Invalidate();
        }

        public override void MoveHandleTo(Point point, int handleNumber)
        {
            if ( handleNumber == 1 )
                _startPoint = point;
            else
                _endPoint = point;

            Invalidate();
        }

        public override void Resize(Point newscale,Point oldscale)
        {
            _startPoint = RecalcPoint(_startPoint, newscale,oldscale);
            _endPoint = RecalcPoint(_endPoint, newscale,oldscale);
            RecalcStrokeWidth(newscale,oldscale);
            Invalidate();
        }

        public override void SaveToXml(XmlTextWriter writer,double scale)
        {
            writer.WriteStartElement("line");
            string s = "stroke-width:" + StrokeWidth;
            writer.WriteAttributeString("style",s);
            writer.WriteAttributeString("x1",_startPoint.X.ToString());
            writer.WriteAttributeString("y1",_startPoint.Y.ToString());
            writer.WriteAttributeString("x2",_endPoint.X.ToString());
            writer.WriteAttributeString("y2",_endPoint.Y.ToString());
            writer.WriteEndElement();
        }

        /// <summary>
        /// Create graphic objects used from hit test.
        /// </summary>
        protected virtual void CreateObjects()
        {
            if ( AreaPath != null )
                return;

            // Create path which contains wide line
            // for easy mouse selection
            AreaPath = new GraphicsPath();
            //AreaPen = new Pen(Color.Black, 2);
            AreaPath.AddLine(_startPoint.X, _startPoint.Y, _endPoint.X, _endPoint.Y);
            AreaPath.Widen(AreaPen);

            // Create region from the path
            //AreaRegion = new Region(AreaPath);
        }

        /// <summary>
        /// Invalidate object.
        /// When object is invalidated, path used for hit test
        /// is released and should be created again.
        /// </summary>
        protected void Invalidate()
        {
            if ( AreaPath != null )
            {
                AreaPath.Dispose();
                AreaPath = null;
            }

            if ( AreaPen != null )
            {
                AreaPen.Dispose();
                AreaPen = null;
            }

            if ( AreaRegion != null )
            {
                AreaRegion.Dispose();
                AreaRegion = null;
            }
        }

        protected override bool PointInObject(Point point)
        {
            CreateObjects();

            System.Drawing.Point pointToCheck = new System.Drawing.Point(point.X, point.Y);

            if (AreaRegion != null)
                return AreaRegion.IsVisible(pointToCheck);
            else
                return false;
        }

        #endregion Methods
    }
}