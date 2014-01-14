using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.WindowsAPI;
using WhiteSharp.Extensions;
using WhiteSharp.Interfaces;
using ComboBox = TestStack.White.UIItems.ListBoxItems.ComboBox;

namespace WhiteSharp
{
    public class UIControl : UIItem, IUIControl
    {
        public UIWindow Window { get; internal set; }

        internal UIControl(AutomationElement element, ActionListener listener) : base(element, listener)
        {
        }

        public string GetId()
        {
            return AutomationElement.GetId();
        }

        public UIControl FindChild(Finder f)
        {
            List<AutomationElement> container = AutomationElement.FindAll(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
            List<AutomationElement> list = f.Result.Where(container.Contains).ToList();

            Logging.ControlFound(f);
            if (list.Count() > 1)
                Logging.MutlipleControlsWarning(f.Result.Where(container.Contains).ToList());

            return new UIControl(list.First(), actionListener);
        }

        public UIControl FindChild(string automationId)
        {
            var f = new Finder
            {
                Result = AutomationElement.FindAll(TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                    .OfType<AutomationElement>().ToList()
            };
            return new UIControl(f.AutomationId(automationId).Result.First(), ActionListener);
        }

        /// <summary>
        ///     Clicks without waiting for control to get enabled
        /// </summary>
        /// <returns></returns>
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
            Logging.Click(this);
            return this;
        }

        /// <summary>
        ///     Waits for control to get enabled enabled, then clicks
        /// </summary>
        /// <returns></returns>
        public new UIControl Click()
        {
            WaitForControlEnabled();
            return ClickAnyway();
        }

        /// <summary>
        /// Supported shortcuts: {F5}, {Tab}, {Esc}, {Enter}.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns text of UIControl
        /// </summary>
        /// <returns></returns>
        public string GetText()
        {
            return AutomationElement.GetText();
        }

        public UIControl WaitForControlEnabled()
        {
            DateTime start = DateTime.Now;
            do
            {
                Thread.Sleep(Settings.Default.Delay);
            } while (!AutomationElement.Current.IsEnabled &&
                     ((DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout));

            if (!AutomationElement.Current.IsEnabled)
                throw new ControlNotEnabledException(Logging.ControlException(GetId()));
            return this;
        }

        public void SelectItem(string name)
        {
            WaitForControlEnabled();
            if (!AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
            {
                throw new GeneralException(string.Format(Logging.Strings["NotACombobox"], GetId()));
            }
            var combobox = new ComboBox(AutomationElement, actionListener);
            combobox.Select(name);
        }

        public void SelectItem(int index)
        {
            WaitForControlEnabled();
            if (!AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
            {
                throw new GeneralException(string.Format(Logging.Strings["NotACombobox"], GetId()));
            }
            var combobox = new ComboBox(AutomationElement, actionListener);
            combobox.Select(index);
        }

        public UIItem SelectItemTree(string name)
        {
            WaitForControlEnabled();
            if (!AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
            {
                throw new GeneralException(string.Format(Logging.Strings["NotACombobox"], GetId()));
            }
            var combobox = new ComboBox(AutomationElement, actionListener);
            var p = (ValuePattern)combobox.AutomationElement.GetCurrentPattern(ValuePattern.Pattern);
            p.SetValue(name);
            return this;
        }

        public new UIControl DrawHighlight()
        {
            base.DrawHighlight();
            return this;
        }
    }
}