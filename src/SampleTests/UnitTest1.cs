using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteSharp;

namespace SampleTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Settings.Default.Language = "Ru";
            Settings.Default.Timeout = 100;
            new UIWindow("MainWindow").FindControl(By.AutomationId("AComboBox").ClassName("ComboBox").Enabled(true).Name("1"));
        }
    }
}
