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

        public void ClickByNameAndEnabled(string name)
        {
            FindControl<Button>(By.Name(name).AndEnabled()).Click();
        }

        public void TryClickByName(string name)
        {
            try
            {
                FindControl<Button>(By.Name(name).AndEnabled()).Click();
            }
            catch (Exception)
            {
                Thread.Sleep(100);
            }
        }

        public bool SendManual(By criteria, string value)
        {
            if (value == null) throw new Exception("Параметр value не может быть пустой строкой!");
            var textBox = FindControl<TextBox>(criteria);
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds < 10)
            {
                textBox.Click();
                foreach (var letter in value.ToCharArray())
                {
                    switch (letter)
                    {
                        case '0':
                            Send(Keys.Zero);
                            break;
                        case '1':
                            Send(Keys.One);
                            break;
                        case '2':
                            Send(Keys.Two);
                            break;
                        case '3':
                            Send(Keys.Three);
                            break;
                        case '4':
                            Send(Keys.Four);
                            break;
                        case '5':
                            Send(Keys.Five);
                            break;
                        case '6':
                            Send(Keys.Six);
                            break;
                        case '7':
                            Send(Keys.Seven);
                            break;
                        case '8':
                            Send(Keys.Eight);
                            break;
                        case '9':
                            Send(Keys.Nine);
                            break;
                    }
                }
                Send(Keys.Tab);
                if (textBox.GetValue() == null || !textBox.GetValue().Equals(value)) continue;
                Logging.Info(String.Format("Ввели число: {0}, текст в поле после ввода: {1}", value, textBox.GetValue()));
                Send(Keys.Tab);
                Keyboard.Instance.LeaveAllKeys();
                return true;
            }

            Keyboard.Instance.LeaveAllKeys();
            Logging.Info(String.Format("Ввели число: {0}, текст в поле после ввода: {1}", value, textBox.GetValue()));
            return false;
        }

        public new string SetKeys(TextBox tb, string keys)
        {
            tb.Click();
            try
            {
                tb.ClearValue();
            }
            catch
            {
                SendKeys.SendWait("{END}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{BS}{HOME}{DEL}{DEL}{DEL}{DEL}{DEL}{DEL}{DEL}{DEL}{HOME}");
            }
            SendKeys.SendWait(keys);
            Keyboard.Instance.LeaveAllKeys();
            return tb.GetValue();
        }

        public bool Prompt(string winName, string btnName)
        {
            try
            {
                var dlg = FindModalWindow(winName);
                var btn = dlg.FindControl<Button>(By.AutomationId(btnName).AndEnabled());
                btn.Click();
                return true;
            }
            catch
            {
                Thread.Sleep(100);
            }
            return false;
        }
        #endregion

    }
}