using System;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.WindowsAPI;

namespace WhiteSharp
{
    public class UIControl : UIItem
    {
        public UIControl(AutomationElement element, ActionListener listener) : base(element, listener)
        {
        }

        public string GetId()
        {
            return new[]
            {
                AutomationElement.Current.AutomationId,
                AutomationElement.Current.Name,
                AutomationElement.Current.ControlType.ToString(),
                AutomationElement.Current.ClassName
            }.First(x => !String.IsNullOrEmpty(x));
        }

        public UIControl FindChild(Finder f)
        {
            f.Result = AutomationElement.FindAll(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
            return new UIControl(f.Result.First(), ActionListener);
        }

        public UIControl ClickAnyway()
        {
            WaitForControlEnabled();
            if (AutomationElement.Current.ControlType.Equals(ControlType.Edit))
            {
                do
                {
                    Mouse.Instance.Click(ClickablePoint);
                    Thread.Sleep(Config.Delay);
                } while (!AutomationElement.Current.HasKeyboardFocus);
            }
            else
                Mouse.Instance.Click(ClickablePoint);
            Logging.Click(this);
            return this;
        }

        public new UIControl Click()
        {
            WaitForControlEnabled();
            return this.ClickAnyway();
        }

        public UIControl Send(string value)
        {
            switch (value)
            {
                case "{F5}":
                {
                    Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.F5);
                    break;
                }
                case "{Tab}":
                {
                    Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
                    break;
                }
                case "{Esc}":
                {
                    Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.ESCAPE);
                    break;
                }
                case "{Enter}":
                {
                    Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
                    break;
                }
                default:
                {
                    SendKeys.SendWait(value);
                    break;
                }
            }
            Logging.Sent(value);
            return this;
        }

        public UIControl Send(int value)
        {
            SendKeys.SendWait(value.ToString());
            return this;
        }

        public void WaitForControlEnabled()
        {
            DateTime start = DateTime.Now;
            do
            {
                Thread.Sleep(Config.Delay);
            } while (!AutomationElement.Current.IsEnabled && ((DateTime.Now - start).TotalMilliseconds < Config.Timeout));

            if (!AutomationElement.Current.IsEnabled)
                throw new ControlNotEnabledException(Logging.ControlException(GetId()));
        }

        //public void SelectItem(string name)
        //{
        //    if (UIItem.AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
        //    {
        //        throw new WhiteException(string.Format("Элемент {0} не является комбобоксом"));
        //    }
        //    if (true)
        //    {
        //        WaitForControlEnabled();
        //        UIItem.SetValue(name);
        //    }
        //    else
        //    {
        //        Click();
        //        SendKeys.SendWait(name);
        //        SendKeys.SendWait("{Down}");
        //        Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
        //        Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
        //    }
        //}
    }
}