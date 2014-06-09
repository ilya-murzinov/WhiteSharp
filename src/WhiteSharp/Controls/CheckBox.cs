using System.Windows.Automation;

namespace WhiteSharp.Controls
{
    public class CheckBox : Control
    {
        public CheckBox(AutomationElement automationElement, IControlContainer window, By searchCriteria, int index) 
            : base(automationElement, window, searchCriteria, index)
        {
        }

        public void SetToggleState(ToggleState state)
        {
            object objPat;
            if (AutomationElement.TryGetCurrentPattern(TogglePattern.Pattern, out objPat))
            {
                var togglePattern = (TogglePattern)objPat;
                if (togglePattern.Current.ToggleState != state)
                {
                    togglePattern.Toggle();
                }
            }
            else
            {
                throw new GeneralException(string.Format(Logging.Strings["UnsupportedPattern"],
                    SearchCriteria.Identifiers,
                    "TogglePattern"));
            }
        }

        public void SetToggleState(bool toggled)
        {
            SetToggleState(toggled ? ToggleState.On : ToggleState.Off);
        }
    }
}
