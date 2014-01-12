using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Automation;
using TestStack.White.UIItems.WindowItems;

namespace WhiteSharp
{
    public class Logging
    {
        public enum OutputLevel
        {
            Trace = 1,
            Standart = 2
        }

        private const string StartOpenTag =
            "\r\n\r\n------------------------------------------------------------------------------------------";

        private const string StartCloseTag =
            "------------------------------------------------------------------------------------------";

        private const string FoungTag = "Found";
        private const string ActionTag = "Action";
        private const string AssertTag = "Assert";
        private const string WarningTag = "Warning";
        private const string ExceptionTag = "Exception";
        public static int Output = (int) OutputLevel.Standart;

        private static String Tag(string tag)
        {
            return string.Format("[{0} - {1}] ", DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss"), tag);
        }

        public static void Write(string msg)
        {
            switch (Settings.Default.OutputLevel)
            {
                case (int) OutputLevel.Trace:
                {
                    Trace.WriteLine(msg);
                    break;
                }
                case (int) OutputLevel.Standart:
                {
                    Console.WriteLine(msg);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        public static void Start(string msg)
        {
            Write(StartOpenTag);
            Write(String.Format(Strings.TestStarted, msg));
            Write(StartCloseTag);
        }

        public static void Decsription(string description)
        {
            Write("\r\n" + description.ToUpper() + "\r\n");
        }

        public static string ControlFound(Finder f)
        {
            string s = string.Format(Strings.ControlFound, f.Identifiers.Select(x => string.Format("\"{0}\"", x))
                .Aggregate((x, y) => x + " " + Strings.And + " " + y), f.Duration.Seconds);
            Write(Tag(FoungTag) + s);
            return s;
        }

        public static string WindowFound(Window window, TimeSpan duration)
        {
            string s = string.Format(Strings.WindowFound, window.Title, duration.TotalSeconds);
            Write(Tag(FoungTag) + s);
            return s;
        }

        public static string Click(UIControl control)
        {
            string s = string.Format(Strings.Click, control.GetId());
            Write(Tag(ActionTag) + s);
            return s;
        }

        public static string Sent(string msg)
        {
            string s = string.Format(Strings.Sent, msg);
            Write(Tag(ActionTag) + s);
            return s;
        }

        public static string AssertSucceeded(UIControl control)
        {
            string s = string.Format(Strings.AssertSucceeded, control.GetId());
            Write(Tag(AssertTag) + s);
            return s;
        }

        public static string MutlipleControlsWarning(List<AutomationElement> list)
        {
            string s = string.Format(Strings.MultipleControlsWarning, list.Count);
            Write(Tag(WarningTag) + s);
            return s;
        }

        public static string MutlipleWindowsWarning(List<Window> list)
        {
            string s = string.Format(Strings.MultipleWindowsWarning, list.Count);
            Write(Tag(WarningTag) + s);
            return s;
        }

        public static string ControlException(string id)
        {
            string s = string.Format(Strings.ControlException, id);
            Write(Tag(ExceptionTag) + s);
            return s;
        }

        public static string WindowException(string id)
        {
            string s = string.Format(Strings.WindowException, id);
            Write(Tag(ExceptionTag) + s);
            return s;
        }

        public static string AssertException(string msg)
        {
            Write(Tag(ExceptionTag) + msg);
            return msg;
        }

        public static string Exception(Exception e)
        {
            Write(Tag(ExceptionTag) + e.Message);
            return e.Message;
        }
    }
}