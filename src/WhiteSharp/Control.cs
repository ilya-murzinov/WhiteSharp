using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using TestStack.White.InputDevices;
using WhiteSharp.Extensions;
using WhiteSharp.Factories;

namespace WhiteSharp
{
    public class Control : IControl
    {
        #region Private Fields

        private AutomationElement _automationElement;
        private bool _clicked = false;

        #endregion

        #region Properties

        public List<AutomationElement> BaseAutomationElementList { get; protected set; }

        internal Window Window { get; set; }

        public String WindowTitle { get; internal set; }

        internal By SearchCriteria { get; set; }

        internal int Index { get; set; }

        public bool IsOffScreen
        {
            get { return _automationElement.IsOffScreen(); }
        }

        public Rect BoundingRectangle
        {
            get { return AutomationElement.Current.BoundingRectangle; }
        }

        internal string Id
        {
            get { return AutomationElement.GetId(); }
        }

        public AutomationElement AutomationElement
        {
            get
            {
                if (!IsOffScreen)
                {
                    return _automationElement;
                }

                if (WindowTitle != null)
                {
                    return _automationElement =
                        new Window(Regex.Escape(WindowTitle)).FindControl(SearchCriteria, Index).AutomationElement;
                }

                return null;
            }
            set { _automationElement = value; }
        }

        public bool Enabled
        {
            get { return AutomationElement.Current.IsEnabled; }
        }

        public string Name
        {
            get { return AutomationElement.Current.Name; }
        }

        #endregion

        #region Constructors

        public Control(AutomationElement automationElement, Window window, By searchCriteria, int index)
        {
            AutomationElement = automationElement;
            Window = window;
            SearchCriteria = searchCriteria;
            Index = index;
            Automation.AddAutomationEventHandler(InvokePattern.InvokedEvent, AutomationElement, TreeScope.Element,
                (sender, args) =>
                {
                    _clicked = true;
                });
        }

        #endregion

        #region Control Finders

        private void RefreshBaseList(AutomationElement automationElement)
        {
            BaseAutomationElementList = automationElement
                .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
        }

        internal List<AutomationElement> Find(AutomationElement automationElement, By searchCriteria, int index)
        {
            DateTime start = DateTime.Now;

            RefreshBaseList(AutomationElement);

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
                    RefreshBaseList(AutomationElement);
                }
            }

            if (element == null)
                throw new ControlNotFoundException(
                    Logging.ControlNotFoundException(searchCriteria.Identifiers));

            searchCriteria.Duration = (DateTime.Now - start).TotalSeconds;

            return list;
        }

        public T FindControl<T>(By searchCriteria, int index = 0)
        {
            List<AutomationElement> elements = Find(AutomationElement, searchCriteria, index);

            var returnControl =
                ControlFactory.Create<T>(elements.ElementAt(index), Window, searchCriteria, index);

            Logging.ControlFound(searchCriteria);

            if (elements.Count() > 1)
                Logging.MutlipleControlsWarning(elements);

            return returnControl;
        }

        public T FindControl<T>(string automationId, int index = 0)
        {
            return FindControl<T>(By.AutomationId(automationId), index);
        }

        public T FindControl<T>(ControlType type)
        {
            return FindControl<T>(By.ControlType(type));
        }

        public IControl FindControl(By searchCriteria, int index = 0)
        {
            List<AutomationElement> elements = Find(AutomationElement, searchCriteria, index);
            return ControlFactory.Create(elements.ElementAt(index), Window, searchCriteria, index);
        }

        public IControl FindControl(string automationId, int index = 0)
        {
            By searchCriteria = By.AutomationId(automationId);
            List<AutomationElement> elements = Find(AutomationElement, searchCriteria, index);
            return ControlFactory.Create(elements.ElementAt(index), Window, searchCriteria, index);
        }

        public List<AutomationElement> FindAll(By searchCriteria)
        {
            return Find(AutomationElement, searchCriteria, 0);
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

        public IControl ClickAnyway()
        {
            DateTime start = DateTime.Now;
            Point? point = null;
            if (AutomationElement.Current.ControlType.Equals(ControlType.Edit))
            {
                do
                {
                    try
                    {
                        point = AutomationElement.GetClickablePoint();
                    }
                    catch (NoClickablePointException)
                    {
                        Window.OnTop();
                    }
                    if (point != null)
                    {
                        Mouse.Instance.Click(new Point(point.Value.X, point.Value.Y));
                    }

                    Thread.Sleep(Settings.Default.Delay);
                } while (!AutomationElement.Current.HasKeyboardFocus
                         && (DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout);
            }
            else if (AutomationElement.Current.ControlType.Equals(ControlType.Button))
            {
                while (!_clicked)
                {
                    try
                    {
                        point = AutomationElement.GetClickablePoint();
                    }
                    catch (NoClickablePointException)
                    {
                        Window.OnTop();
                    }
                    if (point != null)
                    {
                        Mouse.Instance.Click(new Point(point.Value.X, point.Value.Y));
                    }
                    //TODO: костыль
                    Thread.Sleep(2000);
                }
            }
            else
            {
                try
                {
                    point = AutomationElement.GetClickablePoint();
                }
                catch (NoClickablePointException)
                {
                    Window.OnTop();
                }
                if (point != null)
                {
                    Mouse.Instance.Click(new Point(point.Value.X, point.Value.Y));
                }
                else
                {
                    Mouse.Instance.Click(AutomationElement.GetClickablePoint());
                }
            }
            Logging.Click(SearchCriteria.Identifiers);
            return this;
        }

        public IControl Click()
        {
            WaitForEnabled();
            return ClickAnyway();
        }

        public IControl DoubleClick()
        {
            Mouse.Instance.DoubleClick(AutomationElement.GetClickablePoint());
            return this;
        }

        public IControl WaitForEnabled()
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
                throw new ControlNotEnabledException(Logging.ControlNotEnabledException(SearchCriteria.Identifiers));
            return this;
        }

        public IControl Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
            return this;
        }

        public string GetText()
        {
            return AutomationElement.GetText();
        }

        public IControl DrawHighlight()
        {
            AutomationElement.DrawHighlight();
            return this;
        }

        private Control ClickAnyway(int x, int y)
        {
            Mouse.Instance.Click(new Point(x, y));
            return this;
        }

        public IControl ClickRightEdge()
        {
            WaitForEnabled();
            Rect r = AutomationElement.Current.BoundingRectangle;
            return ClickAnyway((int) (r.X + r.Width - 1), (int) (r.Y + r.Height/2));
        }

        public IControl ClickLeftEdge()
        {
            WaitForEnabled();
            Rect r = AutomationElement.Current.BoundingRectangle;
            return ClickAnyway((int) (r.X + 1), (int) (r.Y + r.Height/2));
        }

        public ToggleState GetToggleState()
        {
            object objPat;
            if (AutomationElement.TryGetCurrentPattern(TogglePattern.Pattern, out objPat))
            {
                return ((TogglePattern) objPat).Current.ToggleState;
            }
            throw new GeneralException(string.Format(Logging.Strings["UnsupportedPattern"], SearchCriteria.Identifiers,
                "TogglePattern"));
        }

        public Control ScrollVertical(ScrollAmount scrollAmount)
        {
            object o;
            if (AutomationElement.TryGetCurrentPattern(ScrollPattern.Pattern, out o))
            {
                var scrollPattern = (ScrollPattern) o;
                scrollPattern.ScrollVertical(scrollAmount);
            }
            return this;
        }

        public Control ScrollHorizontal(ScrollAmount scrollAmount)
        {
            object o;
            if (AutomationElement.TryGetCurrentPattern(ScrollPattern.Pattern, out o))
            {
                var scrollPattern = (ScrollPattern) o;
                scrollPattern.ScrollHorizontal(scrollAmount);
            }
            return this;
        }

        public IControl Send(Keys key)
        {
            Keyboard.Instance.Send(key);
            Logging.Sent(key.ToString("G"));
            return this;
        }

        public IControl ClickIfExists(By searchCriteria)
        {
            IControl control = null;
            object o;

            if (Exists(searchCriteria, out o))
            {
                control = ControlFactory.Create((AutomationElement) o, Window, searchCriteria, 0);
                control.Click();
            }

            return control;
        }

        public IControl ClickIfExists(string automationId)
        {
            return ClickIfExists(By.AutomationId(automationId));
        }

        #endregion
    }
}