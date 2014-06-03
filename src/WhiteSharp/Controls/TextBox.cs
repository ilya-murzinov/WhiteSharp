using System;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using TestStack.White.InputDevices;

namespace WhiteSharp.Controls
{
    public class TextBox : Control
    {
        public TextBox(AutomationElement automationElement, Window window, By searchCriteria, int index) 
            : base(automationElement, window, searchCriteria, index)
        {
        }

        override public IControl ClickAnyway()
        {
            DateTime start = DateTime.Now;
            Point? point = null;
            do
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
                Thread.Sleep(Settings.Default.Delay);
            } while (!AutomationElement.Current.HasKeyboardFocus
                        && (DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout);
            Logging.Click(SearchCriteria.Identifiers);
            return this;
        }

        public TextBox SetValue(string value)
        {
            WaitForEnabled();
            object o;
            if (AutomationElement.TryGetCurrentPattern(ValuePattern.Pattern, out o))
            {
                var pattern = (ValuePattern)o;
                if (!pattern.Current.IsReadOnly)
                {
                    pattern.SetValue(value);
                }
                Logging.Sent(value);
                return this;
            }
            throw new GeneralException(string.Format(Logging.Strings["UnsupportedPattern"],
                SearchCriteria.Identifiers,
                "Value Pattern"));
        }

        public string GetValue()
        {
            Object o;
            AutomationElement.TryGetCurrentPattern(ValuePattern.Pattern, out o);
            if (o != null)
            {
                return ((ValuePattern)o).Current.Value;
            }
            return null;
        }

        public TextBox ClearValue()
        {
            object o;
            if (AutomationElement.TryGetCurrentPattern(ValuePattern.Pattern, out o))
            {
                var pattern = (ValuePattern)o;
                if (!pattern.Current.IsReadOnly)
                {
                    pattern.SetValue("");
                }
            }
            else
                throw new GeneralException(string.Format(Logging.Strings["UnsupportedPattern"],
                    SearchCriteria.Identifiers,
                    "Value Pattern"));

            return this;
        }

        public TextBox Send(string value)
        {
            if (value == null)
                return this;

            WaitForEnabled();

            try
            {
                AutomationElement.SetFocus();
            }
            catch (InvalidOperationException)
            {
            }

            ClearValue();
            Keyboard.Instance.Send(value);
            Logging.Sent(value);
            return this;
        }
    }
}
