using System.Collections.Generic;
using System.Windows.Automation;

namespace WhiteSharp.Interfaces
{
    //TODO: add adequate descriptions here
    public interface IWindow
    {
        /// <summary>
        /// ProcessId of window.
        /// </summary>
        int ProcessId { get; }

        AutomationElement AutomationElement { get; }

        string Title { get; }

        WindowVisualState DisplayState { get; }       

        /// <summary>
        /// Sends shortcut to the window.
        /// Supported shortcuts: {F5}, {Esc}, {Tab}, {Alt}+{F4}, ^{Enter}
        /// </summary>
        /// <param name="message"></param>
        void Send(string message);
    }
}