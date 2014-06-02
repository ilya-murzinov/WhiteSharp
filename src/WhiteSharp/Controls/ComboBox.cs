using System;
using System.Windows.Automation;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.ListBoxItems;

namespace WhiteSharp.Controls
{
    public class Combobox : Control
    {
        public Combobox(AutomationElement automationElement, Window window, By searchCriteria, int index) 
            : base(automationElement, window, searchCriteria, index)
        {
        }

        public Control SelectItem(string name)
        {
            if (name == null)
                return this;

            WaitForEnabled();
            if (!AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
                throw new GeneralException(string.Format(Logging.Strings["NotACombobox"], SearchCriteria.Identifiers));

            object o;

            if (AutomationElement.TryGetCurrentPattern(ValuePattern.Pattern, out o))
            {
                var valuePattern = (ValuePattern)o;
                valuePattern.SetValue(name);
            }
            else
            {
                //TODO: replace this ro remove link to TestStack.White
                var comboBox = new ComboBox(AutomationElement, new NullActionListener());
                comboBox.Select(name);
            }

            Logging.ItemSelected(name, SearchCriteria.Identifiers);
            return this;
        }

        public Control SelectItem(int index)
        {
            WaitForEnabled();
            if (!AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
            {
                throw new GeneralException(string.Format(Logging.Strings["NotACombobox"], SearchCriteria.Identifiers));
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
            Logging.ItemSelected(index.ToString(), SearchCriteria.Identifiers);
            return this;
        }
    }
}
