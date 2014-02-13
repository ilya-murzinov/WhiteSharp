using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using Castle.Core.Internal;
using WhiteSharp.Extensions;
using WhiteSharp.Interfaces;

namespace WhiteSharp
{
    public class Window : IWindow
    {
        #region Private Fields

        private static DateTime _start;
        private static List<string> _identifiers = new List<string>();
        private AutomationElement _automationElement;
        private WindowVisualState _displayState;
        private WindowPattern _windowPattern;

        #endregion

        #region Properties

        public AutomationElement AutomationElement
        {
            get
            {
                return (!_automationElement.IsOffScreen())
                    ? _automationElement
                    : new Window(Title).AutomationElement;
            }
            protected set { _automationElement = value; }
        }

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

        internal static string Identifiers
        {
            get { return _identifiers.Aggregate((x, y) => x + ", " + y); }
        }

        public bool IsOffScreen
        {
            get { return _automationElement.IsOffScreen(); }
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

        public Window(params string[] titles)
            : this(SelectWindow(FindWindows(titles)))
        {
        }

        public Window(Predicate<AutomationElement> p)
            : this(SelectWindow(FindWindows(p)))
        {
        }

        public Window(int processId)
            : this(SelectWindow(FindWindows(processId)))
        {
        }

        public Window(int processId, Predicate<AutomationElement> p)
            : this(SelectWindow(FindWindows(processId, p)))
        {
        }

        #endregion

        #region Window Finders

        private static List<AutomationElement> FindWindows(Predicate<AutomationElement> predicate)
        {
            var windows = new List<AutomationElement>();

            _start = DateTime.Now;

            while (!windows.Any() && ((DateTime.Now - _start).TotalMilliseconds < Settings.Default.Timeout))
            {
                try
                {
                    windows = Desktop.Instance.Windows.FindAll(predicate);
                }
                catch (Exception e)
                {
                }
                Thread.Sleep(Settings.Default.Delay);
            }
            if (!windows.Any())
            {
                Exception e = new WindowNotFoundException(string.Format(Logging.Strings["WindowException"], Identifiers));
                Logging.Exception(e);
                throw e;
            }
            return windows;
        }

        private static List<AutomationElement> FindWindows(params string[] titles)
        {
            _identifiers = new List<string>();
            var windows = new List<AutomationElement>();
            _start = DateTime.Now;
            titles.ForEach(x => _identifiers.Add(x));
            windows = FindWindows(window => titles.Select(x => window.Title().Contains(x)).All(x => x.Equals(true)));
            return windows;
        }

        private static List<AutomationElement> FindWindows(int processId)
        {
            _identifiers = new List<string>();
            _start = DateTime.Now;
            try
            {
                Process.GetProcessById(processId);
            }
            catch (ArgumentException)
            {
                throw new GeneralException(Logging.Strings["ProcessNotFound"]);
            }
            _identifiers.Add(processId.ToString());

            return FindWindows(window => window.Current.ProcessId.Equals(processId));
        }

        private static List<AutomationElement> FindWindows(int processId, Predicate<AutomationElement> p)
        {
            _identifiers = new List<string>();
            List<AutomationElement> windows = null;
            _start = DateTime.Now;

            while ((windows == null ||
                    !windows.Any()) && ((DateTime.Now - _start).TotalMilliseconds < Settings.Default.Timeout))
            {
                try
                {
                    windows = FindWindows(p).Where(x => x.Current.ProcessId.Equals(processId)).ToList();
                }
                catch (Exception e)
                {
                    Logging.Exception(e);
                }
                Thread.Sleep(Settings.Default.Delay);
            }

            _identifiers.Add(processId.ToString());
            _identifiers.Add(p.ToString());

            return windows;
        }

        private static AutomationElement SelectWindow(List<AutomationElement> windows)
        {
            if (windows == null || !windows.Any())
                throw new WindowNotFoundException(
                    Logging.WindowException(_identifiers.Aggregate((x, y) => x + ", " + y)));


            AutomationElement returnWindow = windows.First();

            Logging.WindowFound(returnWindow.Title(), DateTime.Now - _start);

            if (windows.Count > 1)
                Logging.MutlipleWindowsWarning(windows.Count);

            return returnWindow;
        }

        #endregion

        #region Control Finders

        public Control FindControl(By searchCriteria, int index = 0)
        {
            List<AutomationElement> elements = Find(AutomationElement, searchCriteria, index);

            var returnControl = new Control(elements.ElementAt(index))
            {
                Identifiers = searchCriteria.Identifiers,
                Window = this
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
                        ? new Window(Title).AutomationElement 
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

            while ((DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout)
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

        public Control ClickIfExists(By searchCriteria)
        {
            Control control = null;
            object o;

            if (Exists(searchCriteria, out o))
            {
                control = new Control((AutomationElement) o);
                control.Click();
            }

            return control;
        }

        public Control ClickIfExists(string automationId)
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