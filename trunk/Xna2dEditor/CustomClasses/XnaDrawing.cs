using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Fyri2dEditor.Xna2dDrawingLibrary;

namespace Xna2dEditor
{
    public class XnaDrawing
    {
        public static Texture2D blank;
        public static SpriteBatch spriteBatch;
        public static XnaDrawingBatch drawingBatch;
        public static SpriteFont defaultFont;

        public static void DrawLine(float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }

        internal static void DrawFilledRectangle(Rectangle r, Color Fill)
        {
            spriteBatch.Draw(blank, r, Fill);
        }

        internal static void DrawRectangle(Rectangle rect, Color Stroke)
        {
            float x = rect.X;
            float y = rect.Y;
            float w = rect.Width;
            float h = rect.Height;

            Vector2 p1 = new Vector2(x, y);
            Vector2 p2 = new Vector2(x + w, y);
            Vector2 p3 = new Vector2(x + w, y + h);
            Vector2 p4 = new Vector2(x, y + h);
            //lines.Add(new RoundLine(x, y, x + w, y));
            //lines.Add(new RoundLine(x + w, y, x + w, y + h));
            //lines.Add(new RoundLine(x + w, y + h, x, y + h));
            //lines.Add(new RoundLine(x, y + h, x, y));

            DrawLine(2.0f, Color.Black, p1, p2);
            DrawLine(2.0f, Color.Black, p2, p3);
            DrawLine(2.0f, Color.Black, p3, p4);
            DrawLine(2.0f, Color.Black, p4, p1);
        }

        internal static void DrawFilledEllipse(Rectangle r, Color Fill)
        {
            drawingBatch.DrawFilledEllipse(r, Fill);
        }

        internal static void DrawEllipse(Rectangle r, Color Stroke)
        {
            drawingBatch.DrawEllipse(r, Stroke);
        }

        internal static void DrawPolyline(Vector2[] arr, Color Fill)
        {
            throw new NotImplementedException();
        }

        internal static void DrawText(SpriteFont Font, string Text, Color Stroke, Rectangle Rectangle, System.Drawing.StringFormat TextAnchor)
        {
            spriteBatch.DrawString(Font, Text, new Vector2(Rectangle.X, Rectangle.Y), Stroke);
        }
    }
}
