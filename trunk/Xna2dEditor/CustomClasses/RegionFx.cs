using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xna2dEditor
{
    public class RegionFx
    {
        private System.Drawing.RectangleF rectangleF;
        private RectangleFx Bounds;

        public RegionFx(System.Drawing.RectangleF rectangleF)
        {
            // TODO: Complete member initialization
            this.rectangleF = rectangleF;
        }

        public RegionFx(RectangleFx Bounds)
        {
            // TODO: Complete member initialization
            this.Bounds = Bounds;
        }

        public RegionFx()
        {
            // TODO: Complete member initialization
        }

        internal void Intersect(RegionFx currentClip)
        {
            throw new NotImplementedException();
        }

        internal RegionFx Clone()
        {
            throw new NotImplementedException();
        }

        internal bool IsVisible(System.Drawing.RectangleF rectangleF)
        {
            throw new NotImplementedException();
        }

        internal bool IsVisible(RectangleFx bounds)
        {
            throw new NotImplementedException();
        }

        internal void MakeInfinite()
        {
            throw new NotImplementedException();
        }

        internal void Intersect(XnaGraphicsPath TEMP_PATH)
        {
            throw new NotImplementedException();
        }
    }
}
