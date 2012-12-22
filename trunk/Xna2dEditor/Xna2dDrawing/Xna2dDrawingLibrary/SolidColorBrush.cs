using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fyri2dEditor.Xna2dDrawingLibrary
{
    public class SolidColorBrush : Brush
    {
        public Color Color
        {
            get;
            private set;
        }

        public SolidColorBrush(Color color)
        {
            this.Color = color;
        }

        private Texture2D texture;
        public override Texture2D Texture
        {
            get { return this.texture; }
        }

        private Rectangle region;
        public override Rectangle Region
        {
            get { return this.region; }
        }
    }
}
