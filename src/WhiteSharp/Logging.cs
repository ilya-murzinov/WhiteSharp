using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Automation;

namespace WhiteSharp
{
    public class Logging
    {
        #region Strings

        public static readonly Dictionary<string, string> Strings = new Dictionary<string, string>();

        private static readonly Dictionary<string, string> StringsRu = new Dictionary<string, string>
        {
            {"ProcessNotFound", "Process with given Id was not found"},
            {"TestStarted", "Запущен тест: {0}"},
            {"And", "и"},
            {"ControlFound", "Контрол по условию {0} найден за {1} секунд"},
            {"ControlNotFoundException", "Контрол по условию {0} не найден"},
            {"ControlNotEnabledException", "Контрол {0} недоступен"},
            {"WindowFound", "Окно \"{0}\" найдено за {1} секунд"},
            {"WindowException", "Окно \"{0}\" не найдено"},
            {"Click", "Выполнен клик по контролу {0}"},
            {"ItemSelected", "Выбран элемент \"{0}\" из комбобокса {1}"},
            {"Sent", "Нажато \"{0}\""},
            {"AssertSucceeded", "Контрол \"{0}\" успешно прошел проверку"},
            {
                "AssertFailed",
                "Проверка провалилась. Контрол: \"{0}\".\r\n                       Ожидалось: \"{1}\", но было \"{2}\""
            },
            {"MultipleControlsWarning", "Найдено {0} контролa(-ов)"},
            {"MultipleWindowsWarning", "Найдено {0} окон(-на)"},
            {"WindowClosed", "Окно \"{0}\" закрыто"},
            {"Contains", "сожержит"},
            {"NotACombobox", "Контрол \"{0}\" не является комбобоксом"},
            {"NotARadioButton", "Контрол {0} не является радио-кнопкой"},
            {"UnsupportedPattern", "Контрол {0} не поддерживает паттерн {1}"}
        };

        private static readonly Dictionary<string, string> StringsEn = new Dictionary<string, string>
        {
            {"ProcessNotFound", "Не найден процесс с заданным Id"},
            {"TestStarted", "Test started: {0}"},
            {"And", "and"},
            {"ControlFound", "Control by {0} was found in {1} seconds"},
            {"ControlNotFoundException", "Control by {0} was not found"},
            {"ControlNotEnabledException", "Control {0} is not enabled"},
            {"WindowFound", "Window \"{0}\" was found in {1} seconds"},
            {"WindowException", "Window \"{0}\" was not found"},
            {"Click", "Control {0} was clicked"},
            {"ItemSelected", "Item \"{0}\" was selected from combobox {1}"},
            {"Sent", "Sent \"{0}\""},
            {"AssertSucceeded", "Control \"{0}\" passed assertion"},
            {
                "AssertFailed",
                "Assertion failed. Control: \"{0}\".\r\n                       Expected: \"{1}\", but was \"{2}\""
            },
            {"MultipleControlsWarning", "{0} controls was found"},
            {"MultipleWindowsWarning", "{0} windows was found"},
            {"WindowClosed", "Window \"{0}\" was closed"},
            {"Contains", "contains"},
            {"NotACombobox", "Control \"{0}\" is not a combobox"},
            {"NotARadioButton", "Control {0} is noy a radio button"},
            {"UnsupportedPattern", "Control {0} doesn't support {1} pattern"}
        };

        #endregion

        #region Constructor

        static Logging()
        {
            if (Settings.Default.Language.Equals("En"))
                Strings = new Dictionary<string, string>(StringsEn);
            if (Settings.Default.Language.Equals("Ru"))
                Strings = new Dictionary<string, string>(StringsRu);
        }

        #endregion

        #region Tags

        private const string StartOpenTag =
            "\r\n\r\n------------------------------------------------------------------------------------------";

        private const string StartCloseTag =
            "------------------------------------------------------------------------------------------";

        private const string FoungTag = "Found";
        private const string ActionTag = "Action";
        private const string AssertTag = "Assert";
        private const string WarningTag = "Warning";
        private const string ExceptionTag = "Exception";
        private const string InfoTag = "Info";

        private class Tag
        {
            private readonly string _tag;

            public Tag(string tag)
            {
                _tag = string.Format("[{0} - {1}] ", DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss"), tag);
            }

            public override string ToString()
            {
                return _tag;
            }
        }

        #endregion

        #region Private Members

        private static void Write(string msg, bool writeToStdOut = true)
        {
            if (writeToStdOut)
            {
                Console.WriteLine(msg);
                Trace.WriteLine(msg);
            }
            else
                Trace.WriteLine(msg);
        }

        #endregion

        #region Public Members

        public static void Start(string msg)
        {
            Write(StartOpenTag);
            Write(String.Format(Strings["TestStarted"], msg));
            Write(StartCloseTag);
        }

        public static void Description(string description)
        {
            Write("\r\n" + description.ToUpper() + "\r\n");
        }

        public static string Info(string msg)
        {
            string s = new Tag(InfoTag) + msg;
            Write(s);
            return s;
        }

        public static string ControlFound(By b)
        {
            string s = string.Format(Strings["ControlFound"], b.Identifiers, b.Duration);
            Write(new Tag(FoungTag) + s, false);
            return s;
        }

        public static string WindowFound(string windowTitle, TimeSpan duration)
        {
            string s = string.Format(Strings["WindowFound"], windowTitle, duration.TotalSeconds);
            Write(new Tag(FoungTag) + s, false);
            return s;
        }

        public static string Click(string identifiers)
        {
            string s = string.Format(Strings["Click"], identifiers);
            Write(new Tag(ActionTag) + s);
            return s;
        }

        public static string ItemSelected(string item, string id)
        {
            string s = string.Format(Strings["ItemSelected"], item, id);
            Write(new Tag(ActionTag) + s);
            return s;
        }

        public static string Sent(string msg)
        {
            string s = string.Format(Strings["Sent"], msg);
            Write(new Tag(ActionTag) + s);
            return s;
        }

        public static string AssertSucceeded(string identifiers)
        {
            string s = string.Format(Strings["AssertSucceeded"], identifiers);
            Write(new Tag(AssertTag) + s, false);
            return s;
        }

        public static string MutlipleControlsWarning(List<AutomationElement> list)
        {
            string s = string.Format(Strings["MultipleControlsWarning"], list.Count);
            Write(new Tag(WarningTag) + s, false);
            return s;
        }

        public static string MutlipleWindowsWarning(int count)
        {
            string s = string.Format(Strings["MultipleWindowsWarning"], count);
            Write(new Tag(WarningTag) + s, false);
            return s;
        }

        public static string WindowClosed(string title)
        {
            string s = string.Format(Strings["WindowClosed"], title);
            Write(new Tag(ActionTag) + s);
            return s;
        }

        public static string ControlNotFoundException(string id)
        {
            string s = string.Format(Strings["ControlNotFoundException"], id);
            Write(new Tag(ExceptionTag) + s);
            return s;
        }

        public static string ControlNotEnabledException(string id)
        {
            string s = string.Format(Strings["ControlNotEnabledException"], id);
            Write(new Tag(ExceptionTag) + s);
            return s;
        }

        public static string WindowException(string id)
        {
            string s = string.Format(Strings["WindowException"], id);
            Write(new Tag(ExceptionTag) + s);
            return s;
        }

        public static string AssertException(string msg)
        {
            Write(new Tag(ExceptionTag) + msg);
            return msg;
        }

        public static string Exception(Exception e)
        {
            Write(new Tag(ExceptionTag) + e.Message + "\n" + e.StackTrace, false);
            return e.Message;
        }

        #endregion
    }
}