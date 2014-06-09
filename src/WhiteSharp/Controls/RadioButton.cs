using System.Threading;
using System.Windows.Automation;

namespace WhiteSharp.Controls
{
    class RadioButton : Control
    {
        public RadioButton(AutomationElement automationElement, IControlContainer window, By searchCriteria, int index) 
            : base(automationElement, window, searchCriteria, index)
        {
        }

        public void Select()
        {
            object objPat;
            if (AutomationElement.TryGetCurrentPattern(SelectionItemPattern.Pattern, out objPat))
            {
                WaitForEnabled();
                var selectionPattern = (SelectionItemPattern)objPat;
                while (!selectionPattern.Current.IsSelected)
                {
                    selectionPattern.Select();
                    Thread.Sleep(Settings.Default.Delay);
                }
            }
            else
            {
                throw new GeneralException(string.Format(Logging.Strings["UnsupportedPattern"],
                    SearchCriteria.Identifiers,
                    "SelectionItemPattern"));
            }
        }
    }
}
