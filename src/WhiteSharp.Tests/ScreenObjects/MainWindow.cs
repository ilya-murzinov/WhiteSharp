namespace WhiteSharp.Tests.ScreenObjects
{
    internal class MainWindow
    {
        private static MainWindow _instance;
        protected string Title = "MainWindow";
        private readonly Control _listViewButton;
        public Window Window;
        
        protected MainWindow()
        {
            Window = new Window(Title);
            _listViewButton = Window.FindControl("OpenListView");
        }

        public static MainWindow Instance
        {
            get { return (_instance = new MainWindow()); }
        }

        
        public ListViewWindow OpenListViewWindow()
        {
            _listViewButton.Click();
            return ListViewWindow.Instance;
        }
        
        public MainWindow OpenMessageBox()
        {
            Window.FindControl("OpenMessageBox").Click();
            return this;
        }

        public MainWindow CloseMessageBox()
        {
            Window.FindModalWindow("Test message box").FindControl("2").Click();
            return this;
        }
    }
}