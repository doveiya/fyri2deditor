using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FuncWorks.XNA.XSpriter
{
    public class FrameImage
    {
        public Int32 TextureFolder;
        public Int32 TextureFile;
        public Vector2 Position;
        public Vector2 Pivot;
        public Single Angle;
        public Boolean Clockwise;
        public Vector2 Scale;

        public Int32 TimelineId = -1;
        public Int32 Parent = -1;
    }
}
