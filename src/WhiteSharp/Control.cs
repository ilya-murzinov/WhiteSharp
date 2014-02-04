using System;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using WhiteSharp.Extensions;
using WhiteSharp.Interfaces;
using ComboBox = TestStack.White.UIItems.ListBoxItems.ComboBox;

namespace WhiteSharp
{
    public class Control : Container, IControl
    {
        #region Properties

        //public Window Window { get; internal set; }

        public string Identifiers { get; internal set; }

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
            AutomationElement = element;
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
                throw new GeneralException(string.Format(Logging.Strings["UnsupportedPattern"], Identifiers, "Value Pattern"));

            return this;
        }

        public Control Send(string value)
        {
            ClearValue();
            Keyboard.Instance.Send(value);
            return this;
        }

        public Control Send(Keys key)
        {
            Keyboard.Instance.Send(key);
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