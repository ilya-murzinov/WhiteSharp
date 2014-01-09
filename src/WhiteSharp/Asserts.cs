using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using TestStack.White;

namespace WhiteSharp
{
    public class AssertThat
    {
        public static void AreEqual(UIControl control, string expected, string actual)
        {
            string msg = null;
            if (!expected.Equals(actual))
            {
                msg = string.Format(Strings.AssertFailed, control.GetId(), expected, actual);
                throw new AssertException(Logging.AssertException(msg));
            }
        }

        public static void StringContains(UIControl control, string value, string actual)
        {
            string msg = null;
            if (!value.Contains(actual))
            {
                msg = string.Format(Strings.AssertFailed, control.GetId(), value, actual);
                throw new AssertException(Logging.AssertException(msg));
            }
        }

        public static void Enabled(List<AutomationElement> list)
        {
            DateTime now = DateTime.Now;
            list.ForEach(x =>
            {
                if (!x.Current.IsEnabled)
                {
                    do
                    {
                        Thread.Sleep(Settings.Default.Delay);
                    } while (!x.Current.IsEnabled && (DateTime.Now - now).TotalSeconds < Settings.Default.Timeout);
                }
                if (!x.Current.IsEnabled)
                    throw new AssertException(
                        Logging.AssertException(string.Format(Strings.AssertFailed,
                            new UIControl(x, Desktop.Instance).GetId(), "Enabled", "Not enabled")));
                Logging.AssertSucceeded(new UIControl(x, Desktop.Instance));
            });
        }

        public static void Enabled(params UIControl[] items)
        {
            var list = new List<UIControl>(items);
            List<AutomationElement> result = list.Select(x => items[list.IndexOf(x)].AutomationElement).ToList();
            Enabled(result);
        }

        public static void NotEnabled(List<AutomationElement> list)
        {
            DateTime now = DateTime.Now;
            list.ForEach(x =>
            {
                if (x.Current.IsEnabled)
                {
                    do
                    {
                        Thread.Sleep(Settings.Default.Delay);
                    } while (x.Current.IsEnabled && (DateTime.Now - now).TotalSeconds < Settings.Default.Timeout);
                }
                if (x.Current.IsEnabled)
                    throw new AssertException(Logging.AssertException(new UIControl(x, Desktop.Instance).GetId()));
                Logging.AssertSucceeded(new UIControl(x, Desktop.Instance));
            });
        }

        public static void NotEnabled(params UIControl[] items)
        {
            var list = new List<UIControl>(items);
            List<AutomationElement> result = list.Select(x => items[list.IndexOf(x)].AutomationElement).ToList();
            NotEnabled(result);
        }
    }
}