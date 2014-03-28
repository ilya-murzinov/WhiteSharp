using System.Windows.Automation;

namespace WhiteSharp.Tests.ScreenObjects
{
    internal class ListViewWindow : ScreenObject
    {
        private static ListViewWindow _instance;
        private readonly Control _hScrollList;
        private readonly Control _vScrollList;
        private readonly Window _window;

        private ListViewWindow()
        {
            _window = new Window("ListViewWindow");
            _vScrollList = _window.FindControl("ListView");
            _hScrollList = _window.FindControl("ListViewWithHorizontalScroll");
        }

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