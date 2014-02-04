using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using WhiteSharp.Interfaces;

namespace WhiteSharp
{
    public class Container : IContainer
    {
        public string Title { get; protected set; }
        public AutomationElement AutomationElement { get; protected set; }
        public List<AutomationElement> BaseAutomationElementList { get; protected set; }

        internal List<AutomationElement> Find(AutomationElement automationElement, By searchCriteria)
        {
            DateTime start = DateTime.Now;

            if (BaseAutomationElementList == null || !BaseAutomationElementList.Any())
                BaseAutomationElementList = automationElement
                    .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                    .OfType<AutomationElement>().ToList();

            List<AutomationElement> list = new List<AutomationElement>();

            while (!list.Any() && (DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout) 
            {
                try
                {
                    list = BaseAutomationElementList.FindAll(searchCriteria.Result).ToList();
                }
                catch (Exception e)
                {
                    Logging.Exception(e);
                    BaseAutomationElementList = new Window(Title).AutomationElement
                        .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                        .OfType<AutomationElement>().ToList();
                } 
            }
            if (!list.Any())
                throw new ControlNotFoundException(
                    Logging.ControlNotFoundException(searchCriteria.Identifiers));

            searchCriteria.Duration = (DateTime.Now - start).TotalSeconds;

            return list;
        }

        public Control FindControl(By searchCriteria, int index = 0)
        {
            List<AutomationElement> elements = Find(AutomationElement, searchCriteria);
            
            Control returnControl = new Control(elements.ElementAt(index))
            {
                Identifiers = searchCriteria.Identifiers
            };
            
            Logging.ControlFound(searchCriteria);

            if (elements.Count() > 1)
                Logging.MutlipleControlsWarning(elements);

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
            return Find(AutomationElement, searchCriteria);
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
