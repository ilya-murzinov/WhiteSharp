using System.Collections.Generic;
using System.Windows.Automation;

namespace WhiteSharp.Interfaces
{
    public interface IUIWindow
    {
        /// <summary>
        /// ProcessId of window.
        /// </summary>
        int ProcessId { get; }

        List<AutomationElement> BaseAutomationElementList { get;  }

        /// <summary>
        /// Finds control inside the window.
        /// Index is not nessesary, zero by default. Use this parameter if there is more than one identical controls.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        UIControl FindControl(By searchCriteria, int index);

        /// <summary>
        /// Finds control by AutomationId inside the window.
        /// If there is multiple controls with the same AutomationId, use FindChild(By searchCriteria, int index) instead.
        /// You can combine searc criteria as you want, e.g.
        /// <example>FindControl(By.AutomationId("OpenVerticalSplitterButton").AndName("Launch Vertical GridSplitter Window")
        ///     .AndClassName("Button"));</example>
        /// </summary>
        /// <param name="automationId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        UIControl FindControl(string automationId, int index);
        /// <summary>
        /// Finds control by ControlType inside the window.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        UIControl FindControl(ControlType type);
        /// <summary>
        /// Finds all AutomationElements inside the window.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        List<AutomationElement> FindAll(By searchCriteria);

        /// <summary>
        /// Sends shortcut to the window.
        /// Supported shortcuts: {F5}, {Esc}, {Tab}, {Alt}+{F4}, ^{Enter}
        /// </summary>
        /// <param name="message"></param>
        void Send(string message);
    }
}