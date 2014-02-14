namespace WhiteSharp.Tests.ScreenObjects
{
    internal class MainWindowListControlsTab : MainWindow
    {
        private static MainWindowListControlsTab _instance;

        protected MainWindowListControlsTab()
        {
            Window = new Window(Title);
        }

        public new static MainWindowListControlsTab Instance
        {
            get { return (_instance = new MainWindowListControlsTab()); }
        }

        public MainWindowDataGridTab OpenDataGrid()
        {
            Window.FindControl(By.ClassName("TabItem").AndName("Data Grid")).Click();
            return MainWindowDataGridTab.Instance;
        }
    }
}