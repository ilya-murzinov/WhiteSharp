using System.Windows.Automation;
using WhiteSharp.Attributes;
using WhiteSharp.Factories;

namespace WhiteSharp.Tests.ScreenObjects
{
    public class ListViewWindow : ScreenObject
    {
        private static ListViewWindow _instance;

        [Window("ListViewWindow")]
        private readonly Window _window;

        [FindsBy("ListViewWithHorizontalScroll")]
        private readonly Control _hScrollList;

        [FindsBy("ListView")]
        private readonly Control _vScrollList;

        private ListViewWindow()
        {
            ScreenFactory.InitControls(this);
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