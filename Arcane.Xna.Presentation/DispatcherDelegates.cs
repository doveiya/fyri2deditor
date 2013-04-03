using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Collections;

namespace FyriDispatcher
{
    public delegate void Dispatched();
    public delegate void DispatchedObject(object objItem);
    public delegate void DispatchedPriority(DispatcherPriority ePriority);
    public delegate void DispatchedInt(int iInteger);
    public delegate void DispatchedDouble(double dDouble);
    public delegate void DispatchedString(string strString);
    public delegate void DispatchedIList(IList objCollection);
}
