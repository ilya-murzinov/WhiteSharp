using System.Windows.Automation;

namespace WhiteSharp.Interfaces
{
    public interface IUIControl
    {
        UIControl FindChild(By searchCriteria, int index);
        UIControl FindChild(string automationId);
        UIControl FindChild(ControlType type);
        UIControl Click();
        UIControl ClickAnyway();
        UIControl Send(string message);
        void SelectItem(string name);
        void SelectItem(int index);
        string GetText();
        string GetId();
    }
}