using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Fyri2dEditor.Xna2dDrawingLibrary;
using BasicPrimitiveRendering;

namespace Xna2dEditor
{
    public class XnaGraphics : IDisposable
    {
        #region SystemDrawing Stuff
        public System.Drawing.RectangleF VisibleClipBounds { get; set; }

        public System.Drawing.Drawing2D.InterpolationMode InterpolationMode { get; set; }

        public System.Drawing.Drawing2D.SmoothingMode SmoothingMode { get; set; }

        public System.Drawing.Drawing2D.CompositingQuality CompositingQuality { get; set; }

        public System.Drawing.Drawing2D.PixelOffsetMode PixelOffsetMode { get; set; }

        public System.Drawing.Drawing2D.CompositingMode CompositingMode { get; set; }

        public System.Drawing.Text.TextRenderingHint TextRenderingHint { get; set; }

        public float DpiY { get; set; }

        #endregion

        private Matrix transform = Matrix.Identity;
        public Matrix Transform 
        { 
            get {return transform;} 
            set {transform = value; }
        }

        public RectangleFx ClipBounds 
        {
            get 
            {

                return new RectangleFx(graphicsDevice.ScissorRectangle.X, graphicsDevice.ScissorRectangle.Y, graphicsDevice.ScissorRectangle.Width, graphicsDevice.ScissorRectangle.Height); 
            } 
            set 
            {
                graphicsDevice.ScissorRectangle = new Rectangle((int)value.X, (int)value.Y, (int)value.Width, (int)value.Height); 
            } 
        }

        public RegionFx Clip { get; set; }

        private GraphicsDevice graphicsDevice;
        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
            set { graphicsDevice = value; }
        }

        private XnaDrawingContext drawingContext;
        public XnaDrawingContext DrawingContext
        {
            get { return drawingContext; }
            set { drawingContext = value; }
        }

        private SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }

        PrimitiveRasterizer polyRasterizer;

        public PrimitiveRasterizer Rasterizer
        {
            get { return polyRasterizer; }
            set { polyRasterizer = value; }
        }

        public XnaGraphics(GraphicsDevice graphicsDevice, XnaDrawingContext drawContext)
        {
            this.graphicsDevice = graphicsDevice;
            this.drawingContext = drawContext;
            this.SpriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(graphicsDevice);

            polyRasterizer = new BasicPrimitiveRendering.PrimitiveRasterizer(this.GraphicsDevice, this.SpriteBatch);
        }

        internal void Clear(System.Drawing.Color color)
        {
            throw new NotImplementedException();
        }

        internal void Flush()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        internal IntPtr GetHdc()
        {
            throw new NotImplementedException();
        }

        internal void ReleaseHdc(IntPtr bmDC)
        {
            throw new NotImplementedException();
        }


        internal void TranslateTransform(float p, float p_2)
        {
            throw new NotImplementedException();
        }

        internal void ScaleTransform(float scale, float scale_2)
        {
            throw new NotImplementedException();
        }

        internal void MultiplyTransform(Matrix matrix)
        {
            this.transform = Matrix.Multiply(transform, matrix);
        }

        internal void DrawString(string text, SpriteFont font, System.Drawing.Brush brush, int p, int p_2, System.Drawing.StringFormat format)
        {
            throw new NotImplementedException();
        }

        internal void DrawImage(System.Drawing.Bitmap bitmap, int p, int p_2)
        {
            throw new NotImplementedException();
        }

        internal void DrawImage(System.Drawing.Bitmap bitmap, int p, int p_2, System.Drawing.Rectangle rectangle, System.Drawing.GraphicsUnit graphicsUnit)
        {
            throw new NotImplementedException();
        }

        internal void DrawPath(System.Drawing.Pen gridPen, System.Drawing.Drawing2D.GraphicsPath gridLine)
        {
            throw new NotImplementedException();
        }

        internal void DrawImage(System.Drawing.Image image, System.Drawing.RectangleF rectangleF)
        {
            throw new NotImplementedException();
        }

        internal void DrawString(string p, SpriteFont font, Color brush, PointFx pointF)
        {
            //DrawingContext.Begin(
            //    SpriteSortMode.Deferred,
            //    BlendState.AlphaBlend,
            //    SamplerState.LinearClamp, DepthStencilState.None,
            //    RasterizerState.CullCounterClockwise, 
            //    null, 
            //    Transform);
            //DrawingContext.Begin();
            DrawingContext.DrawText(font, p, new Vector2(pointF.X, pointF.Y), brush);
            //DrawingContext.End();
        }


        internal void DrawString(string text, SpriteFont renderFont, Color textBrush, RectangleFx rectangleF, System.Drawing.StringFormat stringFormat)
        {
            DrawingContext.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                Transform);
            //DrawingContext.Begin();
            DrawingContext.DrawText(renderFont, text, new Vector2(rectangleF.X, rectangleF.Y), textBrush);
            DrawingContext.End();
        }

        internal void DrawRectangle(System.Drawing.Pen boundsPen, float p, float p_2, float p_3, float p_4)
        {
            throw new NotImplementedException();
        }

        internal void DrawRectangle(Color brush, RectangleFx rectangle)
        {
            DrawingContext.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Transform);
            //DrawingContext.Begin();
            DrawingContext.DrawRectangle(rectangle.ToRectangle(), brush);
            DrawingContext.End();
        }

        internal void DrawImage(System.Drawing.Image ImageCache, int p, int p_2)
        {
            throw new NotImplementedException();
        }

        internal void DrawImage(System.Drawing.Image paintBuffer, System.Drawing.RectangleF rectangleF, System.Drawing.RectangleF rectangleF_2, System.Drawing.GraphicsUnit graphicsUnit)
        {
            throw new NotImplementedException();
        }

        internal void DrawPath(System.Drawing.Pen pen, XnaGraphicsPath path)
        {
            //Vector2[] vectors = new Vector2[path.PointCount];

            //for(int i = 0; i < path.PointCount; i++)
            //{
            //    vectors[i] = new Vector2(path.PathPoints[i].X, path.PathPoints[i].Y);
            //}

            Color c = Color.FromNonPremultiplied(pen.Color.R, pen.Color.G, pen.Color.B, pen.Color.A);

            //DrawingContext.Begin();
            //DrawingContext.DrawPolyline(vectors, c);
            //DrawingContext.End();

            //BasicPrimitiveRendering.PrimitiveRasterizer polyRasterizer = new BasicPrimitiveRendering.PrimitiveRasterizer(this.GraphicsDevice, this.SpriteBatch);
            //polyRasterizer.Colour = c;
            //polyRasterizer.Thickness = pen.Width;

            //for (int i = 0; i < path.PathPoints.Count() - 1; i++)
            //{
            //    Vector2 startPoint = new Vector2(path.PathPoints[i].X, path.PathPoints[i].Y);
            //    Vector2 endPoint = new Vector2(path.PathPoints[i + 1].X, path.PathPoints[i + 1].Y);

            //    polyRasterizer.CreateLine(startPoint, endPoint, BasicPrimitiveRendering.PrimitiveRasterizer.Antialiasing.Enabled, BasicPrimitiveRendering.PrimitiveRasterizer.FillMode.Outline);
            //}

            ////Connect the last lines to close figure?
            ////Vector2 sPoint = new Vector2(path.PathPoints[path.PathPoints.Count()].X, path.PathPoints[path.PathPoints.Count()].Y);
            ////Vector2 ePoint = new Vector2(path.PathPoints[0].X, path.PathPoints[0].Y);

            ////polyRasterizer.CreateLine(sPoint, ePoint, BasicPrimitiveRendering.PrimitiveRasterizer.Antialiasing.Enabled, BasicPrimitiveRendering.PrimitiveRasterizer.FillMode.Fill);

            //polyRasterizer.Render();
        }

        internal void FillRectangle(Color brush, RectangleFx rectangle)
        {
            DrawingContext.DrawFilledRectangle(rectangle.ToRectangle(), brush);
        }

        internal void FillRectangle(Color backgroundBrush, int p, int p_2, int width, int height)
        {
            throw new NotImplementedException();
        }

        internal void FillPath(Color b, System.Drawing.Drawing2D.GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        internal void FillPath(Matrix transformM, Color b, XnaGraphicsPath path)
        {
            polyRasterizer.Clear();
            polyRasterizer.Colour = b;
            polyRasterizer.Position = new Vector2(Transform.Translation.X, Transform.Translation.Y);
            //polyRasterizer.SetWorld(Transform);

            for (int i = 0; i < path.PathPoints.Count() - 1; i++)
            {
                Vector2 startPoint = new Vector2(path.PathPoints[i].X, path.PathPoints[i].Y);
                Vector2 endPoint = new Vector2(path.PathPoints[i + 1].X, path.PathPoints[i + 1].Y);

                polyRasterizer.CreateLine(startPoint, endPoint, PrimitiveRasterizer.Antialiasing.Enabled, PrimitiveRasterizer.FillMode.Fill);
            }

            //Connect the last lines to close figure?
            //Vector2 sPoint = new Vector2(path.PathPoints[path.PathPoints.Count()].X, path.PathPoints[path.PathPoints.Count()].Y);
            //Vector2 ePoint = new Vector2(path.PathPoints[0].X, path.PathPoints[0].Y);

            //polyRasterizer.CreateLine(sPoint, ePoint, BasicPrimitiveRendering.PrimitiveRasterizer.Antialiasing.Enabled, BasicPrimitiveRendering.PrimitiveRasterizer.FillMode.Fill);

            polyRasterizer.Render();
        }

        internal static XnaGraphics FromImage(System.Drawing.Bitmap bitmap)
        {
            throw new NotImplementedException();
        }

        internal static XnaGraphics FromImage(System.Drawing.Image image)
        {
            throw new NotImplementedException();
        }

        internal static XnaGraphics FromGraphics(System.Drawing.Graphics graphics)
        {
            throw new NotImplementedException();
        }
    }
}
