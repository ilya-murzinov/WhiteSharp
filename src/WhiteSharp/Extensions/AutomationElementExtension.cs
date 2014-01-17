using System;
using System.Linq;
using System.Windows.Automation;
using Castle.Core.Internal;

namespace WhiteSharp.Extensions
{
    public static class AutomationElementExtension
    {
        internal static string GetId(this AutomationElement automationElement)
        {
            string[] identifiers =
            {
                "AutomationId",
                "Name",
                "Control Type",
                "Class Name"
            };

            var propertyList = new[]
            {
                automationElement.Current.AutomationId,
                automationElement.Current.Name,
                automationElement.Current.ControlType.ToString(),
                automationElement.Current.ClassName
            };

            return identifiers[Array.FindIndex(propertyList, 0, x => !String.IsNullOrEmpty(x))] + " = " +
                   propertyList.First(x => !String.IsNullOrEmpty(x));
        }

        public static string GetText(this AutomationElement automationElement)
        {
            SelectionPattern selectionPattern;
            ValuePattern valuePattern;

            string value = null;
            string selectedItemName = null;
            string name = automationElement.Current.Name;
            string helpText = automationElement.Current.HelpText;

            if (automationElement.Current.ControlType.Equals(ControlType.Edit))
            {
                valuePattern = (ValuePattern) automationElement.GetCurrentPattern(ValuePattern.Pattern);
                value = valuePattern.Current.Value;
            }

            if (automationElement.Current.ControlType.Equals(ControlType.ComboBox))
            {
                object p;
                if (automationElement.TryGetCurrentPattern(SelectionPattern.Pattern, out p))
                {
                    selectionPattern = (SelectionPattern) p;
                    selectedItemName = selectionPattern.Current.GetSelection().First().Current.Name;
                }
                if (automationElement.TryGetCurrentPattern(ValuePattern.Pattern, out p))
                {
                    valuePattern = (ValuePattern) p;
                    value = valuePattern.Current.Value;
                }
            }

            string text = new[]
            {
                value,
                selectedItemName,
                name,
                helpText
            }.FirstOrDefault(x => !x.IsNullOrEmpty()) ?? "";

            return text;
        }
    }
}