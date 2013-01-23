using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;

namespace Xna2dEditor
{
    // This is an example of a class for the PropertyGrid to select.
    public class SelectableClass
    {
        private XnaColor _Color = new XnaColor();
        [EditorAttribute(typeof(XnaColorEditor), typeof(UITypeEditor))]
        [CategoryAttribute("Design"), DefaultValueAttribute(typeof(XnaColor), "0 0 0 0"), DescriptionAttribute("Example of a ColorDialog in a PropertyGrid.")]
        public XnaColor Color
        {
            get { return _Color; }
            set { _Color = value; }
        }
    }
}
