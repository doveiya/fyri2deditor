using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fyri2dEditor.RenderEngine
{
    public abstract class RenderEngineBase
    {
        public abstract bool Initialize();

        public abstract bool Draw();

    }
}
