using System;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using TestStack.White.InputDevices;

namespace WhiteSharp.Controls
{
    public class Button : Control
    {
        public Button(AutomationElement automationElement, Window window, By searchCriteria, int index) 
            : base(automationElement, window, searchCriteria, index)
        {
        }

        public override IControl ClickAnyway()
        {
            DateTime start = DateTime.Now;
            Point? point = null;
            while (!Clicked)
            {
                try
                {
                    point = AutomationElement.GetClickablePoint();
                }
                catch (NoClickablePointException)
                {
                    Window.OnTop();
                }
                if (point != null)
                {
                    Mouse.Instance.Click(new Point(point.Value.X, point.Value.Y));
                }
                //TODO: костыль
                Thread.Sleep(2000);
            }
            Logging.Click(SearchCriteria.Identifiers);
            return this;
        }
    }
}
