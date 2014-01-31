using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Automation;
using Castle.Core.Internal;
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

        #region Native Window Methods
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
            uint uFlags);

        private static readonly IntPtr HWND_TOP = new IntPtr(0);
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 SWP_SHOWWINDOW = 0x0040; 
        #endregion

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

        private static readonly InitializeOption InitializeOption = InitializeOption.NoCache;

        private new static readonly WindowSession WindowSession = new WindowSession(new ApplicationSession(),
            InitializeOption);

        private readonly string _title;
        private static int _processId;

        public int ProcessId
        {
            get { return _processId; }
        }

        internal List<AutomationElement> BaseAutomationElementList = new List<AutomationElement>();

        private static List<string> _identifiers = new List<string>();
        private static DateTime _start;

        #region Constructors

        public UIWindow(params string[] titles)
            : base(SelectWindow(FindWindows(titles)).AutomationElement, InitializeOption, WindowSession)
        {
            BaseAutomationElementList = AutomationElement
                .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
            _title = AutomationElement.Current.Name;
        }

        public UIWindow(Predicate<Window> p)
            : base(SelectWindow(FindWindows(p)).AutomationElement, InitializeOption, WindowSession)
        {
            BaseAutomationElementList = AutomationElement
                .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
            _title = AutomationElement.Current.Name;
        }

        public UIWindow(int processId)
            : base(FindWindows(processId).First().AutomationElement, InitializeOption, WindowSession)
        {
            BaseAutomationElementList = AutomationElement
                .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
            _title = AutomationElement.Current.Name;
        }

        public UIWindow(int processId, Predicate<Window> p)
            : base(SelectWindow(FindWindows(processId, p)).AutomationElement, InitializeOption, WindowSession)
        {
            BaseAutomationElementList = AutomationElement
                .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
            _title = AutomationElement.Current.Name;
        }

        #endregion

        #region Window Finders

        private static List<Window> FindWindows(params string[] titles)
        {
            _identifiers = new List<string>();
            List<Window> windows = new List<Window>();
            _start = DateTime.Now;
            titles.ForEach(x=>_identifiers.Add(x));

            while (!windows.Any() && (DateTime.Now - _start).TotalMilliseconds < Settings.Default.Timeout)
            {
                try
                {
                    windows = Desktop.Instance.Windows().FindAll(window => !window.Title.IsNullOrEmpty())
                        .Where(window => titles.Select(x => window.Title.Contains(x)).All(x=>x.Equals(true))).ToList();
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
            } while (windows == null &&
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
            List<Window> windows = null;
            _start = DateTime.Now;
            do
            {
                try
                {
                    windows = Desktop.Instance.Windows().FindAll(p)
                        .Where(x => x.AutomationElement.Current.ProcessId.Equals(processId)).ToList();
                }
                catch (Exception e)
                {
                    Logging.Exception(e);
                }
                Thread.Sleep(Settings.Default.Delay);
            } while ((windows == null || 
                        !windows.Any()) && ((DateTime.Now - _start).TotalMilliseconds < Settings.Default.Timeout));
            _identifiers.Add(processId + " " + p);
            return windows;
        }

        private static Window SelectWindow(List<Window> windows)
        {
            if (windows == null || !windows.Any())
                throw new WindowNotFoundException(
                    Logging.WindowException(_identifiers.Aggregate((x, y) => x + ", " + y)));


            Window returnWindow = windows.First();

            if (returnWindow.DisplayState == DisplayState.Minimized)
                returnWindow.DisplayState = DisplayState.Restored;
            SetWindowPos(new IntPtr(returnWindow.AutomationElement.Current.NativeWindowHandle), HWND_TOP, 0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);

            _processId = returnWindow.AutomationElement.Current.ProcessId;

            Logging.WindowFound(returnWindow, DateTime.Now - _start);
            if (windows.Count > 1)
                Logging.MutlipleWindowsWarning(windows);
            
            return returnWindow;
        }

        #endregion

        #region Control Finders
        public UIControl FindControl(By searchCriteria, int index = 0)
        {
            DateTime start = DateTime.Now;
            List<AutomationElement> element = Finder.Find(BaseAutomationElementList, searchCriteria);
            if (element == null)
            {
                BaseAutomationElementList = new UIWindow(_title).AutomationElement
                    .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                    .OfType<AutomationElement>().ToList();
                element = Finder.Find(BaseAutomationElementList, searchCriteria);
            }


            UIControl returnControl = new UIControl(element.ElementAt(index), actionListener)
            {
                Identifiers = searchCriteria.Identifiers,
                Window = this
            };

            searchCriteria.Duration = (DateTime.Now - start).TotalSeconds;

            Logging.ControlFound(searchCriteria);

            return returnControl;
        }

        public UIControl FindControl(string automationId, int index = 0)
        {
            return FindControl(By.AutomationId(automationId), index);
        }

        public UIControl FindControl(ControlType type)
        {
            return FindControl(By.ControlType(type));
        }

        public List<AutomationElement> FindAll(By searchCriteria)
        {
            return Finder.Find(BaseAutomationElementList, searchCriteria);
        }

        public bool Exists(By searchCriteria)
        {
            try
            {
                if (AutomationElement.FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                    .OfType<AutomationElement>().ToList().FindAll(searchCriteria.Result).Count > 0)
                    return true;
                return false;

            }
            catch (Exception)
            {
                return false;
            }
        } 
        #endregion

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

        public new void Close()
        {
            base.Close();
            Logging.WindowClosed(_title);
        }
    }
}