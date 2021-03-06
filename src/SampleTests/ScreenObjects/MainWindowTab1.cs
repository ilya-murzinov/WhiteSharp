﻿using System.Windows.Automation;
using WhiteSharp;
using WhiteSharp.Controls;

namespace SampleTests.ScreenObjects
{
    public class MainWindowTab1
    {
        private static MainWindowTab1 _instance;

        private Window _window;
        private ComboBox _combobox;
        private CheckBox _item1Checkbox;
        private CheckBox _item2Checkbox;

        private MainWindowTab1()
        {
            _window = new Window("MainWindow");
            _combobox = _window.FindControl<ComboBox>("AComboBox");
            _item1Checkbox = _window.FindControl<CheckBox>(By.ClassName("CheckBox").AndName("Item1"));
            _item2Checkbox = _window.FindControl<CheckBox>(By.ClassName("CheckBox").AndName("Item2"));
        }
        
        public static MainWindowTab1 Instance
        {
            get { return _instance ?? (_instance = new MainWindowTab1()); }
        }

        public MainWindowTab1 SelectItemFromCombobox(string item)
        {
            _combobox.SelectItem(item);
            return this;
        }

        public MainWindowTab1 SetItem1CheckboxState(ToggleState state)
        {
            _item1Checkbox.SetToggleState(state);
            return this;
        }

        public MainWindowTab1 SetItem2CheckboxState(ToggleState state)
        {
            _item2Checkbox.SetToggleState(state);
            return this;
        }

        public MainWindowTab2 OpenSecondTab()
        {
            _window.FindControl(By.Name("Input Controls").AndClassName("TabItem"))
                .Click();
            return MainWindowTab2.Instance;
        }
    }
}
