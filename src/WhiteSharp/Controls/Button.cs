using System.Windows.Automation;

namespace WhiteSharp.Controls
{
    public class Button : Control
    {
        public Button(AutomationElement automationElement, Window window, By searchCriteria, int index) 
            : base(automationElement, window, searchCriteria, index)
        {
        }
    }
}
