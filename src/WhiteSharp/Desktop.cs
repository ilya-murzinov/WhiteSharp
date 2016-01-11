using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Automation;
using WhiteSharp.Extensions;

namespace WhiteSharp
{
    public class Desktop
    {
        private static Desktop _instance;

        private static DateTime _start;
        private static List<string> _identifiers = new List<string>();

        private Desktop()
        {
            Windows = AutomationElement.RootElement.FindAll(TreeScope.Children,
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window))
                .OfType<AutomationElement>().ToList();
        }

        internal static string Identifiers
        {
            get { return _identifiers.Aggregate((x, y) => x + ", " + y); }
        }

        internal List<AutomationElement> Windows { get; private set; }

        public static Desktop Instance
        {
            get { return (_instance = new Desktop()); }
        }

        private static List<AutomationElement> FindWindows(Predicate<AutomationElement> predicate, int timeout)
        {
            var windows = new List<AutomationElement>();

            _start = DateTime.Now;

            while (!windows.Any() && ((DateTime.Now - _start).TotalMilliseconds < timeout))
            {
                try
                {
                    windows = Instance.Windows.FindAll(predicate);
                }
                catch (Exception)
                {
                    Thread.Sleep(Settings.Default.Delay);
                }
            }
            if (!windows.Any())
            {
                Exception e = new WindowNotFoundException(string.Format(Logging.Strings["WindowException"], Identifiers));
                Logging.Exception(e);
                throw e;
            }
            return windows;
        }

        public List<AutomationElement> FindWindows(string title, int timeout = 20000)
        {
            _identifiers = new List<string> { title };
            _start = DateTime.Now;
            var regex = new Regex(title);
            List<AutomationElement> windows = FindWindows(window => regex.IsMatch(window.Title()), timeout);
            return windows;
        }

        private static List<AutomationElement> FindWindows(int processId, string title, int timeout = 0)
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
            _identifiers.Add(processId.ToString("D"));
            _identifiers.Add(title);
            var regex = new Regex(title);
            return FindWindows(window => window.Current.ProcessId.Equals(processId), timeout)
                .FindAll(window => regex.IsMatch(window.Title()));
        }

        public AutomationElement SelectWindow(List<AutomationElement> windows)
        {
            if (windows == null || !windows.Any())
                throw new WindowNotFoundException(
                    Logging.WindowException(_identifiers.Aggregate((x, y) => x + ", " + y)));

            var returnWindow = windows.First();

            Logging.WindowFound(returnWindow.Title(), DateTime.Now - _start);

            if (windows.Count > 1)
                Logging.MutlipleWindowsWarning(windows.Count);

            return returnWindow;
        }

        public Window FindWindow(string title)
        {
            return new Window(SelectWindow(FindWindows(title, Settings.Default.Timeout)));
        }

        internal Window FindWindow(int processId, string title)
        {
            return new Window(SelectWindow(FindWindows(processId, title, Settings.Default.Timeout)));
        }

        public List<AutomationElement> FindAll(string title)
        {
            return Windows.FindAll(x => x.Title().Contains(title));
        }

        public List<AutomationElement> FindAll(int processId)
        {
            return Windows.FindAll(x => x.Current.ProcessId == processId);
        }

        public bool TryFindWindow(string title, out Window window, bool doThrowExeption)
        {
            Window outWindow = null;

            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < Settings.Default.Timeout)
            {
                try
                {
                    outWindow = new Window(SelectWindow(FindWindows(title, Settings.Default.Timeout)));
                    window = outWindow;
                    return true;
                }
                catch (WindowNotFoundException)
                {
                }
            }

            if (outWindow == null && doThrowExeption)
                throw new Exception("Окно не найдено! Название = " + title);
            window = outWindow;
            return window != null;
        }

        public void TryFindWindow(string title, out Window window)
        {

            Window outWindow = null;
            try
            {
                outWindow = new Window(SelectWindow(FindWindows(title, Settings.Default.Timeout / 5)));
            }
            catch (WindowNotFoundException)
            {
            }
            window = outWindow;
        }

        public bool TryFindWindow(string title)
        {
            Window outWindow = null;
            try
            {
                outWindow = new Window(SelectWindow(FindWindows(title, Settings.Default.Timeout / 4)));
            }
            catch (WindowNotFoundException)
            {
            }
            return outWindow == null;
        }
    }
}