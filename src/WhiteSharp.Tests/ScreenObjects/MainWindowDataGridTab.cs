namespace WhiteSharp.Tests.ScreenObjects
{
    public class MainWindowDataGridTab : MainWindow
    {
        private static MainWindowDataGridTab _instance;

        protected MainWindowDataGridTab()
        {
            Window = new Window(Title);
        }

        public new static MainWindowDataGridTab Instance
        {
            get { return (_instance = new MainWindowDataGridTab()); }
        }
    }
}