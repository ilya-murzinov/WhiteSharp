using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Automation;
using WhiteSharp.Extensions;

namespace WhiteSharp
{
    public sealed class Window : Container
    {
        #region Private Fields

        private WindowVisualState _displayState;
        private WindowPattern _windowPattern;
        private List<Control> _controls = new List<Control>();

        #endregion

        #region Properties

        public override AutomationElement AutomationElement
        {
            get
            {
                return (!AutomationElementField.IsOffScreen())
                    ? AutomationElementField
                    : (AutomationElementField =
                    new Window(Regex.Escape(WindowTitle)).AutomationElement);
            }
            protected set { AutomationElementField = value; }
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
        
        public int ProcessId { get; private set; }
        
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
            WindowTitle = element.Title();
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

        public override List<AutomationElement> Find(AutomationElement automationElement, By searchCriteria, int index)
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
                    element = list.ElementAtOrDefault(index);
                }
                catch (Exception ex)
                {
                }
                if (element == null)
                {
                    RefreshBaseList(IsOffScreen
                        ? new Window(Regex.Escape(WindowTitle)).AutomationElement
                        : AutomationElement);
                }
            }

            if (element == null)
                throw new ControlNotFoundException(
                    Logging.ControlNotFoundException(searchCriteria.Identifiers));

            searchCriteria.Duration = (DateTime.Now - start).TotalSeconds;

            return list;
        }

        #region Actions

        public void Send(Keys key)
        {
            Keyboard.Instance.Send(key);
        }

        public void Close()
        {
            WindowPattern.Close();
            Logging.WindowClosed(WindowTitle);
        }

        #endregion
    }
}