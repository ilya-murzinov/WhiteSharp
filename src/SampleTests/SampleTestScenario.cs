using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using SampleTests.ScreenObjects;
using System.Windows.Automation;

namespace SampleTests
{
    [TestClass]
    [TestFixture]
    public class SampleTestScenario
    {
        [TestFixtureSetUp]
        public void Init()
        {

        }

        [TestMethod]
        [Test]
        public void SampleTest()
        {
            MainWindowTab1.Instance
                .SelectItemFromCombobox("Test4")
                .SetItem1CheckboxState(ToggleState.On)
                .SetItem2CheckboxState(ToggleState.On)
                .SetItem1CheckboxState(ToggleState.Off)
                .OpenSecondTab()
                .SetTextToMultilineTextbox("some text")
                .SelectRadiobuttonState();
        }

        [TestFixtureTearDown]
        public void Shutdown()
        {

        }
    }
}
