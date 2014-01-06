using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteSharp
{
    internal class Strings
    {
        internal static string TestStarted;
        internal static string ControlFound;
        internal static string ControlException;
        internal static string WindowFound;
        internal static string WindowException;
        internal static string Click;
        internal static string Sent;
        internal static string AssertSucceeded;
        internal static string MultipleResultsWarning;
        internal static string Contains;
        internal static string AssertFailed;
        internal static string NotACombobox; 

        static Strings()
        {
            if (Config.Language == (int)Config.Languages.En)
            {
                TestStarted = "Test started: {0}";
                ControlFound = "Control by \"{0}\" was found in {1} seconds";
                ControlException = "Control by \"{0}\" was not found";
                WindowFound = "Window \"{0}\" was found in {1} seconds";
                WindowException = "Window \"{0}\" was not found";
                Click = "Control \"{0}\" was clicked";
                Sent = "Sent \"{0}\"";
                AssertSucceeded = "Control \"{0}\" passed assertion";
                AssertFailed = "Assertion failed. Control: \"{0}\".\r\n                       Expected: \"{1}\", but was \"{2}\"";
                MultipleResultsWarning = "{0} controls was found";
                Contains = "contains";
                NotACombobox = "Control \"{0}\" is not a combobox";
            }
            else if (Config.Language == (int)Config.Languages.Ru)
            {
                TestStarted = "Запущен тест: {0}";
                ControlFound = "Контрол по условию \"{0}\" найден за {1} секунд";
                ControlException = "Контрол по условию \"{0}\" не найден";
                WindowFound = "Окно \"{0}\" найдено за {1} секунд";
                WindowException = "Окно \"{0}\" не найдено";
                Click = "Выполнен клик по контролу \"{0}\"";
                Sent = "Нажато \"{0}\"";
                AssertSucceeded = "Контрол \"{0}\" успешно прошел проверку";
                AssertFailed = "Проверка провалилась. Контрол: \"{0}\".\r\n                       Ожидалось: \"{1}\", но было \"{2}\"";
                MultipleResultsWarning = "Найдено {0} контролa(-ов)";
                Contains = "содержит";
                NotACombobox = "Контрол \"{0}\" не является комбобоксом";
            }
        }
    }
}
