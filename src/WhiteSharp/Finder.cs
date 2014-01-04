using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using TestStack.White.UIItems.WindowItems;

namespace WhiteSharp
{
    public class Finder
    {
        public List<AutomationElement> Result;

        public Finder()
        {
            Result = By.Window.AutomationElement
                .FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.IsOffscreenProperty, false))
                .OfType<AutomationElement>().ToList();
        }

        private List<AutomationElement> Find(Func<AutomationElement, bool> p, string identifier)
        {
            DateTime start = DateTime.Now;
            List<AutomationElement> list;
            do
            {
                Thread.Sleep(Config.Delay);
                list = Result.Where(p).ToList();
            } while (!list.Any() && ((DateTime.Now - start).TotalMilliseconds < Config.Timeout));
            if (!list.Any())
                throw new ControlNotFoundException(Logging.ControlException(identifier));
            Logging.ControlFound(identifier, DateTime.Now - start);
            if (list.Count() != 1)
                Logging.MutlipleResultsWarning(list);
            return list;
        }

        public Finder AutomationId(string automationId)
        {
            Result = Find(x => x.Current.AutomationId.Equals(automationId), string.Format("AutomationId = {0}", automationId));
            return this;
        }

        public Finder AutomationIdContains(string automationId)
        {
            Result = Find(x => x.Current.AutomationId.Contains(automationId), string.Format("AutomationId {0} {1}", Strings.Contains, automationId));
            return this;
        }

        public Finder Name(String name)
        {
            Result = Find(x => x.Current.Name.Equals(name), string.Format("Name = {0}",name));
            return this;
        }

        public Finder NameContains(String name)
        {
            Result = Find(x => x.Current.Name.Contains(name), string.Format("Name {0} {1}", Strings.Contains, name));
            return this;
        }

        public Finder ClassName(string className)
        {
            Result = Find(x => x.Current.ClassName.Equals(className), string.Format("ClassName = {0}", className));
            return this;
        }

        public Finder ControlType(ControlType type)
        {
            Result = Find(x => x.Current.ControlType.Equals(type), string.Format("ContolType = {0}", type));
            return this;
        }

        public Finder Text(String text)
        {
            Result = Find(x => x.Current.HelpText.Equals(text), string.Format("Text = {0}", text));
            return this;
        }

        public Finder TextContains(String text)
        {
            Result = Find(x => x.Current.HelpText.Contains(text), string.Format("Text {0} {1}",  Strings.Contains, text));
            return this;
        }

        public Finder GridCell(int i, int j)
        {
            return AutomationId("CellElement_" + i + "_" + j);
        }

        public Finder Row(int i)
        {
            return AutomationId("Row_" + i).ClassName("TreeListViewRow");
        }

        public Finder Enabled(bool b)
        {
            Result = Find(x => x.Current.IsEnabled == b, "Enabled = " + b);
            return this;
        }

        public Finder Index(int index)
        {
            Result = new List<AutomationElement> {Result.ElementAt(index)};
            return this;
        }
    }

    public class By
    {
        public static Window Window = null;

        public static Finder AutomationId(string automationId)
        {
            return new Finder().AutomationId(automationId);
        }

        public static Finder AutomationIdContains(string automationId)
        {
            return new Finder().AutomationIdContains(automationId);
        }

        public static Finder Name(String name)
        {
            return new Finder().Name(name);
        }

        public static Finder NameContains(String name)
        {
            return new Finder().NameContains(name);
        }

        public static Finder ClassName(string className)
        {
            return new Finder().ClassName(className);
        }

        public static Finder ControlType(ControlType type)
        {
            return new Finder().ControlType(type);
        }

        public Finder Text(String text)
        {
            return new Finder().Text(text);
        }

        public Finder TextContains(String text)
        {
            return new Finder().TextContains(text);
        }

        public static Finder GridCell(int i, int j)
        {
            return new Finder().GridCell(i, j);
        }

        public static Finder Row(int i)
        {
            return new Finder().Row(i);
        }
    }
}