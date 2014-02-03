using System;
using System.Collections.Generic;
using System.Linq;
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
        public string Identifiers { get; set; }

        internal UIControl(AutomationElement element, ActionListener listener) : base(element, listener)
        {
        }

        public UIControl(AutomationElement element)
        {            
        }

        internal string GetId()
        {
            return AutomationElement.GetId();
        }

        public UIControl FindChild(By searchCriteria, int index = 0)
        {
            List<AutomationElement> baseAutomationElementList = automationElement
                .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
            UIControl returnControl =
                new UIControl(Finder.Find(baseAutomationElementList, searchCriteria).ElementAt(index),
                actionListener)
            {
                Identifiers = searchCriteria.Identifiers,
                Window = Window
            };

            Logging.ControlFound(searchCriteria);

            return returnControl;
        }

        public List<AutomationElement> FindChildren(By searchCriteria)
        {
            List<AutomationElement> baseAutomationElementList = automationElement
                .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
            return Finder.Find(baseAutomationElementList, searchCriteria);
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
            WaitForEnabled();
            return ClickAnyway();
        }


        public bool Exists(By searchCriteria)
        {
            try
            {
                if (AutomationElement.FindAll(TreeScope.Children,
                    new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                    .OfType<AutomationElement>().ToList().FindAll(searchCriteria.Result).Count > 0)
                    return true;
                return false;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public UIControl ClearValue()
        {
            object o;
            if (AutomationElement.TryGetCurrentPattern(ValuePattern.Pattern, out o))
            {
                ValuePattern pattern = (ValuePattern)o;
                if (!pattern.Current.IsReadOnly)
                {
                    pattern.SetValue("");
                }
            }
            else
                throw new GeneralException(string.Format(Logging.Strings["UnsupportedPattern"], "Value Pattern"));

            return this;
        }

        public UIControl Send(string value)
        {
            WaitForEnabled();
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
                case "{Del}":
                {
                    Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.DELETE);
                    break;
                }
                case "^{a}":
                {
                    Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    Keyboard.Instance.Enter("a");
                    Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                    break;
                }
                default:
                {
                    ClearValue();
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

        public UIControl WaitForEnabled()
        {
            DateTime start = DateTime.Now;
            int count = 0;

            while (count < 100 &&
                       (DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout)
            {
                while (!Enabled &&
                       (DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout)
                {
                    Thread.Sleep(Settings.Default.Delay);
                }

                if (Enabled)
                {
                    count++;
                }
            }

            if (!Enabled &&
                    (DateTime.Now - start).TotalMilliseconds > Settings.Default.Timeout)
                throw new ControlNotEnabledException(Logging.ControlNotEnabledException(Identifiers));
            return this;
        }

        public UIControl Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
            return this;
        }

        public UIControl SelectItem(string name)
        {
            WaitForEnabled();
            if (!AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
                throw new GeneralException(string.Format(Logging.Strings["NotACombobox"], Identifiers));

            object o;

            if (AutomationElement.TryGetCurrentPattern(ValuePattern.Pattern, out o))
            {
                ValuePattern valuePattern = (ValuePattern) o;
                valuePattern.SetValue(name);
            }
            else
            {
                ComboBox comboBox = new ComboBox(AutomationElement, ActionListener);
                comboBox.Select(name);
            }

            Logging.ItemSelected(name, Identifiers);
            return this;
        }

        public UIControl SelectItem(int index)
        {
            WaitForEnabled();
            if (!AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
            {
                throw new GeneralException(string.Format(Logging.Strings["NotACombobox"], Identifiers));
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
            return this;
        }

        public UIControl SelectFirstItem()
        {
            return SelectItem("");
        }

        public void SetToggleState(ToggleState state)
        {
            object objPat;
            if (AutomationElement.TryGetCurrentPattern(TogglePattern.Pattern, out objPat))
            {
                TogglePattern togglePattern = (TogglePattern)objPat;
                if (togglePattern.Current.ToggleState != state)
                {
                    togglePattern.Toggle();
                }
            }
            else
            {
                throw new GeneralException(string.Format(Logging.Strings["UnsupportedPattern"], Identifiers, "TogglePattern"));
            }
        }

        public void SelectRadioButton()
        {
            if (AutomationElement.Current.ControlType.Equals(ControlType.RadioButton))
            {
                var p = (SelectionItemPattern) AutomationElement.GetCurrentPattern(SelectionItemPattern.Pattern);
                p.Select();
            }
            else
            {
                throw new GeneralException(string.Format(Logging.Strings["NotARadioButton"], Identifiers));
            }
        }

        public new UIControl DrawHighlight()
        {
            base.DrawHighlight();
            return this;
        }
    }
}