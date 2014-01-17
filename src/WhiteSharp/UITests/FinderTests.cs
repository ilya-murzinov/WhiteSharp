using System.Windows.Automation;
using NUnit.Framework;

namespace WhiteSharp.UITests
{
    [TestFixture]
    public class FinderTests : TestBaseNUnit
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

        [TestCase("AComboBox")]
        [TestCase("EditableComboBox")]
        [TestCase("OpenHorizontalSplitterButton")]
        [TestCase("ListBoxWithVScrollBar")]
        [TestCase("CheckedListBox")]
        public void FindControlByAutomaionIdString(string id)
        {
            Window.FindControl(id);
        }

        [Test]
        public void FindControlByControlTypeType()
        {
            Window.FindControl(ControlType.Edit);
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
            Window.FindControl(By.ClassName(id), index);
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

        [TestCase("ControlsTab", "ComboBox")]
        [TestCase("ControlsTab", "TabItem")]
        [TestCase("ListBoxWpf", "ListBoxItem")]
        [TestCase("ScenariosPane", "Button")]
        [TestCase("CheckedListBox", "CheckBox")]
        public void FindChild(string parentId, string childId)
        {
            Window.FindControl(By.AutomationId(parentId)).FindChild(By.ClassName(childId));
        }

        [TestCase("AComboBox", "", "ComboBox")]
        [TestCase("OpenVerticalSplitterButton", "Launch Vertical GridSplitter Window", "Button")]
        [TestCase("", "Item1", "ListBoxItem")]
        [TestCase("ListControlsTab", "List Controls", "TabItem")]
        [TestCase("ToolStrip1", "", "ToolBar")]
        public void FindControlByMultipleConditions(string id1, string id2, string id3)
        {
            Window.FindControl(By.AutomationId(id1).AndName(id2).AndClassName(id3));
        }

        [TestCase(1, 0, "Item1")]
        [TestCase(1, 1, "Item2")]
        [TestCase(2, 0, "Simple item 1")]
        public void FindGeriCell(int i, int j, string result)
        {
            Window.FindControl(By.ClassName("TabItem").AndName("Data Grid")).Click();
            var c = Window.FindControl(By.GridCell(i, j));
            AssertThat.AreEqual(c, c.GetText(), result);
        }

        [TestCase("ControlsTab", "ListControlsTab", "ListControls", "ListBoxWpf", "ListBoxItem")]
        public void FindChildMultiple(string id1, string id2, string id3, string id4, string id5)
        {
            Window.FindControl(By.AutomationId(id1)).FindChild(By.AutomationId(id2)).FindChild(By.AutomationId(id3))
                .FindChild(By.AutomationId(id4)).FindChild(By.ClassName(id5));
        }
    }
}