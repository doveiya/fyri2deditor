using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xna2dEditor
{
    public enum PathPointType
    {
        Start,
        Line,
        Bezier,
        PathTypeMask,
        DashMode,
        PathMarker,
        CloseSubpath,
        Bezier3
    }



    public class XnaGraphicsPath
    {
        private List<PointFx> pts = new List<PointFx>();
        //private PointFx[] pts;
        private List<PathPointType> types = new List<PathPointType>();
        //private byte[] types;
        //private System.Drawing.Drawing2D.FillMode fillMode;
        public int PointCount { get { return pts.Count; } }

        private Matrix transform;
        public Matrix TransformMatrix
        {
            get { return transform; }
        }


        public XnaGraphicsPath(PointFx[] pts, PathPointType[] types, System.Drawing.Drawing2D.FillMode fillMode)
        {
            // TODO: Complete member initialization
            this.pts.AddRange(pts);
            this.types.AddRange(types);
            //this.fillMode = fillMode;
            transform = Matrix.Identity;

            //BasicPrimitiveRendering.PrimitiveRasterizer polyRasterizer = new BasicPrimitiveRendering.PrimitiveRasterizer(this.GraphicsDevice, this.SpriteBatch);
        }

        public XnaGraphicsPath()
        {
            // TODO: Complete member initialization
            //this.fillMode = System.Drawing.Drawing2D.FillMode.Alternate;
            transform = Matrix.Identity;
        }

        internal RectangleFx GetBounds()
        {
            float x1 = pts[0].X;
            float x2 = pts[0].X;
            float y1 = pts[0].Y;
            float y2 = pts[0].Y;

            foreach (PointFx point in pts)
            {
                if (point.X < x1)
                    x1 = point.X;

                if (point.X > x2)
                    x2 = point.X;

                if (point.Y < y1)
                    y1 = point.Y;

                if (point.Y > y2)
                    y2 = point.Y;
            }

            return new RectangleFx(x1, y1, x2 - x1, y2 - y1);
        }

        internal void Widen(System.Drawing.Pen pen)
        {
            //throw new NotImplementedException();
        }

        internal void AddPath(System.Drawing.Drawing2D.GraphicsPath path, bool p)
        {
            throw new NotImplementedException();
        }

        internal void Reset()
        {
            this.pts.Clear();
            this.types.Clear();
            //this.FillMode = System.Drawing.Drawing2D.FillMode.Alternate;
        }

        internal void AddPath(XnaGraphicsPath path, bool connect)
        {
            if (connect)
            {
                this.AddLine(pts[pts.Count].X, pts[pts.Count].Y, path.PathPoints[0].X, path.PathPoints[0].Y);
                this.pts.AddRange(path.PathPoints);
                this.types.AddRange(path.PathTypes);
            }
            else
            {
                this.pts.AddRange(path.PathPoints);
                this.types.AddRange(path.PathTypes);
            }
        }

        internal void Widen(System.Drawing.Pen pen, System.Drawing.Drawing2D.Matrix matrix)
        {
            throw new NotImplementedException();
        }

        internal void Transform(System.Drawing.Drawing2D.Matrix matrix)
        {
            throw new NotImplementedException();
        }

        internal XnaGraphicsPath Clone()
        {
            throw new NotImplementedException();
        }

        internal void Widen(System.Drawing.Pen pen, Microsoft.Xna.Framework.Matrix matrix)
        {
            //Implement Widen here
        }

        internal void Transform(Microsoft.Xna.Framework.Matrix matrix)
        {
            this.transform = Matrix.Multiply(this.transform, matrix);
        }

        void AddPoint(PointFx point, PathPointType type)
        {
            pts.Add(point);
            types.Add(type);
        }

        internal void AddLine(float x1, float y1, float x2, float y2)
        {
            AddPoint(new PointFx(x1, y1), PathPointType.Start);
            AddPoint(new PointFx(x2, y2), PathPointType.Line);
        }

        internal void AddRectangle(RectangleFx rect)
        {
            float x = rect.X;
            float y = rect.Y;
            float w = rect.Width;
            float h = rect.Height;

            
            AddPoint(new PointFx(x, y), PathPointType.Start);
            AddPoint(new PointFx(x + w, y), PathPointType.Line);
            AddPoint(new PointFx(x + w, y + h), PathPointType.Line);
            AddPoint(new PointFx(x, y + h), PathPointType.Line);
            AddPoint(new PointFx(x, y), PathPointType.CloseSubpath);
            //AddLine(x, y, x + w, y);
            //AddLine(x + w, y, x + w, y + h);
            //AddLine(x + w, y + h, x, y + h);
            //AddLine(x, y + h, x, y);
        }

        internal void AddPolygon(PointFx[] points)
        {
            throw new NotImplementedException();
        }

        internal void CloseFigure()
        {
            throw new NotImplementedException();
        }

        internal void CloseAllFigures()
        {
            throw new NotImplementedException();
        }

        internal void AddEllipse(float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        internal void AddCurve(PointFx[] points)
        {
            throw new NotImplementedException();
        }

        internal void AddClosedCurve(PointFx[] points)
        {
            throw new NotImplementedException();
        }

        internal void AddBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            throw new NotImplementedException();
        }

        internal void AddArc(float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            throw new NotImplementedException();
        }

        public PathData PathData { get; set; }

        //public System.Drawing.Drawing2D.FillMode FillMode { get; set; }

        internal void Flatten()
        {
            throw new NotImplementedException();
        }

        public PointFx[] PathPoints { get { return pts.ToArray(); } }
        public PathPointType[] PathTypes { get { return types.ToArray(); } }

        internal void Draw(Color b)
        {
            //polyRasterizer.Colour = b;
            //polyRasterizer.Position = new Vector2(Transform.Translation.X, Transform.Translation.Y);
            ////polyRasterizer.SetWorld(Transform);

            //for (int i = 0; i < path.PathPoints.Count() - 1; i++)
            //{
            //    Vector2 startPoint = new Vector2(path.PathPoints[i].X, path.PathPoints[i].Y);
            //    Vector2 endPoint = new Vector2(path.PathPoints[i + 1].X, path.PathPoints[i + 1].Y);

            //    polyRasterizer.CreateLine(startPoint, endPoint, BasicPrimitiveRendering.PrimitiveRasterizer.Antialiasing.Enabled, BasicPrimitiveRendering.PrimitiveRasterizer.FillMode.Fill);
            //}
        }
    }
}
