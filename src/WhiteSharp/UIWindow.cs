using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.Sessions;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;
using WhiteSharp.Interfaces;

namespace WhiteSharp
{
    public class UIWindow : Window, IUIWindow
    {
        #region WindowMembers

        private readonly WindowFactory windowFactory = null;

        public override PopUpMenu Popup
        {
            get { return factory.WPFPopupMenu(this) ?? windowFactory.PopUp(this); }
        }

        public override Window ModalWindow(string title, InitializeOption option)
        {
            WindowFactory desktopWindowsFactory = WindowFactory.Desktop;
            return desktopWindowsFactory.FindModalWindow(title,
                Process.GetProcessById(automationElement.Current.ProcessId), option, automationElement,
                WindowSession.ModalWindowSession(option));
        }

        public override Window ModalWindow(SearchCriteria searchCriteria, InitializeOption option)
        {
            WindowFactory desktopWindowsFactory = WindowFactory.Desktop;
            return desktopWindowsFactory.FindModalWindow(searchCriteria, option, automationElement,
                WindowSession.ModalWindowSession(option));
        }

        #endregion

        private static readonly InitializeOption InitializeOption = InitializeOption.NoCache;
        private static readonly new WindowSession WindowSession = new WindowSession(new ApplicationSession(), InitializeOption);

        private static int _processId;
        public int ProcessId
        {
            get
            {
                return _processId;
            }
        }

        private static List<string> _identifiers = new List<string>();
        private static DateTime _start;

        #region Constructors

        public UIWindow(params string[] titles)
            : base(SelectWindow(FindWindows(titles)).AutomationElement, InitializeOption, WindowSession)
        {
        }

        public UIWindow(Predicate<Window> p)
            : base(SelectWindow(FindWindows(p)).AutomationElement, InitializeOption, WindowSession)
        {
        }

        public UIWindow(int processId)
            : base(FindWindows(processId).First().AutomationElement, InitializeOption, WindowSession)
        {
        }

        public UIWindow(int processId, Predicate<Window> p)
            : base(SelectWindow(FindWindows(processId, p)).AutomationElement, InitializeOption, WindowSession)
        {
        } 

        #endregion

        #region Finders

        private static List<Window> FindWindows(params string[] titles)
        {
            List<Window> windows = null;

            _start = DateTime.Now;

            do
            {
                try
                {
                    windows = Desktop.Instance.Windows().FindAll(window => window.Title.Contains(titles[0]));
                    _identifiers.Add(titles[0]);
                    foreach (string title in titles.Skip(1))
                    {
                        windows = windows.Where(window => window.Title.Contains(title)).ToList();
                        _identifiers.Add(title);
                    }
                }
                catch (ElementNotAvailableException e)
                {
                    Logging.Exception(e);
                }
                Thread.Sleep(Settings.Default.Delay);
            } while (windows != null &&
                (!windows.Any() && ((DateTime.Now - _start).TotalMilliseconds < Settings.Default.Timeout)));
            return windows;
        }

        private static List<Window> FindWindows(Predicate<Window> predicate)
        {
            List<Window> windows = null;

            _start = DateTime.Now;

            do
            {
                try
                {
                    windows = Desktop.Instance.Windows().FindAll(predicate);
                }
                catch (ElementNotAvailableException e)
                {
                    Logging.Exception(e);
                }
                Thread.Sleep(Settings.Default.Delay);
            } while (windows != null &&
                (!windows.Any() && ((DateTime.Now - _start).TotalMilliseconds < Settings.Default.Timeout)));
            _identifiers.Add(predicate.ToString());
            return windows;
            
        }

        public static List<Window> FindWindows(int processId)
        {
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
            return Desktop.Instance.Windows()
                    .FindAll(window => window.AutomationElement.Current.ProcessId.Equals(processId));
        }

        private static List<Window> FindWindows(int processId, Predicate<Window> p)
        {
            _identifiers.Add(processId + " " + p);
            return FindWindows(p).Where(x=>x.AutomationElement.Current.ProcessId.Equals(processId)).ToList();
        }

        private static Window SelectWindow(List<Window> windows)
        {
            if (windows == null || !windows.Any())
                throw new WindowNotFoundException(
                    Logging.WindowException(_identifiers.Select(x => x.ToString()).Aggregate((x, y) => x + ", " + y)));
            _identifiers = new List<string>();

            Window returnWindow = windows.First();
            Logging.WindowFound(returnWindow, DateTime.Now - _start);
            if (windows != null && windows.Count > 1)
                Logging.MutlipleWindowsWarning(windows);

            By.Window = returnWindow;
            if (returnWindow.DisplayState == DisplayState.Minimized)
                returnWindow.DisplayState = DisplayState.Restored;
            returnWindow.Focus();
            _processId = returnWindow.AutomationElement.Current.ProcessId;
            return returnWindow;
        } 
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f">Search criteria</param>
        /// <returns></returns>
        public UIControl FindControl(Finder f)
        {
            Logging.ControlFound(f);
            if (f.Result.Count > 1)
                Logging.MutlipleControlsWarning(f.Result);
            
            return new UIControl(f.Result.First(), this);
        }

        public UIControl FindControl(string automationId)
        {
            return new UIControl(By.AutomationId(automationId).Result.First(), this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f">Search criteria</param>
        /// <returns>List of controls found by given criteria</returns>
        public List<AutomationElement> FindAll(Finder f)
        {
            return f.Result;
        }

        /// <summary>
        /// Supported shortcuts: {F5}, {Tab}, {Esc}, {Alt}+{F4}.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Send(string value)
        {
            Focus();
            switch (value)
            {
                case "{F5}":
                {
                    TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.F5,
                        ActionListener);
                    break;
                }
                case "{Tab}":
                {
                    Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
                    break;
                }
                case "{Esc}":
                {
                    Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.ESCAPE);
                    break;
                }
                case "{Alt}+{F4}":
                {
                    Keyboard.HoldKey(KeyboardInput.SpecialKeys.ALT);
                    Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.F4);
                    Keyboard.LeaveKey(KeyboardInput.SpecialKeys.ALT);
                    break;
                }
                case "^{ENTER}":
                {
                    Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
                    Keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                    break;
            }
                    
            }
            Logging.Sent(value);
        }
    }
}
