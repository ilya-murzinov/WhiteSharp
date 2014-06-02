using System.Windows.Automation;
using WhiteSharp;
using WhiteSharp.Controls;

namespace SampleTests.ScreenObjects
{
    public class MainWindowTab2
    {
        private static MainWindowTab2 _instance;

        private Window _window;
        private TextBox _textbox;
        private RadioButton _radiobutton;

        private MainWindowTab2()
        {
            _window = new Window("MainWindow");
            _textbox = _window.FindControl<TextBox>("MultiLineTextBox");
            _radiobutton = _window.FindControl<RadioButton>("RadioButton1");

        }
        
        public static MainWindowTab2 Instance
        {
            get { return _instance ?? (_instance = new MainWindowTab2()); }
        }

        public MainWindowTab2 SetTextToMultilineTextbox(string text)
        {
            _textbox.Send(text);
            return this;
        }

        public MainWindowTab2 SelectRadiobuttonState()
        {
            _radiobutton.Select();
            return this;
        }
    }
}
