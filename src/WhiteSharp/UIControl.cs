﻿using System;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.WindowsAPI;
using TestStack.White.UIItems.ListBoxItems;

namespace WhiteSharp
{
    public class UIControl : UIItem
    {
        internal UIControl(AutomationElement element, ActionListener listener) : base(element, listener)
        {
        }

        internal string GetId()
        {
            string[] identifiers = new string[]
            {
                "AutomationId",
                "Name",
                "Control Type",
                "Class Name"
            };

            var PropertyList = new string[]
            {
                AutomationElement.Current.AutomationId,
                AutomationElement.Current.Name,
                AutomationElement.Current.ControlType.ToString(),
                AutomationElement.Current.ClassName
            };

            return identifiers[Array.FindIndex<string>(PropertyList, 0, x => !String.IsNullOrEmpty(x))] + " = " + 
            PropertyList.First(x => !String.IsNullOrEmpty(x));
        }

        public UIControl FindChild(Finder f)
        {
            f.Result = AutomationElement.FindAll(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
            return new UIControl(f.Result.First(), ActionListener);
        }

        /// <summary>
        /// Clicks without waiting for control to get enabled
        /// </summary>
        /// <returns></returns>
        public UIControl ClickAnyway()
        {
            if (AutomationElement.Current.ControlType.Equals(ControlType.Edit))
            {
                do
                {
                    Mouse.Instance.Click(ClickablePoint);
                    Thread.Sleep(Settings.Default.Delay);
                } while (!AutomationElement.Current.HasKeyboardFocus);
            }
            else
                Mouse.Instance.Click(ClickablePoint);
            Logging.Click(this);
            return this;
        }

        /// <summary>
        /// Waits for control to get enabled enabled, then clicks
        /// </summary>
        /// <returns></returns>
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
                Thread.Sleep(Settings.Default.Delay);
            } while (!AutomationElement.Current.IsEnabled && ((DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout));

            if (!AutomationElement.Current.IsEnabled)
                throw new ControlNotEnabledException(Logging.ControlException(GetId()));
        }

        public UIItem SelectItem(string name)
        {
            WaitForControlEnabled();
            if (AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
            {
                throw new GeneralException(string.Format(Strings.NotACombobox, this.GetId()));
            }
            TestStack.White.UIItems.ListBoxItems.ComboBox combobox = new TestStack.White.UIItems.ListBoxItems.ComboBox(AutomationElement, actionListener);
            combobox.Select(name);
            return this;
        }

        public UIItem SelectItem(int index)
        {
            WaitForControlEnabled();
            if (!AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
            {
                throw new GeneralException(string.Format(Strings.NotACombobox, this.GetId()));
            }
            TestStack.White.UIItems.ListBoxItems.ComboBox combobox = new TestStack.White.UIItems.ListBoxItems.ComboBox(AutomationElement, actionListener);
            combobox.Select(index);
            return this;
        }
    }
}