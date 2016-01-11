using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using Shouldly;
using WhiteSharp.Extensions;
using WhiteSharp.Factories;
using ComboBox = WhiteSharp.Controls.ComboBox;
using TextBox = WhiteSharp.Controls.TextBox;

namespace WhiteSharp
{
    public abstract class Container : IControlContainer
    {
        protected AutomationElement AutomationElementField;

        public string WindowTitle { get; protected set; }

        public bool IsOffScreen
        {
            get { return AutomationElementField.IsOffScreen(); }
        }

        public List<AutomationElement> BaseAutomationElementList { get; protected set; }
        public abstract AutomationElement AutomationElement { get; protected set; }

        public T FindControl<T>(By searchCriteria, int index = 0) where T : class, IControl
        {
            var elements = Find(AutomationElement, searchCriteria, index);
            var returnControl =
                ControlFactory.Create<T>(elements.ElementAt(index), this, searchCriteria, index);

            Logging.ControlFound(searchCriteria);

            if (elements.Count() > 1)
                Logging.MutlipleControlsWarning(elements);

            return returnControl;
        }

        public List<T> FindControls<T>(By searchCriteria, int index = 0) where T : class, IControl
        {
            var elements = Find(AutomationElement, searchCriteria, index);
            var res = elements.Select((t, i) => ControlFactory.Create<T>(elements.ElementAt(i), this, searchCriteria, i)).ToList();

            Logging.ControlFound(searchCriteria);

            if (elements.Count() > 1)
                Logging.MutlipleControlsWarning(elements);

            return res;
        }

        public T FindLastControl<T>(By searchCriteria, int index = 0) where T : class, IControl
        {
            var elements = Find(AutomationElement, searchCriteria, index);
            var count = elements.Count;
            if (count > 1)
                Logging.MutlipleControlsWarning(elements);

            var returnControl =
                ControlFactory.Create<T>(elements.ElementAt(count - 1), this, searchCriteria, count - 1);

            Logging.ControlFound(searchCriteria);

            return returnControl;
        }
        
        public bool TryTextBoxSend(By searchCriteria, string value, int index = 0)
        {
            if (!Exists(searchCriteria)) return false;
            var control = FindControl<TextBox>(searchCriteria, index);
            control.Send(value);
            return true;
        }

        public bool TryComboboxSelect(By searchCriteria, string value, int index = 0)
        {
            if (!Exists(searchCriteria)) return false;
            var control = FindControl<ComboBox>(searchCriteria, index);
            control.SelectItem(value);
            return true;
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
            var elements = Find(AutomationElement, searchCriteria, index);
            return ControlFactory.Create(elements.ElementAt(index), this, searchCriteria, index);
        }

        public IControl FindControl(string automationId, int index = 0)
        {
            var searchCriteria = By.AutomationId(automationId);
            var elements = Find(AutomationElement, searchCriteria, index);
            return ControlFactory.Create(elements.ElementAt(index), this, searchCriteria, index);
        }
        
        public List<AutomationElement> FindAll(By searchCriteria)
        {
            return Find(AutomationElement, searchCriteria, 0);
        }

        public bool Exists(By searchCriteria)
        {
            var start = DateTime.Now;

            while ((DateTime.Now - start).TotalMilliseconds * 5 < Settings.Default.Timeout)
            {
                try
                {
                    RefreshBaseList(AutomationElement);
                    var elements = BaseAutomationElementList.FindAll(searchCriteria.Result);
                    return elements.Count > 0;
                }
                catch (Exception)
                {
                }
            }
            return false;
        }

        public void ShouldExist(By searchCriteria)
        {
            Exists(searchCriteria).ShouldBe(true);
        }

        public void ShouldNotExist(By searchCriteria)
        {
            Exists(searchCriteria).ShouldBe(false);
        }

        public bool Exists(string automationId)
        {
            return Exists(By.AutomationId(automationId));
        }

        public bool Exists(By searchCriteria, out object o)
        {
            var start = DateTime.Now;
            o = null;

            while ((DateTime.Now - start).TotalMilliseconds * 10 < Settings.Default.Timeout)
            {
                try
                {
                    RefreshBaseList(AutomationElement);

                    var elements = BaseAutomationElementList.FindAll(searchCriteria.Result);
                    if (elements.Count <= 0) continue;
                    o = elements.ElementAtOrDefault(0);
                    return true;
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
                control.Click().Wait(300);
            }

            return control;
        }

        public IControl ClickIfExists(string automationId)
        {
            return ClickIfExists(By.AutomationId(automationId));
        }

        public abstract List<AutomationElement> Find(AutomationElement automationElement, By searchCriteria, int index);

        protected void RefreshBaseList(AutomationElement automationElement)
        {
            BaseAutomationElementList = automationElement
                .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
        }
    }
}