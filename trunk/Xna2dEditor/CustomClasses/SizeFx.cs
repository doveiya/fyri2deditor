using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xna2dEditor
{
    public struct SizeFx
    {
        private float width;
        public float Width
        {
            get { return width; }
            set { width = value;}
        }

        private float height;
        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public SizeFx(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        private static SizeFx mEmpty;

        static SizeFx()
        {
            SizeFx.mEmpty = new SizeFx();
        }

        public static SizeFx Empty
        {
            get { return SizeFx.mEmpty; }
        }

        public bool IsEmpty
        {
            get { return this == SizeFx.Empty; }
        }

        public static bool operator ==(SizeFx a, SizeFx b)
        {
            return
                (a.Width == b.Width) &&
                (a.Height == b.Height);
        }

        public static bool operator !=(SizeFx a, SizeFx b)
        {
            return
                (a.Width != b.Width) ||
                (a.Height != b.Height);
        }

        public override bool Equals(object obj)
        {
            if (obj is SizeFx)
            {
                return this == (SizeFx)obj;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public System.Drawing.SizeF ToSizeF()
        {
            return new System.Drawing.SizeF(this.Width, this.Height);
        }
    }
}
