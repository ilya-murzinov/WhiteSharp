using System;
using System.Globalization;
using System.Linq;
using System.Windows.Automation;

namespace WhiteSharp.Controls
{
    public class Document : Control
    {
        public Document(AutomationElement automationElement, IControlContainer window, By searchCriteria, int index)
            : base(automationElement, window, searchCriteria, index)
        {
        }

        public string GetAllText()
        {
            var tp = (TextPattern)AutomationElement.GetCurrentPattern(TextPattern.Pattern);
            var text = tp.DocumentRange.GetText(-1).Trim();
            return text;
        }

        public string FindActualRecording()
        {
            var text = GetAllText();
            var rows = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var length = rows.GetLength(0);
            if (rows.Equals(null) || !rows.Any()) return text;
            for (var i = 0; i < length; i++)
            {
                var date = DateTime.Now.ToString("T",
                    CultureInfo.CreateSpecificCulture("es-ES"));
                if (rows[i].Contains(date)) text = rows[i];
            }
            return text;
        }

        public string FindLastRow()
        {
            var text = GetAllText();
            var rows = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var length = rows.GetLength(0);
            return (rows.Equals(null) || !rows.Any()) ? text : rows[length - 1];
        }

        public bool IsContainsInLastRow(string подстрока)
        {
            return FindLastRow().Contains(подстрока);
        }
    }
}
