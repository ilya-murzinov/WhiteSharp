using System.Windows.Automation;

namespace WhiteSharp
{
    public interface IControl : IControlContainer
    {
        bool Enabled { get; }
        string Name { get; }
        IControl ClickAnyway(bool doCheckErrorWindow = false);
        IControl Click();
        IControl DoubleClick();
        IControl WaitForEnabled();
        IControl Wait(int milliseconds);
        IControl DrawHighlight();
        IControl ClickRightEdge();
        IControl ClickLeftEdge();
        IControl Send(Keys key);
        string GetText();
        string GetHelpText();
        string GetName();

        IControl ScrollVertical(ScrollAmount scrollAmount);
        IControl ScrollHorizontal(ScrollAmount scrollAmount);

        void TryFindErrorWindow();
        void Send(string name);
    }
}