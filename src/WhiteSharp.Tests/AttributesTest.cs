using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteSharp.Attributes;
using WhiteSharp.Factories;

namespace WhiteSharp.Tests
{
    [TestClass]
    public class AttributesTest : ScreenObject
    {
        [FindsBy(Using = "navigationCommandButton_2130", How = How.AutomationId)] [FindsBy(How = How.ControlType, Using = "ControlType.Button")] [FindsBy(How = How.ControlType, Using = "Button")] private Control _control;
        private Window _window = new Window("Катарсис");

        private AttributesTest()
        {
        }

        [TestMethod]
        public void TestMethod1()
        {
            ScreenFactory.InitControls(this);
            _control.Click();
        }
    }
}