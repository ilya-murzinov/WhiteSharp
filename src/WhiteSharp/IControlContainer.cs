using System.Collections.Generic;
using System.Windows.Automation;

namespace WhiteSharp
{
    public interface IControlContainer
    {
        T FindControl<T>(By searchCriteria, int index = 0);
        T FindControl<T>(string automationId, int index = 0);
        T FindControl<T>(ControlType type);
        IControl FindControl(By searchCriteria, int index = 0);
        IControl FindControl(string automationId, int index = 0);
        List<AutomationElement> FindAll(By searchCriteria);
        bool Exists(By searchCriteria);
        bool Exists(string automationId);
        bool Exists(By searchCriteria, out object o);
        bool Exists(string automationId, out object o);
        IControl ClickIfExists(By searchCriteria);
        IControl ClickIfExists(string automationId);
    }
}