using System.Windows.Automation;

namespace WhiteSharp
{
    public static class ControlFactory
    {
        internal static T Create<T>(AutomationElement automationElement) where T: Control
        {
            var constructorInfo = typeof (T).GetConstructor(new[] {typeof(AutomationElement)});
            if (constructorInfo != null)
                return (T) constructorInfo.Invoke(new object[] {automationElement});
            return null;
        }
    }
}
