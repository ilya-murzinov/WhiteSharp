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

namespace WhiteSharp
{
    public class UIWindow : Window
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="titles">List of strings, which the desired window's title should contain</param>
        /// <returns></returns>
        public UIWindow(params string[] titles) : base(FindWindow(titles).AutomationElement, InitializeOption.NoCache,
            new WindowSession(new ApplicationSession(),
                InitializeOption.NoCache))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="titles">List of strings, which the desired window's title should contain</param>
        /// <returns></returns>
        private static Window FindWindow(params string[] titles)
        {
            List<Window> windows = null;

            DateTime start = DateTime.Now;
            do
            {
                try
                {
                    windows = Desktop.Instance.Windows().FindAll(window => window.Title.Contains(titles[0]));
                    foreach (string title in titles.Skip(1))
                    {
                        windows = windows.Where(window => window.Title.Contains(title)).ToList();
                    }
                }
                catch (ElementNotAvailableException e)
                {
                    Logging.Exception(e);
                }
                Thread.Sleep(Settings.Default.Delay);
            } while (windows != null &&
                     (!windows.Any() && ((DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout)));
            if (windows != null && !windows.Any())
                throw new WindowNotFoundException(
                    Logging.WindowException(titles.ToList().Aggregate((x, y) => x + ", " + y)));

            if (windows != null)
            {
                Window returnWindow = windows.First();
                Logging.WindowFound(returnWindow, DateTime.Now - start);
                if (windows != null && windows.Count > 1)
                    Logging.MutlipleWindowsWarning(windows);

                By.Window = returnWindow;
                if (returnWindow.DisplayState == DisplayState.Minimized)
                    returnWindow.DisplayState = DisplayState.Restored;
                returnWindow.Focus();

                return returnWindow;
            }
            return null;
        }

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
        public UIWindow Send(string value)
        {
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
            }
            Logging.Sent(value);
            return this;
        }
    }
}