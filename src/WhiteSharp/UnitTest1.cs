using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WhiteSharp
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            new Window("MainWindow").FindControl("AComboBox").Click();
        }
    }
}
