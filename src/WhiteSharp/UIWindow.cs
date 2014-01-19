using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
            uint uFlags);

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 SWP_SHOWWINDOW = 0x0040;

        #region Window Members

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

        public new UIWindow ModalWindow(string title)
        {
            return new UIWindow(ProcessId, x => x.Title.Equals(Process.GetProcessById(ProcessId).MainWindowTitle));
        }

        private static readonly InitializeOption InitializeOption = InitializeOption.NoCache;

        private new static readonly WindowSession WindowSession = new WindowSession(new ApplicationSession(),
            InitializeOption);

        private static int _processId;
        public int ProcessId
        {
            get { return _processId; }
        }

        private static List<AutomationElement> _baseList;
        internal List<AutomationElement> BaseList 
        {
            get { return _baseList; }
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

        #region Window finders

        private static List<Window> FindWindows(params string[] titles)
        {
            _identifiers = new List<string>();
            List<Window> windows = new List<Window>();
            _start = DateTime.Now;

            _identifiers.Add(titles[0]);
            while (!windows.Any() && (DateTime.Now - _start).TotalMilliseconds < Settings.Default.Timeout)
            {
                try
                {
                    windows = Desktop.Instance.Windows().FindAll(window => window.Title.Contains(titles[0]));
                    foreach (string title in titles.Skip(1))
                    {
                        _identifiers.Add(title);
                        windows = windows.Where(window => window.Title.Contains(title)).ToList();
                    }
                }
                catch (ElementNotAvailableException e)
                {
                    Logging.Exception(e);
                }
                Thread.Sleep(Settings.Default.Delay);
            }
            return windows;
        }

        private static List<Window> FindWindows(Predicate<Window> predicate)
        {
            _identifiers = new List<string>();
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

        private static List<Window> FindWindows(int processId)
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
            return Desktop.Instance.Windows()
                .FindAll(window => window.AutomationElement.Current.ProcessId.Equals(processId));
        }

        private static List<Window> FindWindows(int processId, Predicate<Window> p)
        {
            _identifiers = new List<string>();
            _identifiers.Add(processId + " " + p);
            return FindWindows(p).Where(x => x.AutomationElement.Current.ProcessId.Equals(processId)).ToList();
        }

        private static Window SelectWindow(List<Window> windows)
        {
            if (windows == null || !windows.Any())
                throw new WindowNotFoundException(
                    Logging.WindowException(_identifiers.Aggregate((x, y) => x + ", " + y)));
            
            Window returnWindow = windows.First();
            Logging.WindowFound(returnWindow, DateTime.Now - _start);
            if (windows.Count > 1)
                Logging.MutlipleWindowsWarning(windows);

            if (returnWindow.DisplayState == DisplayState.Minimized)
                returnWindow.DisplayState = DisplayState.Restored;
            SetWindowPos(new IntPtr(returnWindow.AutomationElement.Current.NativeWindowHandle), HWND_TOPMOST, 0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);

            _processId = returnWindow.AutomationElement.Current.ProcessId;

            _baseList = returnWindow.AutomationElement.FindAll(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.IsOffscreenProperty, false)).OfType<AutomationElement>().ToList();

            return returnWindow;
        }

        #endregion

        public UIControl FindControl(By searchCriteria, int index = 0)
        {
            DateTime start = DateTime.Now;
            List<AutomationElement> result = Finder.Find(BaseList, searchCriteria);
            UIControl returnControl = new UIControl(result.ElementAt(index), actionListener)
            {
                Identifiers = searchCriteria.Identifiers,
                Window = this
            };

            Logging.ControlFound(searchCriteria.Identifiers, this.Title, DateTime.Now - start);

            if (result.Count() > 1)
                Logging.MutlipleControlsWarning(result);

            return returnControl;
        }

        public UIControl FindControl(string automationId)
        {
            return FindControl(By.AutomationId(automationId));
        }

        public UIControl FindControl(ControlType type)
        {
            return FindControl(By.ControlType(type));
        }

        public List<AutomationElement> FindAll(By searchCriteria)
        {
            return Finder.Find(BaseList, searchCriteria);
        }

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