using System;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using TestStack.White.InputDevices;

namespace WhiteSharp.Controls
{
    public class Button : Control
    {
        private bool _clicked = false;

        public Button(AutomationElement automationElement, IControlContainer window, By searchCriteria, int index) 
            : base(automationElement, window, searchCriteria, index)
        {
            Automation.AddAutomationEventHandler(InvokePattern.InvokedEvent, AutomationElement, TreeScope.Element,
                (sender, args) =>
                {
                    _clicked = true;
                });
        }

        public override IControl ClickAnyway()
        {
            DateTime start = DateTime.Now;
            Point? point = null;
            while (!_clicked && (DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout)
            {
                try
                {
                    point = AutomationElement.GetClickablePoint();
                }
                catch (NoClickablePointException)
                {
                    ((Window) Window).OnTop();
                }
                Mouse.Instance.Click(point != null
                    ? new Point(point.Value.X, point.Value.Y)
                    : AutomationElement.GetClickablePoint());
                Logging.Click(SearchCriteria.Identifiers);
                //TODO: костыль
                Thread.Sleep(2000);
            }
            _clicked = false;
            Logging.Click(SearchCriteria.Identifiers);
            return this;
        }
    }
}
