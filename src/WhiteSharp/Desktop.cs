using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using Castle.Core.Internal;
using WhiteSharp.Extensions;

namespace WhiteSharp
{
    internal class Desktop
    {
        public List<AutomationElement> Windows { get; private set; }

        private Desktop()
        {
            Windows = AutomationElement.RootElement.FindAll(TreeScope.Children,
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window))
                .OfType<AutomationElement>().ToList();
        }

        private static Desktop _instance;

        public static Desktop Instance
        {
            get { return (_instance = new Desktop()); }
        }
    }
}