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

namespace WhiteExtension
{
    public class UIWindow : Window
    {
        #region NotImplemented
        public override Window ModalWindow(SearchCriteria searchCriteria, InitializeOption option)
        {
            throw new NotImplementedException();
        }
        public override Window ModalWindow(string title, InitializeOption option)
        {
            throw new NotImplementedException();
        }
        public override PopUpMenu Popup
        {
            get
            {
                throw new NotImplementedException();
            }
        } 
        #endregion
        
        public UIWindow(string title) : base(FindWindow(title).AutomationElement, TestStack.White.Factory.InitializeOption.NoCache, 
                new TestStack.White.Sessions.WindowSession(new TestStack.White.Sessions.ApplicationSession(), 
                    TestStack.White.Factory.InitializeOption.NoCache))
        {
            this.DisplayState = DisplayState.Restored;
            this.Focus();
        }

        public static Window FindWindow(string title)
        {
            Window returnWindow = null;
            DateTime start = DateTime.Now;
            do
            {
                try
                {
                    returnWindow = Desktop.Instance.Windows().Find(window => window.Title.Contains(title));
                }
                catch (ElementNotAvailableException e)
                {
                    Logging.Exception(e);
                }
                Thread.Sleep(Config.Delay);
            } while (returnWindow == null && ((DateTime.Now - start).TotalMilliseconds < Config.Timeout));
            if (returnWindow == null)
                throw new WindowNotFoundException(Logging.WindowException(title));
            Logging.WindowFound(returnWindow, DateTime.Now - start);
            By.Window = returnWindow;
            returnWindow.DisplayState = DisplayState.Maximized;
            returnWindow.Focus();
            return returnWindow;
        }

        public UIControl FindControl(Finder f)
        {
            return new UIControl(f.Result.First(), this);
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