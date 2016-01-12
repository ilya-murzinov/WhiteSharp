using WhiteSharp.Factories;

namespace WhiteSharp.Tests.ScreenObjects
{
    public class MainWindowListControlsTab : MainWindow
    {
        private static MainWindowListControlsTab _instance;

        protected MainWindowListControlsTab()
        {
            Window = new Window(Title);
            ScreenFactory.InitControls(this);
        }

        public new static MainWindowListControlsTab Instance
        {
            get { return (_instance = new MainWindowListControlsTab()); }
        }

        public MainWindowDataGridTab OpenDataGrid()
        {
            var dataGridTab = Window.FindControl(By.Name("Data Grid").AndClassName("TabItem"));
            dataGridTab.Click();
            return MainWindowDataGridTab.Instance;
        }
    }
}