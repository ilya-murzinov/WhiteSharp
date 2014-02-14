using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Automation;

namespace WhiteSharp.Extensions
{
    public static class ShouldlyExtensions
    {
        private static void Enabled(this AutomationElement element)
        {
            DateTime now = DateTime.Now;
            if (!element.Current.IsEnabled)
            {
                do
                {
                    Thread.Sleep(Settings.Default.Delay);
                } while (!element.Current.IsEnabled && (DateTime.Now - now).TotalSeconds < Settings.Default.Timeout);
            }
            if (!element.Current.IsEnabled)
                throw new AssertException(
                    Logging.AssertException(string.Format(Logging.Strings["AssertFailed"],
                        element.GetId(), "Enabled", "Not enabled")));
        }

        private static void NotEnabled(this AutomationElement element)
        {
            DateTime now = DateTime.Now;
            if (element.Current.IsEnabled)
            {
                do
                {
                    Thread.Sleep(Settings.Default.Delay);
                } while (element.Current.IsEnabled && (DateTime.Now - now).TotalSeconds < Settings.Default.Timeout);
            }
            if (element.Current.IsEnabled)
                throw new AssertException(
                    Logging.AssertException(string.Format(Logging.Strings["AssertFailed"],
                        element.GetId(), "Enabled", "Not enabled")));
        }

        /// <summary>
        ///     Checks if every AutomationElement in given list is enabled
        ///     Throws AssertException if the condition not satisfied
        /// </summary>
        /// <param name="list"></param>
        public static void ShouldBeEnabled(this List<AutomationElement> list)
        {
            list.ForEach(x =>
            {
                x.Enabled();
                Logging.AssertSucceeded(x.GetId());
            });
        }

        /// <summary>
        ///     Checks if every Control in given list is enabled
        ///     Throws AssertException if the condition not satisfied
        /// </summary>
        /// <param name="items"></param>
        public static void ShouldBeEnabled(params Control[] items)
        {
            var list = new List<Control>(items);
            List<AutomationElement> result = list.Select(x => items[list.IndexOf(x)].AutomationElement).ToList();
            result.ShouldBeEnabled();
        }

        public static void ShouldBeEnabled(this Control control)
        {
            control.AutomationElement.Enabled();
        }

        /// <summary>
        ///     Checks if every AutomationElement in given list is not enabled
        ///     Throws AssertException if the condition not satisfied
        /// </summary>
        /// <param name="list"></param>
        public static void ShouldNotBeEnabled(this List<AutomationElement> list)
        {
            list.ForEach(x =>
            {
                x.NotEnabled();
                Logging.AssertSucceeded(x.GetId());
            });
        }

        /// <summary>
        ///     Checks if every Control in given list is not enabled
        ///     Throws AssertException if the condition not satisfied
        /// </summary>
        /// <param name="items"></param>
        public static void ShouldNotBeEnabled(params Control[] items)
        {
            var list = new List<Control>(items);
            List<AutomationElement> result = list.Select(x => items[list.IndexOf(x)].AutomationElement).ToList();
            result.ShouldNotBeEnabled();
        }

        public static void ShouldNotBeEnabled(this Control control)
        {
            control.AutomationElement.NotEnabled();
        }
    }
}
