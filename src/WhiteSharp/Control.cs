using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using TestStack.White.InputDevices;
using WhiteSharp.Extensions;

namespace WhiteSharp
{
    public class Control : Container, IControl
    {
        #region Properties
        
        public override sealed AutomationElement AutomationElement
        {
            get
            {
                return (!AutomationElementField.IsOffScreen())
                    ? AutomationElementField
                    : (AutomationElementField =
                    new Window(Regex.Escape(WindowTitle))
                        .FindControl(SearchCriteria, Index).AutomationElement);
            }
            protected set { AutomationElementField = value; }
        }

        internal IControlContainer Window { get; set; }
        
        internal By SearchCriteria { get; set; }

        internal int Index { get; set; }

        public Rect BoundingRectangle
        {
            get { return AutomationElement.Current.BoundingRectangle; }
        }

        internal string Id
        {
            get { return AutomationElement.GetId(); }
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

        public Control(AutomationElement automationElement, IControlContainer window, By searchCriteria, int index)
        {
            AutomationElement = automationElement;
            Window = window;
            WindowTitle = window.AutomationElement.Title();
            SearchCriteria = searchCriteria;
            Index = index;
        }

        #endregion

        public override List<AutomationElement> Find(AutomationElement automationElement, By searchCriteria, int index)
        {
            DateTime start = DateTime.Now;

            RefreshBaseList(AutomationElement);

            var list = new List<AutomationElement>();
            AutomationElement element = null;

            while ((!list.Any() || element == null) &&
                   (DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout)
            {
                list = BaseAutomationElementList.FindAll(searchCriteria.Result);
                element = list.ElementAtOrDefault(index);
                
                if (element == null)
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
       
        #region Actions

        public virtual IControl ClickAnyway()
        {
            Point? point = null;
            try
            {
                point = AutomationElement.GetClickablePoint();
            }
            catch (NoClickablePointException)
            {
                ((Window) Window).OnTop();
            }
            Mouse.Instance.Click(point != null
                ? new Point(point.Value.X, point.Value.Y)
                : AutomationElement.GetClickablePoint());
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

        #endregion
    }
}