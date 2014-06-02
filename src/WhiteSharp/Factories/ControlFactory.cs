using System;
using System.Windows.Automation;

namespace WhiteSharp.Factories
{
    internal class ControlFactory
    {
        internal static T Create<T>(AutomationElement automationElement, Window window, By searchCriteria, int index)
        {
            return (T) Activator.CreateInstance(typeof(T), automationElement, window, searchCriteria, index);
        }

        internal static IControl Create(AutomationElement automationElement, Window window, By searchCriteria, int index)
        {
            return Create<Control>(automationElement, window, searchCriteria, index);
        }
    }
}