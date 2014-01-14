using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using TestStack.White.UIItems.WindowItems;
using WhiteSharp.Extensions;

namespace WhiteSharp
{
    public class By
    {
        private DateTime _start = DateTime.Now;

        internal List<string> Identifiers { get; private set; }
        internal TimeSpan Duration { get; private set; }
        internal static Window Window { get; set; }

        internal List<AutomationElement> Result { get; set; }
        
        public By()
        {
            Result = By.Window.AutomationElement
                .FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
        }

        private List<AutomationElement> Find(Func<AutomationElement, bool> p, string identifier)
        {
            List<AutomationElement> list;
            Identifiers.Add(identifier);

            do
            {
                Thread.Sleep(Settings.Default.Delay);
                list = Result.Where(p).ToList();
            } while (!list.Any() && ((DateTime.Now - _start).TotalMilliseconds < Settings.Default.Timeout));
            if (list == null || !list.Any())
                throw new ControlNotFoundException(Logging.ControlException(Identifiers.Select(x => string.Format("\"{0}\"", x))
                    .Aggregate((x, y) => x + " " + Logging.Strings["And"] + " " + y)));

            Duration = DateTime.Now - _start;
            return list;
        }

        #region Static methods
        public static By AutomationId(string automationId)
        {
            return new By().AndAutomationId(automationId);
        }

        public static By AutomationIdContains(string automationId)
        {
            return new By().AndAutomationIdContains(automationId);
        }

        public static By Name(String name)
        {
            return new By().AndName(name);
        }

        public static By NameContains(String name)
        {
            return new By().AndNameContains(name);
        }

        public static By ClassName(string className)
        {
            return new By().AndClassName(className);
        }

        public static By ControlType(ControlType type)
        {
            return new By().AndControlType(type);
        }

        public static By Text(String text)
        {
            return new By().AndText(text);
        }

        public static By TextContains(String text)
        {
            return new By().AndTextContains(text);
        }

        public static By GridCell(int i, int j)
        {
            return new By().AndGridCell(i, j);
        }

        public static By Row(int i)
        {
            return new By().AndRow(i);
        }
        #endregion

        #region Non-static methods
        public By AndAutomationId(string automationId)
        {
            Result = Find(x => x.Current.AutomationId.Equals(automationId),
                string.Format("AutomationId = {0}", automationId));
            return this;
        }

        public By AndAutomationIdContains(string automationId)
        {
            Result = Find(x => x.Current.AutomationId.Contains(automationId),
                string.Format("AutomationId {0} {1}", Logging.Strings["Contains"], automationId));
            return this;
        }

        public By AndName(String name)
        {
            Result = Find(x => x.Current.Name.Equals(name), string.Format("Name = {0}", name));
            return this;
        }

        public By AndNameContains(String name)
        {
            Result = Find(x => x.Current.Name.Contains(name), string.Format("Name {0} {1}", Logging.Strings["Contains"], name));
            return this;
        }

        public By AndClassName(string className)
        {
            Result = Find(x => x.Current.ClassName.Equals(className), string.Format("ClassName = {0}", className));
            return this;
        }

        public By AndControlType(ControlType type)
        {
            Result = Find(x => x.Current.ControlType.Equals(type), string.Format("ContolType = {0}", type));
            return this;
        }

        public By AndText(String text)
        {
            Result = Find(x => x.GetText().Equals(text), string.Format("Text = {0}", text));
            return this;
        }

        public By AndTextContains(String text)
        {
            Result = Find(x => x.GetText().Contains(text), string.Format("Text {0} {1}", Logging.Strings["Contains"], text));
            return this;
        }

        public By AndGridCell(int i, int j)
        {
            return AutomationId("CellElement_" + i + "_" + j);
        }

        public By AndRow(int i)
        {
            return AndAutomationId("Row_" + i).AndClassName("TreeListViewRow");
        }

        public By Enabled(bool b)
        {
            Result = Find(x => x.Current.IsEnabled == b, "Enabled = " + b);
            return this;
        }

        public By Index(int index)
        {
            Result = new List<AutomationElement> {Result.ElementAt(index)};
            return this;
        }
        #endregion
    }
}