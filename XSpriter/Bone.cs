using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FuncWorks.XNA.XSpriter
{
    public class Bone
    {
        public Int32 Id;
        public Int32 Parent = -1;
        public String Name;
        public Vector2 Position;
        public Vector2 Scale;
        public Single Angle;
        public Int32 TimelineId = -1;
        public Boolean Clockwise;
    }
}
