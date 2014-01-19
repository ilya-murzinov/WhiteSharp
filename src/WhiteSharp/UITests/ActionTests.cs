using NUnit.Framework;
using WhiteSharp;

namespace WhiteSharp.UITests
{
    [TestFixture]
    public class ActionTests : TestBaseNUnit
    {
        [TestCase("AComboBox", "Test")]
        [TestCase("EditableComboBox", "Test3")]
        [TestCase("DataBoundComboBox", "Test5")]
        public void SelectItemTest(string id, string item)
        {
            var control = Window.FindControl(id);
            control.SelectItem("");
            AssertThat.AreEqual(control, item, control.GetText());
        }

        [TestCase("AComboBox", 2, "Test3")]
        [TestCase("EditableComboBox", 3, "Test4")]
        [TestCase("DataBoundComboBox", 3, "Test4")]
        public void SelectItemTest(string id, int item, string result)
        {
            var control = Window.FindControl(id);
            control.SelectItem(item);
            AssertThat.AreEqual(control, result, control.GetText());
        }
    }
}