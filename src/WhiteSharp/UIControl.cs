using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.WindowsAPI;
using WhiteSharp.Extensions;
using WhiteSharp.Interfaces;
using ComboBox = TestStack.White.UIItems.ListBoxItems.ComboBox;

namespace WhiteSharp
{
    public class UIControl : UIItem, IUIControl
    {
        public UIWindow Window { get; internal set; }
        internal string Identifiers { get; set; }

        internal UIControl(AutomationElement element, ActionListener listener) : base(element, listener)
        {
        }

        public string GetId()
        {
            return AutomationElement.GetId();
        }

        public UIControl FindChild(By searchCriteria, int index = 0)
        {
            DateTime start = DateTime.Now;
            List<AutomationElement> baseList = AutomationElement.FindAll(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.IsOffscreenProperty, false)).OfType<AutomationElement>().ToList();
            List<AutomationElement> result = Finder.Find(baseList, searchCriteria);
            UIControl returnControl = new UIControl(result.ElementAt(index),
                actionListener)
            {
                Identifiers = searchCriteria.Identifiers,
                Window = Window
            };

            Logging.ControlFound(searchCriteria.Identifiers, Window.Title, DateTime.Now - start);

            return returnControl;
        }

        public UIControl FindChild(string automationId)
        {
            return FindChild(By.AutomationId(automationId));
        }

        public UIControl FindChild(ControlType type)
        {
            return FindChild(By.ControlType(type));
        }

        public UIControl ClickAnyway()
        {
            DateTime start = DateTime.Now;
            if (AutomationElement.Current.ControlType.Equals(ControlType.Edit))
            {
                do
                {
                    Mouse.Instance.Click(ClickablePoint);
                    Thread.Sleep(Settings.Default.Delay);
                } while (!AutomationElement.Current.HasKeyboardFocus
                         && (DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout);
            }
            else
                Mouse.Instance.Click(ClickablePoint);
            Logging.Click(Identifiers);
            return this;
        }

        public new UIControl Click()
        {
            WaitForControlEnabled();
            return ClickAnyway();
        }

        public UIControl Send(string value)
        {
            WaitForControlEnabled();
            DateTime start = DateTime.Now;
            while (!AutomationElement.Current.HasKeyboardFocus &&
                   (DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout)
                Focus();

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
                case "{Down}":
                {
                    Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
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

        public string GetText()
        {
            return AutomationElement.GetText();
        }

        public UIControl WaitForControlEnabled()
        {
            DateTime start = DateTime.Now;

            while (!AutomationElement.Current.IsEnabled && (DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout)
            {
                Trace.WriteLine(AutomationElement.Current.AutomationId + " - " + Enabled);
                Thread.Sleep(Settings.Default.Delay);
            }

            if (!AutomationElement.Current.IsEnabled)
                throw new ControlNotEnabledException(Logging.ControlNotEnabledException(Identifiers));
            return this;
        }

        public void SelectItem(string name)
        {
            WaitForControlEnabled();
            if (!AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
            {
                throw new GeneralException(string.Format(Logging.Strings["NotACombobox"], GetId()));
            }
            var p = (ValuePattern) AutomationElement.GetCurrentPattern(ValuePattern.Pattern);
            p.SetValue(name);
            Logging.ItemSelected(name, Identifiers);
        }

        public void SelectItem(int index)
        {
            WaitForControlEnabled();
            if (!AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
            {
                throw new GeneralException(string.Format(Logging.Strings["NotACombobox"], GetId()));
            }
            var combobox = new ComboBox(AutomationElement, actionListener);
            try
            {
                combobox.Select(index);
            }
            catch (Exception e)
            {
                Logging.Exception(e);
            }
            Logging.ItemSelected(index.ToString(), Identifiers);
        }

        public new UIControl DrawHighlight()
        {
            base.DrawHighlight();
            return this;
        }
    }
}