using System.Collections.Generic;
using System.Windows.Automation;

namespace WhiteSharp
{
    public interface IControlContainer
    {
        AutomationElement AutomationElement { get; }
        T FindControl<T>(By searchCriteria, int index = 0) where T : class, IControl;
        List<T> FindControls<T>(By searchCriteria, int index = 0) where T : class, IControl;
        T FindControl<T>(string automationId, int index = 0) where T : class, IControl;
        T FindControl<T>(ControlType type) where T : class, IControl;
        T FindControl<T>(int index = 0) where T : class, IControl;
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