using NUnit.Framework;
using WhiteSharp.Tests.ScreenObjects;

namespace WhiteSharp.Tests.UITests
{
    [TestFixture]
    public class ActionTests : TestBaseNUnit
    {
        [TestCase("AComboBox", "Test5")]
        [TestCase("EditableComboBox", "Test3")]
        [TestCase("DataBoundComboBox", "Test5")]
        public void SelectItemTest(string id, string item)
        {
            var control = MainWindow.Instance.Window.FindControl(id);
            control.SelectItem(item);
            AssertThat.AreEqual(control, item, control.GetText());
        }

        [TestCase("AComboBox", 2, "Test3")]
        [TestCase("EditableComboBox", 3, "Test4")]
        [TestCase("DataBoundComboBox", 2, "Test3")]
        public void SelectItemTest(string id, int item, string result)
        {
            var control = MainWindow.Instance.Window.FindControl(id);
            control.SelectItem(item);
            AssertThat.AreEqual(control, result, control.GetText());
        }
    }
}