using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteExtension
{
    internal class Strings
    {
        public static string TestStarted;
        public static string ControlFound;
        public static string ControlException;
        public static string WindowFound;
        public static string WindowException;
        public static string Click;
        public static string Sent;
        public static string AssertSucceeded;
        public static string AssertException;
        public static string MultipleResultsWarning;
        public static string Contains;

        static Strings()
        {
            if (Config.Language == (int)Config.Languages.En)
            {
                TestStarted = "Test started";
                ControlFound = "Control by \"{0}\" was found in {1} seconds";
                ControlException = "Control by \"{0}\" was not found";
                WindowFound = "Window \"{0}\" was found in {1} seconds";
                WindowException = "Window \"{0}\" was not found";
                Click = "Performed a click on control \"{0}\"";
                Sent = "Sent \"{0}\"";
                AssertSucceeded = "Control \"{0}\" passed assertion";
                AssertException = "Control \"{0}\" didn't pass assertion";
                MultipleResultsWarning = "{0} controls was found";
                Contains = "contains";
            }
            else if (Config.Language == (int)Config.Languages.Ru)
            {
                TestStarted = "Запущен тест";
                ControlFound = "Контрол по условию \"{0}\" найден за {1} секунд";
                ControlException = "Контрол по условию \"{0}\" не найден";
                WindowFound = "Окно \"{0}\" найдено за {1} секунд";
                WindowException = "Окно \"{0}\" не найдено";
                Click = "Выполнен клик по контролу \"{0}\"";
                Sent = "Нажато \"{0}\"";
                AssertSucceeded = "Контрол \"{0}\" успешно прошел проверку";
                AssertException = "Контрол \"{0}\" не прошел проверку";
                MultipleResultsWarning = "Найдено {0} контролa(-ов)";
                Contains = "содержит";
            }
        }

    }
}
