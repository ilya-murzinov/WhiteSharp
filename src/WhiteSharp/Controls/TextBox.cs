using System;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using TestStack.White.InputDevices;
using TestStack.White.UIItems.Actions;

namespace WhiteSharp.Controls
{
    public class TextBox : Control
    {
        public TextBox(AutomationElement automationElement, IControlContainer window, By searchCriteria, int index)
            : base(automationElement, window, searchCriteria, index)
        {
        }

        public override IControl ClickAnyway(bool doCheckErrorWindow = false)
        {
            var start = DateTime.Now;
            Point? point = null;
            do
            {
                try
                {
                    point = AutomationElement.GetClickablePoint();
                }
                catch (NoClickablePointException)
                {
                    ((Window)Window).OnTop();
                }
                if (point != null)
                {
                    Mouse.Instance.Click(new Point(point.Value.X, point.Value.Y));
                }
                Thread.Sleep(Settings.Default.Delay);
            } while (!AutomationElement.Current.HasKeyboardFocus
                     && (DateTime.Now - start).TotalMilliseconds < 10000);
            Logging.Click(SearchCriteria.Identifiers);
            return this;
        }

        public new TextBox Send(string value)
        {
            WaitForEnabled();
            ClearValueWithNoException();
            if (value == null)
            {
                return this;
            }
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
            Click();
            TestStack.White.InputDevices.Keyboard.Instance.Send(value, new NullActionListener());
            TestStack.White.InputDevices.Keyboard.Instance.LeaveAllKeys();
            return this;
        }

        public TextBox SendWithoutValuePattern(string value)
        {
            if (value == null)
            {
                return this;
            }
            ClickAnyway().Wait(1000);
            Send(Keys.CtrlA);
            TestStack.White.InputDevices.Keyboard.Instance.Send(value, new NullActionListener());
            TestStack.White.InputDevices.Keyboard.Instance.LeaveAllKeys();
            Send(Keys.Tab);
            Thread.Sleep(500);
            return this;
        }

        public TextBox SendManual(string value)
        {
            WaitForEnabled();
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds < 10)
            {
                ClearValue();
                DoubleClick().Wait(1000);
                foreach (var letter in value.ToCharArray())
                {
                    switch (letter)
                    {
                        case '0':
                            Send(Keys.Zero);
                            Thread.Sleep(100);
                            break;
                        case '1':
                            Send(Keys.One);
                            Thread.Sleep(100);
                            break;
                        case '2':
                            Send(Keys.Two);
                            Thread.Sleep(100);
                            break;
                        case '3':
                            Send(Keys.Three);
                            Thread.Sleep(100);
                            break;
                        case '4':
                            Send(Keys.Four);
                            Thread.Sleep(100);
                            break;
                        case '5':
                            Send(Keys.Five);
                            Thread.Sleep(100);
                            break;
                        case '6':
                            Send(Keys.Six);
                            Thread.Sleep(100);
                            break;
                        case '7':
                            Send(Keys.Seven);
                            Thread.Sleep(100);
                            break;
                        case '8':
                            Send(Keys.Eight);
                            Thread.Sleep(100);
                            break;
                        case '9':
                            Send(Keys.Nine);
                            Thread.Sleep(100);
                            break;
                    }
                }
                Send(Keys.Tab);
                Keyboard.Instance.LeaveAllKeys();
                Thread.Sleep(1000);
                if (GetValue() == null || !GetValue().Equals(value)) continue;
                Send(Keys.Tab);
                Keyboard.Instance.LeaveAllKeys();
                return this;
            }
            return this;
        }

        public string GetValue()
        {
            Object o;
            AutomationElement.TryGetCurrentPattern(ValuePattern.Pattern, out o);
            return o != null ? ((ValuePattern)o).Current.Value : null;
        }

        public TextBox ClearValueWithNoException()
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
            {
                Click();
                Send(Keys.CtrlA);
                Thread.Sleep(500);
                Send(Keys.Del);
            }
            Keyboard.Instance.LeaveAllKeys();
            return this;
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
    }
}