namespace WhiteSharp.Tests.ScreenObjects
{
    class MainWindowDataGridTab : MainWindow
    {
        public Window Window;

        protected MainWindowDataGridTab()
        {
            Window = new Window(Title);
        }

        private static MainWindowDataGridTab _instance;

        public static new MainWindowDataGridTab Instance
        {
            get { return (_instance = new MainWindowDataGridTab()); }
        }
    }
}