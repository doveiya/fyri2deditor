#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       DrawPolygon.cs
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
    using System.Collections;
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.Windows.Forms;

    using SVGLib;
    using Microsoft.Xna.Framework;
    using Fyri2dEditor.Xna2dDrawingLibrary;

    /// <summary>
    /// Polygon graphic object
    /// </summary>
    public sealed class XnaDrawPolygon : XnaDrawLine
    {
        #region Fields

        private const string Tag = "polyline";
        private readonly ArrayList _pointArray; // list of points
        private Cursor _handleCursor;

        #endregion Fields

        #region Constructors

        public XnaDrawPolygon()
        {
            _pointArray = new ArrayList();

            LoadCursor();
            Initialize();
        }

        public XnaDrawPolygon(float x1, float y1, float x2, float y2)
        {
            _pointArray = new ArrayList {new Point((int)x1, (int)y1), new Point((int)x2, (int)y2)};

            LoadCursor();
            Initialize();
        }

        public XnaDrawPolygon(Point[] points)
        {
            _pointArray = new ArrayList();
            for (int i = 0;i<points.Length;i++)
            {
                _pointArray.Add(points[i]);
            }
            LoadCursor();
            Initialize();
        }

        #endregion Constructors

        #region Properties

        public override int HandleCount
        {
            get
            {
                return _pointArray.Count;
            }
        }

        #endregion Properties

        #region Methods

        public static XnaDrawPolygon Create(SvgPolyline svg)
        {
            try
            {
                string s = svg.Points.Trim();
                string[] arr = s.Split(' ');
                var points = new Point[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    var arrp = arr[i].Split(',');
                    points[i]=new Point((int)ParseSize(arrp[0],Dpi.X),
                        (int)ParseSize(arrp[1],Dpi.Y));
                }
                var dobj = new XnaDrawPolygon(points);
                dobj.SetStyleFromSvg(svg);
                return dobj;
            }
            catch (Exception ex)
            {
                ErrH.Log("DrawPolygon", "Create", ex.ToString(), ErrH._LogPriority.Info);
                return null;
            }
        }

        public void AddPoint(Point point)
        {
            _pointArray.Add(point);
        }

        public override void Draw(XnaDrawingContext g)
        {
            float x1 = 0, y1 = 0;     // previous point
            try
            {
                //g.SmoothingMode = SmoothingMode.AntiAlias;

                if (Fill != Color.Transparent)
                {
                    var arr = new Vector2[_pointArray.Count];
                    for (int i = 0; i < _pointArray.Count; i++)
                        arr[i] = (Vector2)_pointArray[i];
                    //Brush brush = new SolidBrush(Fill);
                    g.DrawPolyline(arr, Fill);
                }

                //var pen = new Pen(Stroke, StrokeWidth);

                IEnumerator enumerator = _pointArray.GetEnumerator();

                if ( enumerator.MoveNext() )
                {
                    x1 = ((Point)enumerator.Current).X;
                    y1 = ((Point)enumerator.Current).Y;
                }

                while ( enumerator.MoveNext() )
                {
                    float x2 = ((Point)enumerator.Current).X;             // current point
                    float y2 = ((Point)enumerator.Current).Y;             // current point

                    g.DrawLine(x1, y1, x2, y2, Stroke);

                    x1 = x2;
                    y1 = y2;
                }

                //pen.Dispose();
            }
            catch (Exception ex)
            {
                ErrH.Log("DrawPolygon", "Draw", ex.ToString(), ErrH._LogPriority.Info);
            }
        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Point GetHandle(int handleNumber)
        {
            if ( handleNumber < 1 )
                handleNumber = 1;

            if ( handleNumber > _pointArray.Count )
                handleNumber = _pointArray.Count;

            return ((Point)_pointArray[handleNumber - 1]);
        }

        public override Cursor GetHandleCursor(int handleNumber)
        {
            return _handleCursor;
        }

        public override string GetXmlStr(Point scale)
        {
            //  <polyline style="fill:none; stroke:blue; stroke-width:10"
            //points="50,375 150,375
            string s = "<";
            s += Tag;
            s += GetStrStyle(scale);
            s += " points = "+"\"";
            IEnumerator enumerator = _pointArray.GetEnumerator();
            while ( enumerator.MoveNext() )
            {
                float x = ((Point)enumerator.Current).X/scale.X;
                float y = ((Point)enumerator.Current).Y/scale.Y;
                s += x.ToString(CultureInfo.InvariantCulture)+","+y.ToString(CultureInfo.InvariantCulture);
                s += " ";
            }
            s += "\"";
            s += " />" + "\r\n";
            return s;
        }

        public override void Move(float deltaX, float deltaY)
        {
            int n = _pointArray.Count;

            for ( int i = 0; i < n; i++ )
            {
                var point = new Point((int)(((Point)_pointArray[i]).X + deltaX), (int)(((Point)_pointArray[i]).Y + deltaY));

                _pointArray[i] = point;
            }

            Invalidate();
        }

        public override void MoveHandleTo(Point point, int handleNumber)
        {
            if ( handleNumber < 1 )
                handleNumber = 1;

            if ( handleNumber > _pointArray.Count)
                handleNumber = _pointArray.Count;

            _pointArray[handleNumber-1] = point;

            Invalidate();
        }

        public override void Resize(Point newscale,Point oldscale)
        {
            for (int i = 0; i < _pointArray.Count; i ++)
                _pointArray[i] = RecalcPoint((Point)_pointArray[i], newscale,oldscale);
            RecalcStrokeWidth(newscale,oldscale);
            Invalidate();
        }

        /// <summary>
        /// Create graphic object used for hit test
        /// </summary>
        protected override void CreateObjects()
        {
            if ( AreaPath != null )
                return;
            try
            {
                // Create closed path which contains all polygon vertexes
                AreaPath = new GraphicsPath();

                float x1 = 0, y1 = 0;     // previous point

                IEnumerator enumerator = _pointArray.GetEnumerator();

                if ( enumerator.MoveNext() )
                {
                    x1 = ((Point)enumerator.Current).X;
                    y1 = ((Point)enumerator.Current).Y;
                }

                while ( enumerator.MoveNext() )
                {
                    float x2 = ((Point)enumerator.Current).X;             // current point
                    float y2 = ((Point)enumerator.Current).Y;             // current point

                    AreaPath.AddLine(x1, y1, x2, y2);

                    x1 = x2;
                    y1 = y2;
                }

                AreaPath.CloseFigure();

                // Create region from the path
                AreaRegion = new System.Drawing.Region(AreaPath);
            }
            catch(Exception ex)
            {
                ErrH.Log("DrawPolygon", "Create", ex.ToString(), ErrH._LogPriority.Info);
            }
        }

        private void LoadCursor()
        {
            _handleCursor = Cursors.SizeAll;
        }

        #endregion Methods

        #region overrides
        public override object Clone()
        {
            // .net does'nt deep copy arraylist. So we will do it
            XnaDrawPolygon copy = new XnaDrawPolygon();
            for (int i = 0; i < _pointArray.Count; i++)
            {
                Point pointToCopy = (Point)_pointArray[i];
                copy._pointArray.Add(new Point(pointToCopy.X, pointToCopy.Y));
            }
            return copy;
        }
        #endregion overrides
    }
}