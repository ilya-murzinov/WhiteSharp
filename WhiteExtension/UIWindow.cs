using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.InputDevices;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;

namespace WhiteExtension
{
    public class UIWindow
    {
        private readonly Window _window;

        public UIWindow()
        {
            
        }

        public UIWindow(string title)
        {
            _window = GetWindow(title);
            _window.DisplayState = DisplayState.Maximized;
            _window.Focus();
        }

        internal Window GetWindow(string title)
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
            return returnWindow;
        }

        public UIControl Find(Finder f)
        {
            return new UIControl(f.Result.First(), _window);
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
            }
            return this;
        }
    }
}