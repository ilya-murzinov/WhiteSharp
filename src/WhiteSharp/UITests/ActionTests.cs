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
            AssertThat.AreEqual(control, control.GetText(), item);
        }

        [TestCase("AComboBox", 2, "Test2")]
        [TestCase("EditableComboBox", 4, "Test4")]
        [TestCase("DataBoundComboBox", 3, "Test3")]
        public void SelectItemTest(string id, int item, string result)
        {
            var control = Window.FindControl(id);
            control.SelectItem(item);
            AssertThat.AreEqual(control, control.GetText(), result);
        }
    }
}