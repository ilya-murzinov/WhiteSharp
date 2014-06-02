using System.Windows.Automation;

namespace WhiteSharp
{
    public interface IControl : IControlContainer
    {
        AutomationElement AutomationElement { get; }
        bool Enabled { get; }
        IControl ClickAnyway();
        IControl Click();
        IControl DoubleClick();
        IControl WaitForEnabled();
        IControl Wait(int milliseconds);
        IControl DrawHighlight();
        IControl ClickRightEdge();
        IControl ClickLeftEdge();
        IControl Send(Keys key);
        string GetText();
    }
}
