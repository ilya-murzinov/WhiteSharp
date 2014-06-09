using System;
using System.Windows.Automation;

namespace WhiteSharp.Factories
{
    internal class ControlFactory
    {
        internal static T Create<T>(AutomationElement automationElement, IControlContainer window, By searchCriteria, int index) where T : class, IControl
        {
            if (automationElement == null)
            {
                return null;
            }
            return (T) Activator.CreateInstance(typeof(T), automationElement, window, searchCriteria, index);
        }

        internal static IControl Create(AutomationElement automationElement, IControlContainer window, By searchCriteria, int index)
        {
            if (automationElement == null)
            {
                return null;
            }
            return Create<Control>(automationElement, window, searchCriteria, index);
        }
    }
}