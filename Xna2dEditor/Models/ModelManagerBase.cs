using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fyri2dEditor
{
    public abstract class ModelManagerBase
    {
        public abstract void Initialize();
        public abstract FyriModel LoadModel(string fileName);
        public abstract FyriModel GetModel(string modelName);
        public abstract List<FyriModel> RefreshList(List<FyriModel> list);
    }
}
