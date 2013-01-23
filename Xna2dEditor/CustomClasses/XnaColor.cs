using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.ComponentModel;

namespace Xna2dEditor
{
    // This like tells it to use our custom type converter instead of the default.
    [TypeConverter(typeof(XnaColorConverter))]
    public class XnaColor
    {
        #region " The color channel variables w/ accessors/mutators. "
        private byte _Red;
        public byte R
        {
            get { return _Red; }
            set { _Red = value; }
        }

        private byte _Green;
        public byte G
        {
            get { return _Green; }
            set { _Green = value; }
        }

        private byte _Blue;
        public byte B
        {
            get { return _Blue; }
            set { _Blue = value; }
        }

        private byte _Alpha;
        public byte A
        {
            get { return _Alpha; }
            set { _Alpha = value; }
        }
        #endregion

        #region " Constructors "
        public XnaColor()
        {
            _Red = 0;
            _Green = 0;
            _Blue = 0;
            _Alpha = 0;
        }
        public XnaColor(byte red, byte green, byte blue)
        {
            _Red = red;
            _Green = green;
            _Blue = blue;
            _Alpha = 255;
        }

        public XnaColor(byte red, byte green, byte blue, byte alpha)
        {
            _Red = red;
            _Green = green;
            _Blue = blue;
            _Alpha = alpha;
        }
        public XnaColor(byte[] rgb)
        {
            if (rgb.Length != 3)
                throw new Exception("Array must have a length of 3.");
            _Red = rgb[0];
            _Green = rgb[1];
            _Blue = rgb[2];
            _Alpha = 255;
        }

        public XnaColor(int argb)
        {
            byte[] bytes = BitConverter.GetBytes(argb);
            _Red = bytes[2];
            _Green = bytes[1];
            _Blue = bytes[0];
            _Alpha = 255;
        }
        public XnaColor(string rgba)
        {
            string[] parts = rgba.Split(' ');
            if (parts.Length != 4)
                throw new Exception("Array must have a length of 4.");
            _Red = Convert.ToByte(parts[0]);
            _Green = Convert.ToByte(parts[1]);
            _Blue = Convert.ToByte(parts[2]);
            _Alpha = Convert.ToByte(parts[3]);
        }

        public XnaColor(Color color)
        {
            // TODO: Complete member initialization
            this.A = color.A;
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
        }
        #endregion

        #region " Methods "
        public new string ToString()
        {
            return String.Format("{0} {1} {2} {3}", _Red, _Green, _Blue, _Alpha); ;
        }
        public byte[] GetRGB()
        {
            return new byte[] { _Red, _Green, _Blue };
        }
        public int GetARGB()
        {
            byte[] temp = new byte[] { _Blue, _Green, _Red, _Alpha };
            return BitConverter.ToInt32(temp, 0);
        }

        public Color ToColor()
        {
            return Color.FromNonPremultiplied(this.R, this.G, this.B, this.A);
        }
        #endregion
    }
}
