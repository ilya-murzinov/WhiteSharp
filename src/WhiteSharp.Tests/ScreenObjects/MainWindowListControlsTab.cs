namespace WhiteSharp.Tests.ScreenObjects
{
    class MainWindowListControlsTab : MainWindow
    {
        public Window Window;

        protected MainWindowListControlsTab()
        {
            Window = new Window(Title);
        }

        private static MainWindowListControlsTab _instance;

        public static new MainWindowListControlsTab Instance
        {
            get { return _instance ?? (_instance = new MainWindowListControlsTab()); }
        }

        public MainWindowDataGridTab OpenDataGrid()
        {
            Window.FindControl(By.ClassName("TabItem").AndName("Data Grid")).Click();
            return MainWindowDataGridTab.Instance;
        }
    }
}
