using System.Collections.Generic;
using System.Windows.Automation;

namespace WhiteSharp.Interfaces
{
    //TODO: add adequate descriptions here
    public interface IControl
    {
        AutomationElement AutomationElement { get; }
        bool Enabled { get; }
        string Name { get; }

        Window Window { get; }
        string Identifiers { get; }

        /// <summary>
        /// Finds control inside parent Control.
        /// Index is not nessesary, zero by default. Use this parameter only if there is more than one identical controls.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="index"></param>
        /// <returns>new Control</returns>
        Control FindChild(By searchCriteria, int index);
        /// <summary>
        /// Finds control by AutomationId inside parent Control.
        /// If there is multiple controls with the same AutomationId, use FindChild(By searchCriteria, int index) instead.
        /// </summary>
        /// <param name="automationId"></param>
        /// <returns>new Control</returns>
        Control FindChild(string automationId);
        /// <summary>
        /// Finds control by ControlType inside parent Control.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>new Control</returns>
        Control FindChild(ControlType type);
        /// <summary>
        /// Finds all AutomationElements inside parent Control.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns>List of AutomationElements</returns>
        List<AutomationElement> FindChildren(By searchCriteria);

        bool Exists(By searchCriteria);
        bool Exists(string automationId);
        bool Exists(By searchCriteria, out object o);
        bool Exists(string automationId, out object o);
        Control ClickIfExists(By searchCriteria);
        Control ClickIfExists(string automationId);

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