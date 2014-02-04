using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using Castle.Core.Internal;
using TestStack.White.InputDevices;
using TestStack.White.WindowsAPI;
using WhiteSharp.Interfaces;
using WhiteSharp.Extensions;

namespace WhiteSharp
{
    public class Window : Container, IWindow
    {
        #region Private Fields
        private WindowVisualState _displayState;
        private WindowPattern WindowPattern { get; set; }
        private static List<string> _identifiers = new List<string>();
        private static DateTime _start; 
        #endregion

        #region Properties

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
            BaseAutomationElementList = element
                .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
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

        private static List<AutomationElement> FindWindows(params string[] titles)
        {
            _identifiers = new List<string>();
            List<AutomationElement> windows = new List<AutomationElement>();
            _start = DateTime.Now;
            titles.ForEach(x=>_identifiers.Add(x));

            while (!windows.Any() && (DateTime.Now - _start).TotalMilliseconds < Settings.Default.Timeout)
            {
                try
                {
                    windows = Desktop.Instance.Windows
                        .FindAll(window => titles.Select(x => window.Title().Contains(x)).All(x => x.Equals(true))).ToList();
                }
                catch (Exception e)
                {
                    Logging.Exception(e);
                }
                Thread.Sleep(Settings.Default.Delay);
            }
            return windows;
        }

        private static List<AutomationElement> FindWindows(Predicate<AutomationElement> predicate)
        {
            _identifiers = new List<string>();
            List<AutomationElement> windows = null;

            _start = DateTime.Now;

            do
            {
                try
                {
                    windows = Desktop.Instance.Windows.FindAll(predicate);
                }
                catch (ElementNotAvailableException e)
                {
                    Logging.Exception(e);
                }
                Thread.Sleep(Settings.Default.Delay);
            } while (windows == null &&
                     (!windows.Any() && ((DateTime.Now - _start).TotalMilliseconds < Settings.Default.Timeout)));
            _identifiers.Add(predicate.ToString());
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
            return Desktop.Instance.Windows
                .FindAll(window => window.Current.ProcessId.Equals(processId));
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
                    windows = Desktop.Instance.Windows.FindAll(p)
                        .Where(x => x.Current.ProcessId.Equals(processId)).ToList();
                }
                catch (Exception e)
                {
                    Logging.Exception(e);
                }
                Thread.Sleep(Settings.Default.Delay);
            } 
            _identifiers.Add(processId + " " + p);
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

        #region Actions

        public void Send(string value)
        {
            switch (value)
            {
                case "{F5}":
                    {
                        Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.F5);
                        break;
                    }
                case "{Tab}":
                    {
                        Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
                        break;
                    }
                case "{Esc}":
                    {
                        Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.ESCAPE);
                        break;
                    }
                case "{Alt}+{F4}":
                    {
                        Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.ALT);
                        Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.F4);
                        Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.ALT);
                        break;
                    }
                case "^{ENTER}":
                    {
                        Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                        Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
                        Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                        break;
                    }
            }
            Logging.Sent(value);
        }

        public void Close()
        {
            WindowPattern.Close();
            Logging.WindowClosed(Title);
        } 
        #endregion
    }
}