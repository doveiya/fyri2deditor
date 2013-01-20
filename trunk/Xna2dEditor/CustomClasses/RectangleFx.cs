using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Xna2dEditor
{
    /// <summary>
    /// Much like Rectangle, but stored as two Vector2s
    /// </summary>
    [Serializable()]
    public struct RectangleFx : ISerializable
    {
        public Vector2 Min;
        public Vector2 Max;

        public float Left { get { return this.Min.X; } }
        public float Right { get { return this.Max.X; } }
        public float Top { get { return this.Max.Y; } }
        public float Bottom { get { return this.Min.Y; } }

        public float Width 
        { 
            get { return this.Max.X - this.Min.X; }
            //set { this.Max.X = this.Min.X + value;  }
        }
        public float Height 
        { 
            get { return this.Max.Y - this.Min.Y; }
            //set
            //{
            //    this.Max.Y = this.Min.Y + value;
            //}
        }

        public PointFx Location
        {
            get { return new PointFx(this.Min.X, this.Min.Y); }
            //set 
            //{ 
            //    this.Min.X = value.X;
            //    this.Min.Y = value.Y;
            //    this.Max.X = value.X + this.Width;
            //    this.Max.Y = value.Y + this.Height;
            //}
        }

        private static RectangleFx mEmpty;
        private static RectangleFx mMinMax;

        static RectangleFx()
        {
            RectangleFx.mEmpty = new RectangleFx();
            RectangleFx.mMinMax = new RectangleFx(Vector2.One * float.MinValue, Vector2.One * float.MaxValue);
        }

        //public RectangleFx()
        //{
        //}

        public Vector2 Center
        {
            get { return (this.Min + this.Max) / 2; }
        }

        public float X
        {
            get { return this.Min.X; }
            //set
            //{
            //    this.Max.X = value + this.Width;
            //    this.Min.X = value;
            //}
        }

        public float Y
        {
            get { return this.Min.Y; }
            //set 
            //{
            //    this.Max.Y = value + Height;
            //    this.Min.Y = value;
            //}
        }

        public static RectangleFx Empty
        {
            get { return RectangleFx.mEmpty; }
        }


        public static RectangleFx MinMax
        {
            get { return RectangleFx.mMinMax; }
        }

        public bool IsEmpty
        {
            get { return this == RectangleFx.Empty; }
        }

        public bool IsZero
        {
            get
            {
                return
                    (this.Min.X == 0) &&
                    (this.Min.Y == 0) &&
                    (this.Max.X == 0) &&
                    (this.Max.Y == 0);
            }
        }

        public RectangleFx(float x, float y, float width, float height)
        {
            this.Min.X = x;
            this.Min.Y = y;
            this.Max.X = x + width;
            this.Max.Y = y + height;
        }

        public RectangleFx(Vector2 min, Vector2 max)
        {
            this.Min = min;
            this.Max = max;
        }

        public bool Contains(float x, float y)
        {
            return
                (this.Min.X <= x) &&
                (this.Min.Y <= y) &&
                (this.Max.X >= x) &&
                (this.Max.Y >= y);
        }

        public bool Contains(Vector2 vector)
        {
            return
                (this.Min.X <= vector.X) &&
                (this.Min.Y <= vector.Y) &&
                (this.Max.X >= vector.X) &&
                (this.Max.Y >= vector.Y);
        }

        public void Contains(ref Vector2 rect, out bool result)
        {
            result =
                (this.Min.X <= rect.X) &&
                (this.Min.Y <= rect.Y) &&
                (this.Max.X >= rect.X) &&
                (this.Max.Y >= rect.Y);
        }

        public bool Contains(RectangleFx rect)
        {
            return
                (this.Min.X <= rect.Min.X) &&
                (this.Min.Y <= rect.Min.Y) &&
                (this.Max.X >= rect.Max.X) &&
                (this.Max.Y >= rect.Max.Y);
        }

        public void Contains(ref RectangleFx rect, out bool result)
        {
            result =
                (this.Min.X <= rect.Min.X) &&
                (this.Min.Y <= rect.Min.Y) &&
                (this.Max.X >= rect.Max.X) &&
                (this.Max.Y >= rect.Max.Y);
        }

        public bool IntersectsWith(RectangleFx rect)
        {
            return this.ToRectangleF().IntersectsWith(rect.ToRectangleF());
        }

        public bool Intersects(RectangleFx rect)
        {
            return
                (this.Min.X < rect.Max.X) &&
                (this.Min.Y < rect.Max.Y) &&
                (this.Max.X > rect.Min.X) &&
                (this.Max.Y > rect.Min.Y);
        }

        public void Intersects(ref RectangleFx rect, out bool result)
        {
            result =
                (this.Min.X < rect.Max.X) &&
                (this.Min.Y < rect.Max.Y) &&
                (this.Max.X > rect.Min.X) &&
                (this.Max.Y > rect.Min.Y);
        }

        

        public override bool Equals(object obj)
        {
            if (obj is RectangleFx)
            {
                return this == (RectangleFx)obj;
            }

            return false;
        }

        public bool Equals(RectangleFx other)
        {
            return
                (this.Min.X == other.Min.X) &&
                (this.Min.Y == other.Min.Y) &&
                (this.Max.X == other.Max.X) &&
                (this.Max.Y == other.Max.Y);
        }

        public override int GetHashCode()
        {
            return this.Min.GetHashCode() + this.Max.GetHashCode();
        }

        public System.Drawing.RectangleF ToRectangleF()
        {
            return new System.Drawing.RectangleF(this.X, this.Y, this.Width, this.Height);
        }

        public System.Drawing.Rectangle ToSystemRectangle()
        {
            return new System.Drawing.Rectangle((int)this.X, (int)this.Y, (int)this.Width, (int)this.Height);
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle((int)this.X, (int)this.Y, (int)this.Width, (int)this.Height);
        }

        public RectangleFx Inflate(float x, float y)
        {
            System.Drawing.RectangleF rect = this.ToRectangleF();
            rect.Inflate(x, y);
            this.Min.X = rect.X;
            this.Min.Y = rect.Y;
            this.Max.X = rect.X + rect.Width;
            this.Max.Y = rect.Y + rect.Height;
            return this;
        }

        public static bool operator ==(RectangleFx a, RectangleFx b)
        {
            return
                (a.Min.X == b.Min.X) &&
                (a.Min.Y == b.Min.Y) &&
                (a.Max.X == b.Max.X) &&
                (a.Max.Y == b.Max.Y);
        }

        public static bool operator !=(RectangleFx a, RectangleFx b)
        {
            return
                (a.Min.X != b.Min.X) ||
                (a.Min.Y != b.Min.Y) ||
                (a.Max.X != b.Max.X) ||
                (a.Max.Y != b.Max.Y);
        }

        public RectangleFx(SerializationInfo info, StreamingContext context)
        {
            this.Min = (Vector2)info.GetValue("Min", typeof(Vector2));
            this.Max = (Vector2)info.GetValue("Max", typeof(Vector2));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Min", this.Min);
            info.AddValue("Max", this.Max);
        }
    }

    public static class RectangleFxtensions
    {
        #region Extensions

        public static RectangleFx ToRectangleFx(System.Drawing.RectangleF rect)
        {
            return new RectangleFx(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static RectangleFx Intersect(RectangleFx rect1, RectangleFx rect2)
        {
            RectangleFx result;

            float num8 = rect1.Max.X;
            float num7 = rect2.Max.X;
            float num6 = rect1.Max.Y;
            float num5 = rect2.Max.Y;
            float num2 = (rect1.Min.X > rect2.Min.X) ? rect1.Min.X : rect2.Min.X;
            float num = (rect1.Min.Y > rect2.Min.Y) ? rect1.Min.Y : rect2.Min.Y;
            float num4 = (num8 < num7) ? num8 : num7;
            float num3 = (num6 < num5) ? num6 : num5;

            if ((num4 > num2) && (num3 > num))
            {
                result.Min.X = num2;
                result.Min.Y = num;
                result.Max.X = num4;
                result.Max.Y = num3;

                return result;
            }

            result.Min.X = 0;
            result.Min.Y = 0;
            result.Max.X = 0;
            result.Max.Y = 0;

            return result;
        }

        public static void Intersect(ref RectangleFx rect1, ref RectangleFx rect2, out RectangleFx result)
        {
            result = new RectangleFx();
            float num8 = rect1.Max.X;
            float num7 = rect2.Max.X;
            float num6 = rect1.Max.Y;
            float num5 = rect2.Max.Y;
            float num2 = (rect1.Min.X > rect2.Min.X) ? rect1.Min.X : rect2.Min.X;
            float num = (rect1.Min.Y > rect2.Min.Y) ? rect1.Min.Y : rect2.Min.Y;
            float num4 = (num8 < num7) ? num8 : num7;
            float num3 = (num6 < num5) ? num6 : num5;

            if ((num4 > num2) && (num3 > num))
            {
                result.Min.X = num2;
                result.Min.Y = num;
                result.Max.X = num4;
                result.Max.Y = num3;
            }

            result.Min.X = 0;
            result.Min.Y = 0;
            result.Max.X = 0;
            result.Max.Y = 0;
        }

        public static RectangleFx Union(RectangleFx rect1, RectangleFx rect2)
        {
            RectangleFx result = new RectangleFx();

            float num6 = rect1.Max.X;
            float num5 = rect2.Max.X;
            float num4 = rect1.Max.Y;
            float num3 = rect2.Max.Y;
            float num2 = (rect1.Min.X < rect2.Min.X) ? rect1.Min.X : rect2.Min.X;
            float num = (rect1.Min.Y < rect2.Min.Y) ? rect1.Min.Y : rect2.Min.Y;
            float num8 = (num6 > num5) ? num6 : num5;
            float num7 = (num4 > num3) ? num4 : num3;

            result.Min.X = num2;
            result.Min.Y = num;
            result.Max.X = num8;
            result.Max.Y = num7;

            return result;
        }

        public static void Union(ref RectangleFx rect1, ref RectangleFx rect2, out RectangleFx result)
        {
            result = new RectangleFx();

            float num6 = rect1.Max.X;
            float num5 = rect2.Max.X;
            float num4 = rect1.Max.Y;
            float num3 = rect2.Max.Y;
            float num2 = (rect1.Min.X < rect2.Min.X) ? rect1.Min.X : rect2.Min.X;
            float num = (rect1.Min.Y < rect2.Min.Y) ? rect1.Min.Y : rect2.Min.Y;
            float num8 = (num6 > num5) ? num6 : num5;
            float num7 = (num4 > num3) ? num4 : num3;

            result.Min.X = num2;
            result.Min.Y = num;
            result.Max.X = num8;
            result.Max.Y = num7;
        }        

        

        public static RectangleFx CenterRectangle(this RectangleFx r, int x, int y)
        {
            return new RectangleFx(x - r.Center.X, y - r.Center.Y, r.Width, r.Height);
        }

        public static Point GetSizeAsPoint(this RectangleFx r)
        {
            return new Point((int)r.Width, (int)r.Height);
        }

        public static Vector2 GetSizeAsVector2(this RectangleFx r)
        {
            return new Vector2(r.Width, r.Height);
        }

        public static Point BottomRightAsPoint(this RectangleFx r)
        {
            return new Point((int)r.Right, (int)r.Bottom);
        }

        public static Vector2 BottomRightAsVector2(this RectangleFx r)
        {
            return new Vector2(r.Right, r.Bottom);
        }

        public static bool Contains(this RectangleFx r, float x, float y)
        {
            return x >= r.Left && x <= r.Left + r.Width && y >= r.Top && y <= r.Top + r.Height;
        }

        public static bool Contains(this RectangleFx r, Vector2 pos)
        {
            return pos.X >= r.Left && pos.X <= r.Left + r.Width && pos.Y >= r.Top && pos.Y <= r.Top + r.Height;
        }

        public static RectangleFx FromLTRB(this RectangleFx src, float left, float top, float right, float bottom)
        {
            return new RectangleFx(src.Left, src.Top, src.Right - src.Left, src.Bottom - src.Top);
        }


        #endregion
    }
}
