using System.Windows.Automation;
using WhiteSharp.Factories;

namespace WhiteSharp.Tests.ScreenObjects
{
    public class ListViewWindow : ScreenObject
    {
        private static ListViewWindow _instance;

        private readonly Window _window;

        private ListViewWindow()
        {
            _window = new Window("ListViewWindow");
            ScreenFactory.InitControls(this, _window.WindowTitle);
        }

        public static ListViewWindow Instance
        {
            get { return (_instance = new ListViewWindow()); }
        }

        public ListViewWindow ScrollVScrollList(ScrollAmount scrollAmount)
        {
            var vScrollList = _window.FindControl("ListView");
            vScrollList.ScrollVertical(scrollAmount);
            return this;
        }

        public ListViewWindow ScrollHScrollList(ScrollAmount scrollAmount)
        {
            var hScrollList = _window.FindControl("ListViewWithHorizontalScroll");
            hScrollList.ScrollHorizontal(scrollAmount);
            return this;
        }

        public MainWindow Close()
        {
            _window.Close();
            return MainWindow.Instance;
        }
    }
}