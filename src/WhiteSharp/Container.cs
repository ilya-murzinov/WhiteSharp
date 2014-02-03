using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using Castle.Core.Internal;
using TestStack.White.InputDevices;
using TestStack.White.WindowsAPI;
using WhiteSharp.Interfaces;
using WhiteSharp.Extensions;

namespace WhiteSharp
{
    public class Container : IContainer
    {
        public string Title { get; protected set; }
        public AutomationElement AutomationElement { get; protected set; }
        public List<AutomationElement> BaseAutomationElementList { get; protected set; }

        internal static List<AutomationElement> Find(List<AutomationElement> baseAutomationElementList, By searchCriteria)
        {
            List<AutomationElement> list;

            try
            {
                list = baseAutomationElementList.FindAll(searchCriteria.Result).ToList();
            }
            catch (ElementNotAvailableException)
            {
                return null;
            }

            return list;
        }

        public Control FindControl(By searchCriteria, int index = 0)
        {
            DateTime start = DateTime.Now;
            List<AutomationElement> element = Find(BaseAutomationElementList, searchCriteria);

            if (element == null || !element.Any())
            {
                BaseAutomationElementList = new Window(Title).AutomationElement
                    .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                    .OfType<AutomationElement>().ToList();
                element = Find(BaseAutomationElementList, searchCriteria);
            }

            Control returnControl = new Control(element.ElementAt(index))
            {
                Identifiers = searchCriteria.Identifiers,
                Window = new Window(this.AutomationElement)
            };

            searchCriteria.Duration = (DateTime.Now - start).TotalSeconds;

            if (element == null || !element.Any())
                throw new ControlNotFoundException(
                    Logging.ControlNotFoundException(searchCriteria.Identifiers));

            Logging.ControlFound(searchCriteria);
            if (element.Count() > 1)
                Logging.MutlipleControlsWarning(element);            

            return returnControl;
        }

        public Control FindControl(string automationId, int index = 0)
        {
            return FindControl(By.AutomationId(automationId), index);
        }

        public Control FindControl(ControlType type)
        {
            return FindControl(By.ControlType(type));
        }

        public List<AutomationElement> FindAll(By searchCriteria)
        {
            return Find(BaseAutomationElementList, searchCriteria);
        }

        public bool Exists(By searchCriteria)
        {
            DateTime start = DateTime.Now;

            while ((DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout)
            {
                try
                {
                    List<AutomationElement> elements = AutomationElement.FindAll(TreeScope.Subtree,
                        new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                        .OfType<AutomationElement>().ToList().FindAll(searchCriteria.Result);
                    if (elements.Count > 0)
                    {
                        return true;
                    }
                    return false;

                }
                catch (Exception)
                {
                }
            }
            return false;
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
    }
}
