using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using WhiteSharp.Extensions;
using Button = WhiteSharp.Controls.Button;
using TextBox = WhiteSharp.Controls.TextBox;

namespace WhiteSharp
{
    public sealed class Window : Container
    {
        #region Private Fields

        private WindowVisualState _displayState;
        private WindowPattern _windowPattern;

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
                    : (WindowPattern)AutomationElement.GetCurrentPattern(WindowPattern.Pattern);
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

        public Window(AutomationElement element)
        {
            AutomationElement = element;
            WindowTitle = element.Title();
            ProcessId = element.Current.ProcessId;
            WindowPattern = (WindowPattern)element.GetCurrentPattern(WindowPattern.Pattern);
            RefreshBaseList(AutomationElement);
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
            var start = DateTime.Now;

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
                catch (Exception)
                {
                    Thread.Sleep(100);
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

        public bool CheckViewName(string подсторокаНазванияФормы)
        {
            Thread.Sleep(1000);
            Send(Keys.CtrlG);
            Thread.Sleep(1000);
            var fullName = Clipboard.GetText();
            Logging.Info(String.Format("Проверяем, что '{0}' содержит '{1}'...", fullName, подсторокаНазванияФормы));
            return fullName.Contains(подсторокаНазванияФормы);
        }

        public void Send(Keys key)
        {
            Keyboard.Instance.Send(key);
            Logging.Sent(key.ToString());
        }

        public void Send(Keys[] keysArray)
        {
            foreach (var key in keysArray)
            {
                Keyboard.Instance.Send(key);
            }
        }

        public void LeaveAllKeys()
        {
            Keyboard.Instance.LeaveAllKeys();
        }

        public void SendDate(string date)
        {
            SendKeys.SendWait(date);
        }

        public void Close()
        {
            WindowPattern.Close();
            Logging.WindowClosed(WindowTitle);
        }

        #endregion

    }
}