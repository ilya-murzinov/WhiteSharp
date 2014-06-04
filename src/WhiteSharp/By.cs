using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Automation;
using WhiteSharp.Controls;
using WhiteSharp.Extensions;

namespace WhiteSharp
{
    public enum How
    {
        AutomationId,
        AutomationIdContains,
        Name,
        NameContains,
        ControlType,
        ControlTypeContains,
        ClassName
    }

    public class By
    {
        #region Private Fields

        private List<string> _identifiers = new List<string>();
        private List<Predicate<AutomationElement>> _result = new List<Predicate<AutomationElement>>();

        #endregion

        #region Properties

        internal string Identifiers
        {
            get { return _identifiers.Select(x => string.Format("\"{0}\"", x)).Aggregate((x, y) => x + ", " + y); }
            set { _identifiers = new List<string> {value}; }
        }

        internal double Duration { get; set; }

        internal Predicate<AutomationElement> Result
        {
            get
            {
                if (_result.Any())
                {
                    return _result.Aggregate((x, y) => And(x, y));
                }
                return null;
            }
            private set { _result = new List<Predicate<AutomationElement>> {value}; }
        }

        #endregion

        #region Static Methods

        public static By Create(How how, String with)
        {
            switch (how)
            {
                case How.AutomationId:
                    return AutomationId(with);
                case How.ClassName:
                    return ClassName(with);
                case How.Name:
                    return Name(with);
                case How.ControlType:
                    return ControlType(ControlTypeExtensions.FromString(with));
            }
            return null;
        }

        public static By FromControlType(Type t)
        {
            if (t == typeof(TextBox))
            {
                return ControlType(System.Windows.Automation.ControlType.Edit);
            }
            if (t == typeof(Button))
            {
                return ControlType(System.Windows.Automation.ControlType.Button);
            }
            if (t == typeof(ComboBox))
            {
                return ControlType(System.Windows.Automation.ControlType.ComboBox);
            }
            if (t == typeof(CheckBox))
            {
                return ControlType(System.Windows.Automation.ControlType.CheckBox);
            }
            if (t == typeof(RadioButton))
            {
                return ControlType(System.Windows.Automation.ControlType.RadioButton);
            }
            return new By();
        }

        public static By AutomationId(string automationId)
        {
            var b = new By();
            var regex = new Regex(automationId);
            b._result.Add(x => regex.IsMatch(x.Current.AutomationId));
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

        public static By ClassName(string className)
        {
            var b = new By();
            var regex = new Regex(className);
            b._result.Add(x => regex.IsMatch(x.Current.ClassName));
            b._identifiers.Add(String.Format("ClassName = {0}", className));
            return b;
        }

        public static By Name(string name)
        {
            var b = new By();
            var regex = new Regex(name);
            b._result.Add(x => regex.IsMatch(x.Current.Name));
            b._identifiers.Add(String.Format("Name = {0}", name));
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

        public static By Text(string text)
        {
            var b = new By();
            var regex = new Regex(text);
            b._result.Add(x => regex.IsMatch(x.GetText()));
            b._identifiers.Add(String.Format("Text {0} \"{1}\"", Logging.Strings["Contains"], text));
            return b;
        }

        public static By Enabled(bool enabled)
        {
            By b = new By();
            b._result.Add(x => enabled ? x.Current.IsEnabled : !x.Current.IsEnabled);
            b._identifiers.Add(string.Format("Enabled = {0}", enabled));
            return b;
        }

        #endregion

        #region Non-static Methods

        public void Add(By by)
        {
            if (by._result == null || by._result.Count == 0)
            {
                return;
            }
            if (Result == null)
            {
                Result = by.Result;
                Identifiers = by.Identifiers;
            }
            else
            {
                Result = And(Result, by.Result);
                _identifiers.Add(by.Identifiers);
            }
        }

        public By AndAutomationId(string automationId)
        {
            var regex = new Regex(automationId);
            _result.Add(x => regex.IsMatch(x.Current.AutomationId));
            _identifiers.Add(String.Format("AutomationId = {0}", automationId));
            return this;
        }

        public By AndPredicate(Predicate<AutomationElement> predicate)
        {
            _result.Add(predicate);
            _identifiers.Add(String.Format("Predicate = {0}", predicate));
            return this;
        }

        public By AndClassName(string className)
        {
            var regex = new Regex(className);
            _result.Add(x => regex.IsMatch(x.Current.ClassName));
            _identifiers.Add(String.Format("ClassName = {0}", className));
            return this;
        }

        public By AndName(string name)
        {
            var regex = new Regex(name);
            _result.Add(x => regex.IsMatch(x.Current.Name));
            _identifiers.Add(String.Format("Name = {0}", name));
            return this;
        }

        public By AndControlType(ControlType type)
        {
            _result.Add(x => x.Current.ControlType.Equals(type));
            _identifiers.Add(String.Format("ControlType = {0}", type.ProgrammaticName));
            return this;
        }

        public By AndEnabled(bool enabled)
        {
            _result.Add(x => enabled ? x.Current.IsEnabled : !x.Current.IsEnabled);
            _identifiers.Add(string.Format("Enabled = {0}", enabled));
            return this;
        }

        #endregion

        private static Predicate<T> And<T>(params Predicate<T>[] predicates)
        {
            return item => predicates.All(predicate => predicate(item));
        }
    }
}