using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncWorks.XNA.XSpriter
{
    public class AnimationList : List<Animation>
    {
        public Animation this[string name]
        {
            get { return this.FirstOrDefault(x => x.Name.Equals(name)); }
        }
    }
}
