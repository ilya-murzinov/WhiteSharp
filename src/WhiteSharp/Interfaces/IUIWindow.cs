using System.Collections.Generic;
using System.Windows.Automation;

namespace WhiteSharp.Interfaces
{
    public interface IUIWindow
    {
        int ProcessId { get; }

        UIControl FindControl(By searchCriteria, int index);
        UIControl FindControl(string automationId);
        UIControl FindControl(ControlType type);
        List<AutomationElement> FindAll(By searchCriteria);
        void Send(string message);
    }
}