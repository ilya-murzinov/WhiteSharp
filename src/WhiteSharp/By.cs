using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using WhiteSharp.Extensions;

namespace WhiteSharp
{
    public class By
    {
        #region Private Fields

        private readonly List<string> _identifiers = new List<string>();
        private readonly List<Predicate<AutomationElement>> _result = new List<Predicate<AutomationElement>>();

        #endregion

        #region Properties

        internal string Identifiers
        {
            get { return _identifiers.Select(x => string.Format("\"{0}\"", x)).Aggregate((x, y) => x + ", " + y); }
        }

        internal double Duration { get; set; }

        internal Predicate<AutomationElement> Result
        {
            get { return _result.Aggregate((x, y) => And(x, y)); }
        }

        #endregion

        #region Static Methods

        public static By AutomationId(string automationId)
        {
            var b = new By();
            b._result.Add(x => x.Current.AutomationId.Equals(automationId));
            b._identifiers.Add(String.Format("AutomationId = {0}", automationId));
            return b;
        }

        public static By Predicate(Predicate<AutomationElement> predicate)
        {
            var b = new By();
            b._result.Add(predicate);
            b._identifiers.Add(String.Format("Predicate = {0}", predicate));
            return b;
        }

        public static By AutomationIdContains(string automationId)
        {
            var b = new By();
            b._result.Add(x => x.Current.AutomationId.Contains(automationId));
            b._identifiers.Add(String.Format("AutomationId {0} {1}", Logging.Strings["Contains"], automationId));
            return b;
        }

        public static By TextContains(string text)
        {
            var b = new By();
            b._result.Add(x => x.GetText().Contains(text));
            b._identifiers.Add(String.Format("Text: \"{0}\"", text));
            return b;
        }

        public static By ClassName(string className)
        {
            var b = new By();
            b._result.Add(x => x.Current.ClassName.Equals(className));
            b._identifiers.Add(String.Format("ClassName = {0}", className));
            return b;
        }

        public static By Name(string name)
        {
            var b = new By();
            b._result.Add(x => x.Current.Name.ToLower().Equals(name.ToLower()));
            b._identifiers.Add(String.Format("Name = {0}", name));
            return b;
        }

        public static By NameContains(string name)
        {
            var b = new By();
            b._result.Add(x => x.Current.Name.ToLower().Contains(name.ToLower()));
            b._identifiers.Add(String.Format("Name {0} {1}", Logging.Strings["Contains"], name));
            return b;
        }

        public static By GridCell(int i, int j)
        {
            var b = new By();
            b._result.Add(x =>
            {
                object o;
                if (x.TryGetCurrentPattern(TableItemPattern.Pattern, out o))
                {
                    var pattern = (TableItemPattern) o;
                    if (pattern.Current.Column.Equals(i) && pattern.Current.Row.Equals(j))
                        return true;
                }
                return false;
            });
            b._identifiers.Add(String.Format("GridCell = {0}, {1}", i, j));
            return b;
        }

        public static By ControlType(ControlType type)
        {
            var b = new By();
            b._result.Add(x => x.Current.ControlType.Equals(type));
            b._identifiers.Add(String.Format("ControlType = {0}", type.ProgrammaticName));
            return b;
        }

        #endregion

        #region Non-static Methods

        public By AndAutomationId(string automationId)
        {
            _result.Add(x => x.Current.AutomationId.Equals(automationId));
            _identifiers.Add(String.Format("AutomationId = {0}", automationId));
            return this;
        }

        public By AndPredicate(Predicate<AutomationElement> predicate)
        {
            _result.Add(predicate);
            _identifiers.Add(String.Format("Predicate = {0}", predicate));
            return this;
        }

        public By AndAutomationIdContains(string automationId)
        {
            _result.Add(x => x.Current.AutomationId.Contains(automationId));
            _identifiers.Add(String.Format("AutomationId {0} {1}", Logging.Strings["Contains"], automationId));
            return this;
        }

        public By AndClassName(string className)
        {
            _result.Add(x => x.Current.ClassName.Equals(className));
            _identifiers.Add(String.Format("ClassName = {0}", className));
            return this;
        }

        public By AndName(string name)
        {
            _result.Add(x => x.Current.Name.ToLower().Equals(name.ToLower()));
            _identifiers.Add(String.Format("Name = {0}", name));
            return this;
        }

        public By AndNameContains(string name)
        {
            _result.Add(x => x.Current.Name.ToLower().Contains(name.ToLower()));
            _identifiers.Add(String.Format("Name {0} {1}", Logging.Strings["Contains"], name));
            return this;
        }

        public By AndControlType(ControlType type)
        {
            _result.Add(x => x.Current.ControlType.Equals(type));
            _identifiers.Add(String.Format("ControlType = {0}", type.ProgrammaticName));
            return this;
        }

        public By AndEnabled(bool b)
        {
            _result.Add(x => x.Current.IsEnabled);
            _identifiers.Add(string.Format("Enabled = {0}", b));
            return this;
        }

        #endregion

        private static Predicate<T> And<T>(params Predicate<T>[] predicates)
        {
            return item => predicates.All(predicate => predicate(item));
        }
    }
}