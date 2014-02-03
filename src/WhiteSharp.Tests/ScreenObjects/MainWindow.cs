namespace WhiteSharp.Tests.ScreenObjects
{
    class MainWindow
    {
        protected string Title = "MainWindow";
        public Window Window;

        protected MainWindow()
        {
            Window = new Window(Title);
        }

        private static MainWindow _instance;

        public static MainWindow Instance
        {
            get { return _instance ?? (_instance = new MainWindow()); }
        }
    }
}