using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FuncWorks.XNA.XSpriter
{
    public struct AnimationTransform
    {
        public Vector2 Position;
        public Single Angle;
        public Vector2 Scale;
        public Boolean Hidden;

        public AnimationTransform(Single angle, Vector2 position, Vector2 scale)
        {
            Angle = angle;
            Position = position;
            Scale = scale;
            Hidden = false;
        }
    }

    public struct RuntimeTransform
    {
        public Int32? TimelineId;
        public String BoneName;
        public Vector2 Position;
        public Single Angle;
        public Vector2 Scale;
        public Boolean Hidden;
    }

    public struct RenderedPosition
    {
        public Vector2 Positon;
        public Vector2 Pivot;
        public Single Angle;
        public Vector2 Scale;
        public SpriteEffects Effects;
        public Single Layer;
    }
}
