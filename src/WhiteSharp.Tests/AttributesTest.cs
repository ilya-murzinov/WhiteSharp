using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteSharp.Attributes;
using WhiteSharp.Factories;

namespace WhiteSharp.Tests
{
    [TestClass]
    public class AttributesTest
    {
        private Window _window = new Window("Катарсис");

        [FindsBy(Using = "navigationCommandButton_2130", How = How.AutomationId)]
        [FindsBy(How = How.ClassName, Using = "Button")]
        private Control _control;

        [TestMethod]
        public void TestMethod1()
        {
            ScreenFactory.InitControls(this);
            _control.Click();
        }
    }
}