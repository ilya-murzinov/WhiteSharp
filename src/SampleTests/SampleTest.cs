using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteSharp;

namespace SampleTests
{
    [TestClass]
    public class SampleTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Settings.Default.Language = "Ru";
            var w = new UIWindow("MainWindow");
            var c = w.FindControl(By.AutomationId("AComboBox")).DrawHighlight().Click();
            w.FindControl(By.AutomationId("ChangeListItems")).DrawHighlight().Click().Click();
            w.FindControl(By.Name("Input Controls").ClassName("TabItem")).DrawHighlight().Click();
            w.FindControl(By.AutomationId("MultiLineTextBox")).DrawHighlight().Click().Send("kjzhvijaer");
            w.FindControl(By.AutomationId("TextBox")).Click().DrawHighlight().Send("kjzhvijaer");
        }
    }
}
