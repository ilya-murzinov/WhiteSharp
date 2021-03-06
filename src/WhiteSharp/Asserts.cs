﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using WhiteSharp.Extensions;

namespace WhiteSharp
{
    public class AssertThat
    {
        /// <summary>
        ///     Throws AssertException if the condition not satisfied
        /// </summary>
        /// <param name="control">Target control</param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void AreEqual(Control control, string expected, string actual)
        {
            if (!expected.Equals(actual))
            {
                string msg = string.Format(Logging.Strings["AssertFailed"], control.GetId(), expected, actual);
                throw new AssertException(Logging.AssertException(msg));
            }
        }

        /// <summary>
        ///     Throws AssertException if the condition not satisfied
        /// </summary>
        /// <param name="control">Target control</param>
        /// <param name="value"></param>
        /// <param name="actual"></param>
        public static void StringContains(Control control, string value, string actual)
        {
            if (!value.Contains(actual))
            {
                string msg = string.Format(Logging.Strings["AssertFailed"], control.GetId(), value, actual);
                throw new AssertException(Logging.AssertException(msg));
            }
        }

        /// <summary>
        ///     Checks if every AutomationElement in given list is enabled
        ///     Throws AssertException if the condition not satisfied
        /// </summary>
        /// <param name="list"></param>
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
                        Logging.AssertException(string.Format(Logging.Strings["AssertFailed"],
                            x.GetId(), "Enabled", "Not enabled")));
                Logging.AssertSucceeded(x.GetId());
            });
        }

        /// <summary>
        ///     Checks if every Control in given list is enabled
        ///     Throws AssertException if the condition not satisfied
        /// </summary>
        /// <param name="items"></param>
        public static void Enabled(params Control[] items)
        {
            var list = new List<Control>(items);
            List<AutomationElement> result = list.Select(x => items[list.IndexOf(x)].AutomationElement).ToList();
            Enabled(result);
        }

        /// <summary>
        ///     Checks if every AutomationElement in given list is not enabled
        ///     Throws AssertException if the condition not satisfied
        /// </summary>
        /// <param name="list"></param>
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
                    throw new AssertException(
                        Logging.AssertException(string.Format(Logging.Strings["AssertFailed"],
                            x.GetId(), "Not enabled", "Enabled")));
                Logging.AssertSucceeded(x.GetId());
            });
        }

        /// <summary>
        ///     Checks if every Control in given list is not enabled
        ///     Throws AssertException if the condition not satisfied
        /// </summary>
        /// <param name="items"></param>
        public static void NotEnabled(params Control[] items)
        {
            var list = new List<Control>(items);
            List<AutomationElement> result = list.Select(x => items[list.IndexOf(x)].AutomationElement).ToList();
            NotEnabled(result);
        }
    }
}