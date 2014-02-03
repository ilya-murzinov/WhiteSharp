using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace WhiteSharp
{
    internal class Desktop
    {
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

        public List<AutomationElement> Windows { get; private set; }
    }
}