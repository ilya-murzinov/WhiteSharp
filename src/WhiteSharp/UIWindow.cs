using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.InputDevices;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;
using TestStack.White.UIItems.Finders;
using TestStack.White.Factory;
using TestStack.White.UIItems.MenuItems;
using System.Diagnostics;

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
            return desktopWindowsFactory.FindModalWindow(title, Process.GetProcessById(automationElement.Current.ProcessId), option, automationElement,
                                                         WindowSession.ModalWindowSession(option));
        }

        public override Window ModalWindow(SearchCriteria searchCriteria, InitializeOption option)
        {
            WindowFactory desktopWindowsFactory = WindowFactory.Desktop;
            return desktopWindowsFactory.FindModalWindow(searchCriteria, option, automationElement, WindowSession.ModalWindowSession(option));
        }
        #endregion
        
        public UIWindow(params string[] titles) : base(FindWindow(titles).AutomationElement, TestStack.White.Factory.InitializeOption.NoCache, 
                new TestStack.White.Sessions.WindowSession(new TestStack.White.Sessions.ApplicationSession(), 
                    TestStack.White.Factory.InitializeOption.NoCache))
        {
        }

        private static Window FindWindow(params string[] titles)
        {
            List<Window> windows = null;
            Window returnWindow = null;

            DateTime start = DateTime.Now;
            do
            {
                try
                {
                    windows = Desktop.Instance.Windows().FindAll(window => window.Title.Contains(titles[0]));
                    foreach (var title in titles.Skip(1))
                    {
                        windows = windows.Where(window => window.Title.Contains(title)).ToList();
                    }
                }
                catch (ElementNotAvailableException e)
                {
                    Logging.Exception(e);
                }
                Thread.Sleep(Settings.Default.Delay);
            } while (!windows.Any() && ((DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout));
            if (!windows.Any())
                throw new WindowNotFoundException(Logging.WindowException(titles.ToList().Aggregate((x,y)=>x+", "+y)));

            if (windows.Count > 1)
                Logging.MutlipleWindowsWarning(windows);

            returnWindow = windows.First();
            Logging.WindowFound(returnWindow, DateTime.Now - start);

            By.Window = returnWindow;
            if (returnWindow.DisplayState == DisplayState.Minimized)
                returnWindow.DisplayState = DisplayState.Restored;
            returnWindow.Focus();

            return returnWindow;
        }

        public UIControl FindControl(Finder f)
        {
            return new UIControl(f.Result.First(), this);
        }

        public UIControl FindControl(string automationId)
        {
            return new UIControl(By.AutomationId(automationId).Result.First(), this);
        }

        public List<AutomationElement> FindAll(Finder f)
        {
            return f.Result;
        }

        public UIWindow Send(string value)
        {
            switch (value)
            {
                case "{F5}":
                {
                    Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.F5);
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
            return this;
        }
    }
}