using System.Windows.Automation;
using NUnit.Framework;
using Shouldly;
using WhiteSharp.Controls;
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
            ComboBox control = MainWindow.Instance.Window.FindControl<ComboBox>(id);
            control.SelectItem(item);
            control.GetText().ShouldBe(item);
        }

        [TestCase("AComboBox", 2, "Test3")]
        [TestCase("EditableComboBox", 3, "Test4")]
        [TestCase("DataBoundComboBox", 2, "Test3")]
        public void SelectItemTest(string id, int item, string result)
        {
            ComboBox control = MainWindow.Instance.Window.FindControl<ComboBox>(id);
            control.SelectItem(item);
            control.GetText().ShouldBe(result);
        }

        [TestCase]
        public void ClickChangeItemsButtonTest()
        {
            Window window = MainWindow.Instance.Window;
            IControl listItems = window.FindControl("ListBoxWpf");
            listItems.FindControl(By.Name("Spielberg"));
            Button button = window.FindControl<Button>("ChangeListItems");
            button.Click();
            listItems.FindControl(By.Name("Jackson"));
        }

        [Test]
        public void MessageBoxTest()
        {
            MainWindow.Instance
                .OpenMessageBox()
                .CloseMessageBox();
        }

        [Test]
        public void ScrollTest()
        {
            MainWindow.Instance
                .OpenListViewWindow()
                .ScrollVScrollList(ScrollAmount.LargeIncrement)
                .ScrollHScrollList(ScrollAmount.LargeIncrement)
                .ScrollVScrollList(ScrollAmount.SmallIncrement)
                .ScrollHScrollList(ScrollAmount.SmallIncrement)
                .ScrollVScrollList(ScrollAmount.LargeDecrement)
                .ScrollHScrollList(ScrollAmount.LargeDecrement)
                .ScrollVScrollList(ScrollAmount.SmallDecrement)
                .ScrollHScrollList(ScrollAmount.SmallDecrement)
                .Close();
        }
    }
}