using System.Collections.Generic;
using System.Windows.Automation;

namespace WhiteSharp.Interfaces
{
    public interface IUIControl
    {
        /// <summary>
        /// Finds control inside parent UIControl.
        /// Index is not nessesary, zero by default. Use this parameter only if there is more than one identical controls.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="index"></param>
        /// <returns>new UIControl</returns>
        UIControl FindChild(By searchCriteria, int index);
        /// <summary>
        /// Finds control by AutomationId inside parent UIControl.
        /// If there is multiple controls with the same AutomationId, use FindChild(By searchCriteria, int index) instead.
        /// </summary>
        /// <param name="automationId"></param>
        /// <returns>new UIControl</returns>
        UIControl FindChild(string automationId);
        /// <summary>
        /// Finds control by ControlType inside parent UIControl.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>new UIControl</returns>
        UIControl FindChild(ControlType type);
        /// <summary>
        /// Finds all AutomationElements inside parent UIControl.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns>List of AutomationElements</returns>
        List<AutomationElement> FindChildren(By searchCriteria);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>this UIControl</returns>
        UIControl WaitForEnabled();
        /// <summary>
        /// Pauses playback for given time.
        /// Not supposed to be used in normal situations, use this only in exstreme cases.
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns>this UIControl</returns>
        UIControl Wait(int milliseconds);

        /// <summary>
        /// Clicks control after waiting it to get enabled.
        /// </summary>
        /// <returns>this UIControl</returns>
        UIControl Click();
        /// <summary>
        /// Clicks control without waiting it to get enabled.
        /// </summary>
        /// <returns>this UIControl</returns>
        UIControl ClickAnyway();

        /// <summary>
        /// Sends text to control.
        /// Supported shortcuts: {F5}, {Esc}, {Enter}, {Tab}, {Down}
        /// </summary>
        /// <param name="message"></param>
        /// <returns>this UIControl</returns>
        UIControl Send(string message);

        /// <summary>
        /// Selects item from combobox by exact name.
        /// Throws GeneralExcepltion if control is not a combobox.
        /// </summary>
        /// <param name="name"></param>
        UIControl SelectItem(string name);
        /// <summary>
        /// Selects item from combobox by index.
        /// Throws GeneralExcepltion if control is not a combobox.
        /// </summary>
        /// <param name="index"></param>
        UIControl SelectItem(int index);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetText();
    }
}