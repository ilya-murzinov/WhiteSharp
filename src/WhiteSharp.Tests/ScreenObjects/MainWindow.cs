namespace WhiteSharp.Tests.ScreenObjects
{
    class MainWindow
    {
        protected string Title = "MainWindow";
        public UIWindow Window;

        protected MainWindow()
        {
            Window = new UIWindow(Title);
        }

        private static MainWindow _instance;

        public static MainWindow Instance
        {
            get { return _instance ?? (_instance = new MainWindow()); }
        }
    }
}