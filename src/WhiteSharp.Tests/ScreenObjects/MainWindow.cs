﻿using WhiteSharp.Attributes;
using WhiteSharp.Factories;

namespace WhiteSharp.Tests.ScreenObjects
{
    public class MainWindow : ScreenObject
    {
        protected const string Title = "MainWindow";
        private static MainWindow _instance;

        [FindBy(How = How.AutomationId, Using = "OpenListView")] private readonly Control _listViewButton;
        [Window(Title)] private Window _window;

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