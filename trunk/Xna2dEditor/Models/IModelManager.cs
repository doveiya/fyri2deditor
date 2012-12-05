using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fyri2dEditor
{
    public interface IModelManager
    {
        void Initialize();
        FyriModel LoadModel(string fileName);
        FyriModel GetModel(string modelName);
        List<FyriModel> RefreshList(List<FyriModel> list);
    }
}
