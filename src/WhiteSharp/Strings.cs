namespace WhiteSharp
{
    internal class Strings
    {
        internal static string TestStarted;
        internal static string And;
        internal static string ControlFound;
        internal static string ControlException;
        internal static string WindowFound;
        internal static string WindowException;
        internal static string Click;
        internal static string Sent;
        internal static string AssertSucceeded;
        internal static string MultipleControlsWarning;
        internal static string MultipleWindowsWarning;
        internal static string Contains;
        internal static string AssertFailed;
        internal static string NotACombobox;

        static Strings()
        {
            if (Settings.Default.Language == "En")
            {
                TestStarted = "Test started: {0}";
                And = "and";
                ControlFound = "Control by {0} was found in {1} seconds";
                ControlException = "Control by {0} was not found";
                WindowFound = "Window \"{0}\" was found in {1} seconds";
                WindowException = "Window \"{0}\" was not found";
                Click = "Control \"{0}\" was clicked";
                Sent = "Sent \"{0}\"";
                AssertSucceeded = "Control \"{0}\" passed assertion";
                AssertFailed =
                    "Assertion failed. Control: \"{0}\".\r\n                       Expected: \"{1}\", but was \"{2}\"";
                MultipleControlsWarning = "{0} controls was found";
                MultipleWindowsWarning = "{0} windows was found";
                Contains = "contains";
                NotACombobox = "Control \"{0}\" is not a combobox";
            }
            else if (Settings.Default.Language == "Ru")
            {
                TestStarted = "Запущен тест: {0}";
                And = "и";
                ControlFound = "Контрол по условию {0} найден за {1} секунд";
                ControlException = "Контрол по условию {0} не найден";
                WindowFound = "Окно \"{0}\" найдено за {1} секунд";
                WindowException = "Окно \"{0}\" не найдено";
                Click = "Выполнен клик по контролу \"{0}\"";
                Sent = "Нажато \"{0}\"";
                AssertSucceeded = "Контрол \"{0}\" успешно прошел проверку";
                AssertFailed =
                    "Проверка провалилась. Контрол: \"{0}\".\r\n                       Ожидалось: \"{1}\", но было \"{2}\"";
                MultipleControlsWarning = "Найдено {0} контролa(-ов)";
                MultipleWindowsWarning = "Найдено {0} окон(-на)";
                Contains = "содержит";
                NotACombobox = "Контрол \"{0}\" не является комбобоксом";
            }
        }
    }
}