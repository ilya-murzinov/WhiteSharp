using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Automation;
using WhiteSharp.Controls;
using WhiteSharp.Extensions;
using WhiteSharp.Factories;

namespace WhiteSharp
{
    public class Window : IControlContainer
    {
        #region Private Fields

        private AutomationElement _automationElement;
        private WindowVisualState _displayState;
        private WindowPattern _windowPattern;

        #endregion

        #region Properties

        internal WindowPattern WindowPattern
        {
            get
            {
                return IsOffScreen
                    ? _windowPattern
                    : (WindowPattern) AutomationElement.GetCurrentPattern(WindowPattern.Pattern);
            }
            private set { _windowPattern = value; }
        }

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
                    : new Window(Regex.Escape(Title)).AutomationElement;
            }
            protected set { _automationElement = value; }
        }

        public List<AutomationElement> BaseAutomationElementList { get; protected set; }

        public int ProcessId { get; private set; }

        public string Title { get; private set; }

        public WindowVisualState DisplayState
        {
            get { return _displayState; }
            set
            {
                if (_displayState != value)
                {
                    WindowPattern.SetWindowVisualState(value);
                    _displayState = value;
                }
            }
        }

        #endregion

        #region Constructors

        internal Window(AutomationElement element)
        {
            AutomationElement = element;
            WindowPattern = (WindowPattern) element.GetCurrentPattern(WindowPattern.Pattern);
            RefreshBaseList(AutomationElement);
            Title = element.Title();
            ProcessId = element.Current.ProcessId;
            _displayState = WindowPattern.Current.WindowVisualState;

            this.OnTop();
        }

        public Window(string title)
            : this(Desktop.Instance.FindWindow(title).AutomationElement)
        {
        }

        public Window(int processId, string title)
            : this(Desktop.Instance.FindWindow(processId, title).AutomationElement)
        {
        }

        #endregion

        #region Window Finders

        public Window FindModalWindow(string title)
        {
            return new Window(FindAll(By.ControlType(ControlType.Window).AndName(title)).First());
        }

        #endregion

        #region Control Finders

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

        private void RefreshBaseList(AutomationElement automationElement)
        {
            BaseAutomationElementList = automationElement
                .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
        }

        internal List<AutomationElement> Find(AutomationElement automationElement, By searchCriteria, int index)
        {
            DateTime start = DateTime.Now;

            var list = new List<AutomationElement>();
            AutomationElement element = null;

            while ((!list.Any() || element == null) &&
                   (DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout)
            {
                try
                {
                    list = BaseAutomationElementList.FindAll(searchCriteria.Result);
                    element = list.ElementAt(index);
                }
                catch (Exception)
                {
                    RefreshBaseList(IsOffScreen
                        ? new Window(Regex.Escape(Title)).AutomationElement
                        : AutomationElement);
                }
            }

            if (element == null)
                throw new ControlNotFoundException(
                    Logging.ControlNotFoundException(searchCriteria.Identifiers));

            searchCriteria.Duration = (DateTime.Now - start).TotalSeconds;

            return list;
        }

        #endregion

        #region Exists

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

            while ((DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout/10)
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

        #endregion

        #region Actions

        public void Send(Keys key)
        {
            Keyboard.Instance.Send(key);
        }

        public IControl ClickIfExists(By searchCriteria)
        {
            IControl control = null;
            object o;

            if (Exists(searchCriteria, out o))
            {
                control = ControlFactory.Create((AutomationElement) o, this, searchCriteria, 0);
                control.Click();
            }

            return control;
        }

        public IControl ClickIfExists(string automationId)
        {
            return ClickIfExists(By.AutomationId(automationId));
        }

        public void Close()
        {
            WindowPattern.Close();
            Logging.WindowClosed(Title);
        }

        #endregion
    }
}