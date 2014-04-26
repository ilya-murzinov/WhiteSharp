using WhiteSharp.Attributes;
using WhiteSharp.Factories;

namespace WhiteSharp.Tests.ScreenObjects
{
    public class MainWindow : ScreenObject
    {
        private static MainWindow _instance;
        
        protected const string Title = "MainWindow";
        
        [Window(Title)]
        private Window _window;

        [FindBy(Using = "OpenListView")]
        private readonly Control _listViewButton;

        protected MainWindow()
        {
            ScreenFactory.InitControls(this);
        }

        public Window Window
        {
            get { return _window; }
            set { _window = value; }
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
            _window.FindControl("OpenMessageBox").Click();
            return this;
        }

        public MainWindow CloseMessageBox()
        {
            _window.FindModalWindow("Test message box").FindControl("2").Click();
            return this;
        }
    }
}