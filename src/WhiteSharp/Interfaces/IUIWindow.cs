using System;
using System.Collections.Generic;
using System.Windows.Automation;
using TestStack.White.Factory;
using TestStack.White.UIItems.WindowItems;

namespace WhiteSharp.Interfaces
{
    internal interface IUIWindow
    {
        UIControl FindControl(Finder finder);
        List<AutomationElement> FindAll(Finder finder);
        void Send(string message);
    }
}
