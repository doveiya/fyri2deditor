using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xna2dEditor;

namespace Fyri2dEditor
{
    public interface ISpriterManager
    {
        void Initialize();
        FyriSpriter LoadSpriter(string fileName);
        FyriSpriter GetSpriter(string modelName);
        List<FyriSpriter> RefreshList(List<FyriSpriter> list);
    }
}
