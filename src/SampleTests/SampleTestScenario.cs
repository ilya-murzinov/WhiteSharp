using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using SampleTests.ScreenObjects;
using System.Diagnostics;
using System.Windows.Automation;

namespace SampleTests
{
    [TestClass]
    [TestFixture]
    public class SampleTestScenario
    {
        public static string Path = @"..\..\..\..\\TestApps\\WpfTestApplication.exe";
        private Process _proc;

        [TestInitialize]
        [TestFixtureSetUp]
        public void Init()
        {
            _proc = Process.Start(Path);
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

        [TestCleanup]
        [TestFixtureTearDown]
        public void Shutdown()
        {
            _proc.Kill();
        }
    }
}
