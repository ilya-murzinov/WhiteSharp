using System.Windows.Automation;

namespace WhiteSharp.Interfaces
{
    //TODO: add adequate descriptions here
    public interface IControl
    {
        AutomationElement AutomationElement { get; }
        bool Enabled { get; }
        string Name { get; }
        
        //TODO: make this work. Problem in time leak if Control initializer
        //Window Window { get; }
        string Identifiers { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>this Control</returns>
        Control WaitForEnabled();
        /// <summary>
        /// Pauses playback for given time.
        /// Not supposed to be used in normal situations, use this only in exstreme cases.
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns>this Control</returns>
        Control Wait(int milliseconds);

        /// <summary>
        /// Clicks control after waiting it to get enabled.
        /// </summary>
        /// <returns>this Control</returns>
        Control Click();
        Control DoubleClick();
        /// <summary>
        /// Clicks control without waiting it to get enabled.
        /// </summary>
        /// <returns>this Control</returns>
        Control ClickAnyway();

        Control ClearValue();
        /// <summary>
        /// Sends text to control.
        /// Supported shortcuts: {F5}, {Esc}, {Enter}, {Tab}, {Down}
        /// </summary>
        /// <param name="message"></param>
        /// <returns>this Control</returns>
        Control Send(string message);

        /// <summary>
        /// Selects item from combobox by exact name.
        /// Throws GeneralExcepltion if control is not a combobox.
        /// </summary>
        /// <param name="name"></param>
        Control SelectItem(string name);
        /// <summary>
        /// Selects item from combobox by index.
        /// Throws GeneralExcepltion if control is not a combobox.
        /// </summary>
        /// <param name="index"></param>
        Control SelectItem(int index);

        void SetToggleState(ToggleState state);
        void SelectRadioButton();
        Control DrawHighlight();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetText();
    }
}