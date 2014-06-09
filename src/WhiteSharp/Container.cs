using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Automation;
using WhiteSharp.Extensions;
using WhiteSharp.Factories;

namespace WhiteSharp
{
    public abstract class Container : IControlContainer
    {
        private AutomationElement _automationElement;

        public string WindowTitle { get; protected set; }

        public bool IsOffScreen
        {
            get { return _automationElement.IsOffScreen(); }
        }

        public AutomationElement AutomationElement
        {
            get
            {
                return (!_automationElement.IsOffScreen())
                    ? _automationElement
                    : new Window(Regex.Escape(WindowTitle)).AutomationElement;
            }
            protected set { _automationElement = value; }
        }

        public List<AutomationElement> BaseAutomationElementList { get; protected set; }

        public abstract List<AutomationElement> Find(AutomationElement automationElement, By searchCriteria, int index);

        protected void RefreshBaseList(AutomationElement automationElement)
        {
            BaseAutomationElementList = automationElement
                .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
        }

        public T FindControl<T>(By searchCriteria, int index = 0) where T : class, IControl
        {
            searchCriteria.Add(By.FromControlType(typeof(T)));
            List<AutomationElement> elements = Find(AutomationElement, searchCriteria, index);

            var returnControl =
                ControlFactory.Create<T>(elements.ElementAt(index), this, searchCriteria, index);

            Logging.ControlFound(searchCriteria);

            if (elements.Count() > 1)
                Logging.MutlipleControlsWarning(elements);

            return returnControl;
        }

        public T FindControl<T>(string automationId, int index = 0) where T : class, IControl
        {
            return FindControl<T>(By.AutomationId(automationId), index);
        }

        public T FindControl<T>(ControlType type) where T : class, IControl
        {
            return FindControl<T>(By.ControlType(type));
        }

        public T FindControl<T>(int index = 0) where T : class, IControl
        {
            return FindControl<T>(By.FromControlType(typeof(T)));
        }

        public IControl FindControl(By searchCriteria, int index = 0)
        {
            List<AutomationElement> elements = Find(AutomationElement, searchCriteria, index);
            return ControlFactory.Create(elements.ElementAt(index), this, searchCriteria, index);
        }

        public IControl FindControl(string automationId, int index = 0)
        {
            By searchCriteria = By.AutomationId(automationId);
            List<AutomationElement> elements = Find(AutomationElement, searchCriteria, index);
            return ControlFactory.Create(elements.ElementAt(index), this, searchCriteria, index);
        }

        public List<AutomationElement> FindAll(By searchCriteria)
        {
            return Find(AutomationElement, searchCriteria, 0);
        }

        public bool Exists(By searchCriteria)
        {
            DateTime start = DateTime.Now;

            while ((DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout)
            {
                try
                {
                    RefreshBaseList(AutomationElement);

                    List<AutomationElement> elements = BaseAutomationElementList.FindAll(searchCriteria.Result);
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

            while ((DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout / 10)
            {
                try
                {
                    RefreshBaseList(AutomationElement);

                    List<AutomationElement> elements = BaseAutomationElementList.FindAll(searchCriteria.Result);
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

        public IControl ClickIfExists(By searchCriteria)
        {
            IControl control = null;
            object o;

            if (Exists(searchCriteria, out o))
            {
                control = ControlFactory.Create((AutomationElement)o, this, searchCriteria, 0);
                control.Click();
            }

            return control;
        }

        public IControl ClickIfExists(string automationId)
        {
            return ClickIfExists(By.AutomationId(automationId));
        }
    }
}
