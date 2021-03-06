﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using Castle.Core.Internal;
using WhiteSharp.Drawing;

namespace WhiteSharp.Extensions
{
    public static class AutomationElementExtension
    {
        internal static bool IsOffScreen(this AutomationElement automationElement)
        {
            try
            {
                return automationElement.Current.IsOffscreen;
            }
            catch (ElementNotAvailableException)
            {
                return true;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
        }

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

        public static string GetName(this AutomationElement automationElement)
        {
            return automationElement.Current.Name;
        }

        public static string GetHelpText(this AutomationElement automationElement)
        {
            return automationElement.Current.HelpText;
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
                valuePattern = (ValuePattern)automationElement.GetCurrentPattern(ValuePattern.Pattern);
                value = valuePattern.Current.Value;
            }

            if (automationElement.Current.ControlType.Equals(ControlType.ComboBox))
            {
                object p;
                if (automationElement.TryGetCurrentPattern(SelectionPattern.Pattern, out p))
                {
                    selectionPattern = (SelectionPattern)p;
                    selectedItemName = selectionPattern.Current.GetSelection().First().Current.Name;
                }
                if (automationElement.TryGetCurrentPattern(ValuePattern.Pattern, out p))
                {
                    valuePattern = (ValuePattern)p;
                    value = valuePattern.Current.Value;
                }
            }

            var text = new[]
            {
                value,
                selectedItemName,
                name,
                helpText
            }.FirstOrDefault(x => !x.IsNullOrEmpty()) ?? "";

            return text;
        }

        public static string Title(this AutomationElement element)
        {
            string returnName;
            try
            {
                returnName = element.FindFirst(TreeScope.Children,
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TitleBar)).Current.Name;
            }
            catch (Exception)
            {
                return "";
            }
            return returnName;
        }

        public static void DrawHighlight(this AutomationElement element)
        {
            var rectangle = element.Current.BoundingRectangle;

            if (rectangle != Rect.Empty)
            {
                new FrameRectangle(rectangle).Highlight();
            }
        }
    }
}