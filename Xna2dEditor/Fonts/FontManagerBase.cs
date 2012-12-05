using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fyri2dEditor
{
    public abstract class FontManagerBase
    {
        public abstract void Initialize();
        public abstract FyriFont LoadFont(string fileName);
        public abstract FyriFont GetFont(string textureName);
        public abstract List<FyriFont> RefreshList(List<FyriFont> list);
    }
}
