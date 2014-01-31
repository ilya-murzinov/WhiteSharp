namespace WhiteSharp.Tests.ScreenObjects
{
    class MainWindowDataGridTab : MainWindow
    {
        public UIWindow Window;

        protected MainWindowDataGridTab()
        {
            Window = new UIWindow(Title);
        }

        private static MainWindowDataGridTab _instance;

        public static new MainWindowDataGridTab Instance
        {
            get { return _instance ?? (_instance = new MainWindowDataGridTab()); }
        }
    }
}