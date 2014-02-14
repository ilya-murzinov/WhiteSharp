using System.Windows.Automation;

namespace WhiteSharp.Tests.ScreenObjects
{
    class ListViewWindow
    {
        private Window _window;

        private readonly Control _vScrollList;
        private readonly Control _hScrollList;

        private ListViewWindow()
        {
            _window = new Window("ListViewWindow");
            _vScrollList = _window.FindControl("ListView");
            _hScrollList = _window.FindControl("ListViewWithHorizontalScroll");
        }

        private static ListViewWindow _instance;

        public static ListViewWindow Instance
        {
            get { return (_instance = new ListViewWindow()); }
        }

        public ListViewWindow ScrollVScrollList(ScrollAmount scrollAmount)
        {
            _vScrollList.ScrollVertical(scrollAmount);
            return this;
        }

        public ListViewWindow ScrollHScrollList(ScrollAmount scrollAmount)
        {
            _hScrollList.ScrollHorizontal(scrollAmount);
            return this;
        }

        public MainWindow Close()
        {
            _window.Close();
            return MainWindow.Instance;
        }
    }
}
