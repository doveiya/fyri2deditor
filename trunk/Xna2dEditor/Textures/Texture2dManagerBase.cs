using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fyri2dEditor
{
    public abstract class Texture2dManagerBase
    {
        public abstract void Initialize();
        public abstract FyriTexture2d LoadTexture2d(string fileName);
        public abstract FyriTexture2d GetTexture2d(string textureName);
        public abstract List<FyriTexture2d> RefreshList(List<FyriTexture2d> list);
    }
}
