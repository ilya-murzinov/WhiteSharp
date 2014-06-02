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

        private List<AutomationElement> FindWindows(Predicate<AutomationElement> predicate, int timeout)
        {
            var windows = new List<AutomationElement>();

            _start = DateTime.Now;

            while (!windows.Any() && ((DateTime.Now - _start).TotalMilliseconds < timeout))
            {
                try
                {
                    windows = Instance.Windows.FindAll(predicate);
                }
                catch (Exception e)
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

        private List<AutomationElement> FindWindows(string title, int timeout)
        {
            _identifiers = new List<string> {title};
            var windows = new List<AutomationElement>();
            _start = DateTime.Now;
            var regex = new Regex(title);
            windows = FindWindows(window => regex.IsMatch(window.Title()), timeout);
            return windows;
        }

        private List<AutomationElement> FindWindows(int processId, string title, int timeout)
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
            _identifiers.Add(title);
            var regex = new Regex(title);
            return FindWindows(window => window.Current.ProcessId.Equals(processId), timeout)
                .FindAll(window => regex.IsMatch(window.Title()));
        }

        private AutomationElement SelectWindow(List<AutomationElement> windows)
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

        internal Window FindWindow(string title)
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

        public void TryFindWindow(string title, out Window window)
        {
            Window outWindow = null;
            try
            {
                outWindow = new Window(SelectWindow(FindWindows(title, Settings.Default.Timeout/10)));
            }
            catch (WindowNotFoundException)
            {
            }
            window = outWindow;
        }
    }
}