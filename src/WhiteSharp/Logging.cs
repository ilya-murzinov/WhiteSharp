using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Automation;
using TestStack.White.UIItems.WindowItems;

namespace WhiteSharp
{
    public class Logging
    {
        private const string StartOpenTag = "\r\n\r\n------------------------------------------------------------------------------------------\r\n";
        private const string StartCloseTag = "\r\n------------------------------------------------------------------------------------------";
        private const string FoungTag = "Found";
        private const string ClickTag = "Click";
        private const string SentTag = "Sent";
        private const string AssertTag = "Assert";
        private const string WarningTag = "Warning";
        private const string ExceptionTag = "Exception";

        private static String Tag(string tag)
        {
            return string.Format("[{0} - {1}] ", DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss"), tag);
        }

        public static void Write(string msg)
        {
            switch (Config.Output)
            {
                case (int)Config.OutputLevel.Trace:
                {
                    Trace.WriteLine(msg);
                    break;
                }
                case (int)Config.OutputLevel.Standart:
                {
                    Console.WriteLine(msg);
                    break;
                }
                case (int)Config.OutputLevel.File:
                {
                    break;
                }
            }
        }

        public static void Start(string msg)
        {
            Write(StartOpenTag + msg + StartCloseTag);
        }

        public static void Decsription(string description)
        {
            Write("\r\n" + description.ToUpper() + "\r\n");
        }

        public static string ControlFound(string msg, TimeSpan duration)
        {
            string s = string.Format(Strings.ControlFound, msg, duration.TotalSeconds);
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
            Write(Tag(ClickTag) + s);
            return s;
        }

        public static string Sent(string msg)
        {
            string s = string.Format(Strings.Sent, msg);
            Write(Tag(SentTag) + s);
            return s;
        }

        public static string AssertSucceeded(UIControl control)
        {
            string s = string.Format(Strings.AssertSucceeded, control.GetId());
            Write(Tag(AssertTag) + s);
            return s;
        }

        public static string MutlipleResultsWarning(List<AutomationElement> list)
        {
            string s = string.Format(Strings.MultipleResultsWarning, list.Count);
            Write(Tag(WarningTag) + s);
            return s;
        }

        public static string ControlException(string msg)
        {
            string s = string.Format(Strings.ControlException, msg);
            Write(Tag(ExceptionTag) + s);
            return s;
        }

        public static string WindowException(string msg)
        {
            string s = string.Format(Strings.WindowException, msg);
            Write(Tag(ExceptionTag) + s);
            return s;
        }

        public static string AssertException(string msg)
        {
            string s = string.Format(Strings.AssertException, msg);
            Write(Tag(ExceptionTag) + s);
            return s;
        }

        public static string Exception(Exception e)
        {
            Write(Tag(ExceptionTag) + e.Message);
            return e.Message;
        }
    }
}