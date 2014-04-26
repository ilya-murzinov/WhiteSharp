using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;
using TestStack.White.InputDevices;
using TestStack.White.UIItems.Actions;
using WhiteSharp.Extensions;
using WhiteSharp.Interfaces;
using ComboBox = TestStack.White.UIItems.ListBoxItems.ComboBox;
using Point = System.Windows.Point;

namespace WhiteSharp
{
    public class Control : IControl
    {
        #region Private Fields

        private AutomationElement _automationElement;

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
                if (!IsOffScreen) return _automationElement;
                Window = new Window(WindowTitle);
                return _automationElement = 
                    Window.FindControl(SearchCriteria, Index).AutomationElement;
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

        internal Control(AutomationElement element)
        {
            AutomationElement = element;
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

        public Control FindControl(By searchCriteria, int index = 0)
        {
            List<AutomationElement> elements = Find(AutomationElement, searchCriteria, index);

            var returnControl = new Control(elements.ElementAt(index))
            {
                SearchCriteria = searchCriteria,
                Index = index,
                WindowTitle = WindowTitle
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

        public Control ClickAnyway()
        {
            DateTime start = DateTime.Now;
            if (AutomationElement.Current.ControlType.Equals(ControlType.Edit))
            {
                do
                {
                    Point? point = null;
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
            else
                Mouse.Instance.Click(AutomationElement.GetClickablePoint());
            Logging.Click(SearchCriteria.Identifiers);
            return this;
        }

        public Control ClickAnyway(int x, int y)
        {
            Mouse.Instance.Click(new Point(x, y));
            return this;
        }

        public Control Click()
        {
            WaitForEnabled();
            return ClickAnyway();
        }

        public Control ClickRightEdge()
        {
            WaitForEnabled();
            Rect r = AutomationElement.Current.BoundingRectangle;
            return ClickAnyway((int) (r.X + r.Width - 1), (int) (r.Y + r.Height/2));
        }

        public Control ClickLeftEdge()
        {
            WaitForEnabled();
            Rect r = AutomationElement.Current.BoundingRectangle;
            return ClickAnyway((int)(r.X + 1), (int)(r.Y + r.Height/2));
        }

        public Control DoubleClick()
        {
            Mouse.Instance.DoubleClick(AutomationElement.GetClickablePoint());
            return this;
        }

        public Control ClearValue()
        {
            object o;
            if (AutomationElement.TryGetCurrentPattern(ValuePattern.Pattern, out o))
            {
                var pattern = (ValuePattern) o;
                if (!pattern.Current.IsReadOnly)
                {
                    pattern.SetValue("");
                }
            }
            else
                throw new GeneralException(string.Format(Logging.Strings["UnsupportedPattern"], SearchCriteria.Identifiers,
                    "Value Pattern"));

            return this;
        }

        public Control Send(string value)
        {
            if (value == null)
                return this;

            WaitForEnabled();

            try
            {
                AutomationElement.SetFocus();
            }
            catch (InvalidOperationException)
            {
            }

            ClearValue();
            Keyboard.Instance.Send(value);
            return this;
        }

        public Control SetValue(string value)
        {
            Object o;
            AutomationElement.TryGetCurrentPattern(ValuePattern.Pattern, out o);
            if (o != null)
            {
                ((ValuePattern) o).SetValue(value);
            }
            return this;
        }

        public string GetValue()
        {
            Object o;
            AutomationElement.TryGetCurrentPattern(ValuePattern.Pattern, out o);
            if (o != null)
            {
                return ((ValuePattern) o).Current.Value;
            }
            return null;
        }

        public Control SelectItem(string name)
        {
            if (name == null)
                return this;

            WaitForEnabled();
            if (!AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
                throw new GeneralException(string.Format(Logging.Strings["NotACombobox"], SearchCriteria.Identifiers));

            object o;

            if (AutomationElement.TryGetCurrentPattern(ValuePattern.Pattern, out o))
            {
                var valuePattern = (ValuePattern) o;
                valuePattern.SetValue(name);
            }
            else
            {
                //TODO: replace this ro remove link to TestStack.White
                var comboBox = new ComboBox(AutomationElement, new NullActionListener());
                comboBox.Select(name);
            }

            Logging.ItemSelected(name, SearchCriteria.Identifiers);
            return this;
        }

        public Control SelectItem(int index)
        {
            WaitForEnabled();
            if (!AutomationElement.Current.ControlType.Equals(ControlType.ComboBox))
            {
                throw new GeneralException(string.Format(Logging.Strings["NotACombobox"], SearchCriteria.Identifiers));
            }
            var combobox = new ComboBox(AutomationElement, null);
            try
            {
                combobox.Select(index);
            }
            catch (Exception e)
            {
                Logging.Exception(e);
            }
            Logging.ItemSelected(index.ToString(), SearchCriteria.Identifiers);
            return this;
        }

        public void SetToggleState(bool toggled)
        {
            SetToggleState(toggled ? ToggleState.On : ToggleState.Off);
        }

        public void SetToggleState(ToggleState state)
        {
            object objPat;
            if (AutomationElement.TryGetCurrentPattern(TogglePattern.Pattern, out objPat))
            {
                var togglePattern = (TogglePattern)objPat;
                if (togglePattern.Current.ToggleState != state)
                {
                    togglePattern.Toggle();
                }
            }
            else
            {
                throw new GeneralException(string.Format(Logging.Strings["UnsupportedPattern"], SearchCriteria.Identifiers,
                    "TogglePattern"));
            }
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

        public void SelectRadioButton()
        {
            if (AutomationElement.Current.ControlType.Equals(ControlType.RadioButton))
            {
                var p = (SelectionItemPattern) AutomationElement.GetCurrentPattern(SelectionItemPattern.Pattern);
                p.Select();
            }
            else
            {
                throw new GeneralException(string.Format(Logging.Strings["NotARadioButton"], SearchCriteria.Identifiers));
            }
        }

        public void Select()
        {
            object objPat;
            if (AutomationElement.TryGetCurrentPattern(SelectionItemPattern.Pattern, out objPat))
            {
                WaitForEnabled();
                var selectionPattern = (SelectionItemPattern)objPat;
                while (!selectionPattern.Current.IsSelected)
                {
                    selectionPattern.Select();
                    Thread.Sleep(Settings.Default.Delay);
                }
            }
            else
            {
                throw new GeneralException(string.Format(Logging.Strings["UnsupportedPattern"], SearchCriteria.Identifiers,
                    "SelectionItemPattern"));
            }
        }

        public Control WaitForEnabled()
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

        public Control Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
            return this;
        }

        public string GetText()
        {
            return AutomationElement.GetText();
        }

        public Control DrawHighlight()
        {
            AutomationElement.DrawHighlight();
            return this;
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

        public Control Send(Keys key)
        {
            WaitForEnabled();
            Keyboard.Instance.Send(key);
            return this;
        }

        public Control Send(int value)
        {
            WaitForEnabled();
            AutomationElement.SetFocus();
            SendKeys.SendWait(value.ToString());
            return this;
        }

        public Control SelectFirstItem()
        {
            return SelectItem("");
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

        #endregion
    }
}