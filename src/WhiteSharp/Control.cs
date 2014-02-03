using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.WindowsAPI;
using WhiteSharp.Extensions;
using WhiteSharp.Interfaces;
using ComboBox = TestStack.White.UIItems.ListBoxItems.ComboBox;

namespace WhiteSharp
{
    public class Control : IControl
    {
        #region Private Fields
        private readonly AutomationElement _automationElement; 
        #endregion

        #region Properties
        public Window Window { get; internal set; }
        public string Identifiers { get; internal set; }

        public AutomationElement AutomationElement
        {
            get { return _automationElement; }
        }

        public bool Enabled
        {
            get { return AutomationElement.Current.IsEnabled; }
        }

        public string Name
        {
            get { return AutomationElement.Current.Name; }
        }

        internal Control(AutomationElement element)
        {
            _automationElement = element;
        } 
        #endregion

        #region Finders
        public Control FindChild(By searchCriteria, int index = 0)
        {
            List<AutomationElement> baseAutomationElementList = AutomationElement
                .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
            Control returnControl =
                new Control(Finder.Find(baseAutomationElementList, searchCriteria).ElementAt(index))
            {
                Identifiers = searchCriteria.Identifiers,
                Window = Window
            };

            Logging.ControlFound(searchCriteria);

            return returnControl;
        }

        public List<AutomationElement> FindChildren(By searchCriteria)
        {
            List<AutomationElement> baseAutomationElementList = AutomationElement
                .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
            return Finder.Find(baseAutomationElementList, searchCriteria);
        }

        public Control FindChild(string automationId)
        {
            return FindChild(By.AutomationId(automationId));
        }

        public Control FindChild(ControlType type)
        {
            return FindChild(By.ControlType(type));
        }

        public bool Exists(By searchCriteria)
        {
            try
            {
                if (AutomationElement.FindAll(TreeScope.Subtree,
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

        public bool Exists(string automationId)
        {
            return Exists(By.AutomationId(automationId));
        }

        public bool Exists(By searchCriteria, out object o)
        {
            DateTime start = DateTime.Now;
            o = null;

            while ((DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout)
            {
                try
                {
                    List<AutomationElement> elements = AutomationElement.FindAll(TreeScope.Subtree,
                        new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                        .OfType<AutomationElement>().ToList().FindAll(searchCriteria.Result);
                    if (elements.Count > 0)
                    {
                        o = elements.ElementAt(0);
                        return true;
                    }
                }
                catch (Exception)
                {
                }
            }
            return false;
        }

        public bool Exists(string automationId, out object o)
        {
            return Exists(By.AutomationId(automationId), out o);
        }
        #endregion

        #region Actions
        public Control ClickAnyway()
        {
            DateTime start = DateTime.Now;
            if (AutomationElement.Current.ControlType.Equals(ControlType.Edit))
            {
                do
                {
                    Mouse.Instance.Click(AutomationElement.GetClickablePoint());
                    Thread.Sleep(Settings.Default.Delay);
                } while (!AutomationElement.Current.HasKeyboardFocus
                         && (DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout);
            }
            else
                Mouse.Instance.Click(AutomationElement.GetClickablePoint());
            Logging.Click(Identifiers);
            return this;
        }

        public Control Click()
        {
            WaitForEnabled();
            return ClickAnyway();
        }

        public Control DoubleClick()
        {
            Mouse.Instance.DoubleClick(AutomationElement.GetClickablePoint());
            return this;
        }

        public Control ClickIfExists(By searchCriteria)
        {
            Control control = null;
            object o;

            if (Exists(searchCriteria, out o))
            {
                control = new Control((AutomationElement)o);
                control.Click();
            }

            return control;
        }

        public Control ClickIfExists(string automationId)
        {
            return ClickIfExists(By.AutomationId(automationId));
        }

        public Control ClearValue()
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

        public Control Send(string value)
        {
            Click();

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

        public Control Send(int value)
        {
            SendKeys.SendWait(value.ToString());
            return this;
        }

        public Control SelectItem(string name)
        {
            WaitForEnabled();
            if (!AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
                throw new GeneralException(string.Format(Logging.Strings["NotACombobox"], Identifiers));

            object o;

            if (AutomationElement.TryGetCurrentPattern(ValuePattern.Pattern, out o))
            {
                ValuePattern valuePattern = (ValuePattern)o;
                valuePattern.SetValue(name);
            }
            else
            {
                //TODO: replace this ro remove link to TestStack.White
                ComboBox comboBox = new ComboBox(AutomationElement, null);
                comboBox.Select(name);
            }

            Logging.ItemSelected(name, Identifiers);
            return this;
        }

        public Control SelectItem(int index)
        {
            WaitForEnabled();
            if (!AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
            {
                throw new GeneralException(string.Format(Logging.Strings["NotACombobox"], Identifiers));
            }
            var combobox = new ComboBox(AutomationElement, null);
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

        public Control SelectFirstItem()
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
                var p = (SelectionItemPattern)AutomationElement.GetCurrentPattern(SelectionItemPattern.Pattern);
                p.Select();
            }
            else
            {
                throw new GeneralException(string.Format(Logging.Strings["NotARadioButton"], Identifiers));
            }
        } 
        #endregion

        public Control WaitForEnabled()
        {
            DateTime start = DateTime.Now;
            int count = 0;

            while (count < 100 &&
                       (DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout)
            {
                while (!AutomationElement.Current.IsEnabled &&
                       (DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout)
                {
                    Thread.Sleep(Settings.Default.Delay);
                }

                if (AutomationElement.Current.IsEnabled)
                {
                    count++;
                }
            }

            if (!AutomationElement.Current.IsEnabled &&
                    (DateTime.Now - start).TotalMilliseconds > Settings.Default.Timeout)
                throw new ControlNotEnabledException(Logging.ControlNotEnabledException(Identifiers));
            return this;
        }

        public Control Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
            return this;
        }

        public string GetText()
        {
            return AutomationElement.GetText();
        }

        internal string GetId()
        {
            return AutomationElement.GetId();
        }

        public Control DrawHighlight()
        {
            new UIItem(AutomationElement, null).DrawHighlight();
            return this;
        }
    }
}