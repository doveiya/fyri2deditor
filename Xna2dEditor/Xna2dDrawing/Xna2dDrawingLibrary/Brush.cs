using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fyri2dEditor.Xna2dDrawingLibrary
{
    public abstract class Brush
    {
        public abstract Texture2D Texture
        {
            get;
        }

        public abstract Rectangle Region
        {
            get;
        }
    }
}
