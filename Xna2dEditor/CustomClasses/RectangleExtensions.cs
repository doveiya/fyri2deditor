using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Xna2dEditor
{
    public static class RectangleExtensions
    {
        public static Rectangle CenterRectangle(this Rectangle r, int x, int y)
        {
            return new Rectangle(x - r.Center.X, y - r.Center.Y, r.Width, r.Height);
        }

        public static Point GetSizeAsPoint(this Rectangle r)
        {
            return new Point(r.Width, r.Height);
        }

        public static Vector2 GetSizeAsVector2(this Rectangle r)
        {
            return new Vector2(r.Width, r.Height);
        }

        public static Point BottomRightAsPoint(this Rectangle r)
        {
            return new Point(r.Right, r.Bottom);
        }

        public static Vector2 BottomRightAsVector2(this Rectangle r)
        {
            return new Vector2(r.Right, r.Bottom);
        }

        public static bool Contains(this Rectangle r, float x, float y)
        {
            return x >= r.X && x <= r.X + r.Width && y >= r.Y && y <= r.Y + r.Height;
        }

        public static bool Contains(this Rectangle r, Vector2 pos)
        {
            return pos.X >= r.X && pos.X <= r.X + r.Width && pos.Y >= r.Y && pos.Y <= r.Y + r.Height;
        }

        public static Rectangle FromLTRB(this Rectangle src, float left, float top, float right, float bottom)
        {
            return new Rectangle(src.X, src.Y, src.Right - src.X, src.Bottom - src.Y);
        }
    }
}
