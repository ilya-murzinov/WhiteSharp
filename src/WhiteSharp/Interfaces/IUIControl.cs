namespace WhiteSharp.Interfaces
{
    internal interface IUIControl
    {
        UIControl FindChild(Finder finder);
        UIControl Click();
        UIControl ClickAnyway();
        UIControl Send(string message);
        void SelectItem(string name);
        void SelectItem(int index);
        string GetText();
        string GetId();
    }
}