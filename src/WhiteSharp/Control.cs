using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using TestStack.White.InputDevices;
using WhiteSharp.Controls;
using WhiteSharp.Extensions;

namespace WhiteSharp
{
    public class Control : Container, IControl
    {
        #region Properties

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

        public bool Enabled
        {
            get { return AutomationElement.Current.IsEnabled; }
        }

        public string HelpText
        {
            get { return AutomationElement.Current.HelpText; }
        }

        public string Name
        {
            get { return AutomationElement.Current.Name; }
        }

        public bool IsValuePatternReadOnly
        {
            get
            {
                object o;
                return AutomationElement.TryGetCurrentPattern(ValuePattern.Pattern, out o) && ((ValuePattern)o).Current.IsReadOnly;
            }
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
            var start = DateTime.Now;

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

        public IControl ClickOnce()
        {
            ((Window)Window).OnTop();
            Point? point = AutomationElement.GetClickablePoint();
            Thread.Sleep(1000);
            Mouse.Instance.Click(new Point(point.Value.X, point.Value.Y));
            Thread.Sleep(Settings.Default.Delay);
            Logging.Click(SearchCriteria.Identifiers);
            return this;
        }

        public void TryFindErrorWindow()
        {
            try
            {
                var errorWindow = new Window("Сообщение об ошибке");
                var errorName = errorWindow.FindControl("lblHeader").GetText();
                errorWindow.FindControl<Button>(By.Name("Показать подробности")).Click();
                var errorMessage = errorWindow.FindControl<Document>("txtDetails").GetAllText();
                Logging.Info(String.Format("Найдено окно 'Сообщение об ошибке': {0}", errorName));
                Logging.Info(String.Format("Найдено окно 'Сообщение об ошибке': {0}", errorMessage));
                throw new Exception(String.Format("Поймали окно с ошибкой! Текст: {0}", errorName + " " + Environment.NewLine
                    + errorMessage));
            }
            catch { }
        }

        public virtual IControl ClickAnyway(bool doCheckErrorWindow = false)
        {
            Point? point = null;
            try
            {
                point = AutomationElement.GetClickablePoint();
            }
            catch (NoClickablePointException)
            {
                ((Window)Window).OnTop();
            }
            Mouse.Instance.Click(point != null
                ? new Point(point.Value.X, point.Value.Y)
                : AutomationElement.GetClickablePoint());
            Logging.Click(SearchCriteria.Identifiers);

            if (doCheckErrorWindow) TryFindErrorWindow();

            return this;
        }

        public IControl Click()
        {
            WaitForEnabled().Wait(100);
            return ClickAnyway().Wait(200);
        }

        public IControl DoubleClick()
        {
            Mouse.Instance.DoubleClick(AutomationElement.GetClickablePoint());
            return this;
        }

        public IControl WaitForEnabled()
        {
            var start = DateTime.Now;
            var count = 0;

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

        public string GetHelpText()
        {
            return AutomationElement.GetHelpText();
        }

        public string GetName()
        {
            return AutomationElement.GetName();
        }

        public string GetId()
        {
            return AutomationElement.GetId();
        }

        public void Send(string name)
        {
            throw new NotImplementedException();
        }

        public void SelectItem(string name)
        {
            throw new NotImplementedException();
        }

        public IControl DrawHighlight()
        {
            AutomationElement.DrawHighlight();
            return this;
        }

        public IControl ClickRightEdge()
        {
            WaitForEnabled();
            var r = AutomationElement.Current.BoundingRectangle;
            return ClickAnyway((int)(r.X + r.Width - 1), (int)(r.Y + r.Height / 2));
        }

        public IControl ClickLeftEdge()
        {
            WaitForEnabled();
            var r = AutomationElement.Current.BoundingRectangle;
            return ClickAnyway((int)(r.X + 1), (int)(r.Y + r.Height / 2));
        }

        public IControl Send(Keys key)
        {
            Keyboard.Instance.Send(key);
            Logging.Sent(key.ToString("G"));
            Keyboard.Instance.LeaveAllKeys();
            return this;
        }

        private Control ClickAnyway(int x, int y)
        {
            Mouse.Instance.Click(new Point(x, y));
            return this;
        }

        public Control ScrollVertical(ScrollAmount scrollAmount)
        {
            object o;
            if (AutomationElement.TryGetCurrentPattern(ScrollPattern.Pattern, out o))
            {
                var scrollPattern = (ScrollPattern)o;
                scrollPattern.ScrollVertical(scrollAmount);
            }
            return this;
        }

        public Control ScrollHorizontal(ScrollAmount scrollAmount)
        {
            object o;
            if (AutomationElement.TryGetCurrentPattern(ScrollPattern.Pattern, out o))
            {
                var scrollPattern = (ScrollPattern)o;
                scrollPattern.ScrollHorizontal(scrollAmount);
            }
            return this;
        }

        #endregion
    }
}