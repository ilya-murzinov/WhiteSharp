using NUnit.Framework;

namespace WhiteSharp.UnitTests
{
    [TestFixture]
    public class UnitTestsNUnit : TestBaseNUnit
    {
        [TestCase("лоукггкгаг"), ExpectedException(typeof (WindowNotFoundException))]
        [TestCase("MAIN")]
        [TestCase("wIndow")]
        [TestCase("MainWindows")]
        [TestCase("^%$iofkjndsf9)&*")]
        public void GetWindowNegativeTest(string title)
        {
            new UIWindow(title);
        }

        [TestCase("")]
        [TestCase("Main")]
        [TestCase("Window")]
        [TestCase("MainW")]
        [TestCase("M")]
        public void GetWindowPositiveTest(string title)
        {
            new UIWindow(title);
        }

        [TestCase("AComboBox")]
        [TestCase("EditableComboBox")]
        [TestCase("OpenHorizontalSplitterButton")]
        [TestCase("ListBoxWithVScrollBar")]
        [TestCase("CheckedListBox")]
        public void FindControlByAutomaionId(string id)
        {
            Window.FindControl(By.AutomationId(id));
        }

        [TestCase("Input Controls")]
        [TestCase("Get Multiple")]
        [TestCase("Button in toolbar")]
        [TestCase("Open Window With Scrollbars")]
        [TestCase("File")]
        public void FindControlByName(string id)
        {
            Window.FindControl(By.Name(id));
        }

        [TestCase("TextBlock")]
        [TestCase("ComboBox")]
        [TestCase("TabItem")]
        [TestCase("TabItem")]
        [TestCase("Button")]
        public void FindControlClassName(string id)
        {
            Window.FindControl(By.ClassName(id));
        }

        [TestCase("TextBlock", 1)]
        [TestCase("ComboBox", 2)]
        [TestCase("TabItem", 1)]
        [TestCase("TabItem", 1)]
        [TestCase("Button", 1)]
        public void FindControlByClassNameAndIndex(string id, int index)
        {
            Window.FindControl(By.ClassName(id).Index(index));
        }

        [TestCase("TextBlock", 1), ExpectedException(typeof (ControlNotFoundException))]
        [TestCase("ComboBox", 2)]
        [TestCase("TabItem", 1)]
        [TestCase("TabItem", 1)]
        [TestCase("Button", 1)]
        public void FindControlByAutomationIdNegative(string id, int index)
        {
            Window.FindControl(By.AutomationId(id));
        }

        [TestCase("AComboBox"), ExpectedException(typeof (ControlNotFoundException))]
        [TestCase("EditableComboBox")]
        [TestCase("OpenHorizontalSplitterButton")]
        [TestCase("ListBoxWithVScrollBar")]
        [TestCase("CheckedListBox")]
        public void FindControlByNameNegative(string id)
        {
            Window.FindControl(By.Name(id));
        }
    }
}