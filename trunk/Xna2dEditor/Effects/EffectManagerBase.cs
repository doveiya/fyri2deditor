using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fyri2dEditor
{
    public abstract class EffectManagerBase
    {
        public abstract void Initialize();
        public abstract FyriEffect LoadEffect(string fileName);
        public abstract FyriEffect GetEffect(string textureName);
        public abstract List<FyriEffect> RefreshList(List<FyriEffect> list);
    }
}
