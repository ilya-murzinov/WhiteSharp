using WhiteSharp.Attributes;
using WhiteSharp.Factories;

namespace WhiteSharp.Tests.ScreenObjects
{
    public class MainWindowListControlsTab : MainWindow
    {
        private static MainWindowListControlsTab _instance;

        [FindBy(How = How.ClassName, Using = "TabItem")]
        [FindBy(How = How.Name, Using = "Data Grid")]
        private Control _dataGridTab;

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
            _dataGridTab.Click();
            return MainWindowDataGridTab.Instance;
        }
    }
}