using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xna2dEditor
{
    public struct PointFx
    {
        private float x;
        public float X
        {
            get { return x; }
            set { x = value;}
        }

        private float y;
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public PointFx(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        private static PointFx mEmpty;

        static PointFx()
        {
            PointFx.mEmpty = new PointFx();
        }

        public static PointFx Empty
        {
            get { return PointFx.mEmpty; }
        }

        public bool IsEmpty
        {
            get { return this == PointFx.Empty; }
        }

        public static bool operator ==(PointFx a, PointFx b)
        {
            return
                (a.X == b.X) &&
                (a.Y == b.Y);
        }

        public static bool operator !=(PointFx a, PointFx b)
        {
            return
                (a.X != b.X) ||
                (a.Y != b.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is PointFx)
            {
                return this == (PointFx)obj;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
