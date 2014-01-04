using System;
using NUnit.Framework;
using WhiteSharp;
using WhiteSharp.UnitTests;

namespace SampleTests
{
    [TestFixture]
    public class CalculatorTest : TestBaseNUnit
    {
        [TestCase("130", "0")]
        [TestCase("131", "1")]
        [TestCase("132", "2")]
        [TestCase("133", "3")]
        [TestCase("134", "4")]
        [TestCase("135", "5")]
        [TestCase("136", "6")]
        [TestCase("137", "7")]
        [TestCase("138", "8")]
        [TestCase("139", "9")]
        public void NumberButtonsTest(string buttonId, string result)
        {
            UIControl button = MainWindow.FindControl(By.AutomationId(buttonId)).Click();
            AssertThat.AreEqual(Display, Display.Name, result);
        }
        
        [TestCase("1")]
        [TestCase("2")]
        [TestCase("3")]
        [TestCase("4")]
        [TestCase("5")]
        [TestCase("6")]
        [TestCase("7")]
        [TestCase("8")]
        [TestCase("9")]
        [TestCase("0")]
        public void Add(string str)
        {
            UIControl plus = MainWindow.FindControl(By.AutomationId("93"));
            UIControl equals = MainWindow.FindControl(By.AutomationId("121"));
            Random r = new Random();
            int i = (int)r.Next(1000);
            int j = (int)r.Next(1000);
            Display.Click().Send(i.ToString());
            plus.Click();
            Display.Click().Send(j.ToString());
            equals.Click();
            AssertThat.AreEqual(Display, Display.Name, (i + j).ToString());
        }

        [TestCase("1")]
        [TestCase("2")]
        [TestCase("3")]
        [TestCase("4")]
        [TestCase("5")]
        [TestCase("6")]
        [TestCase("7")]
        [TestCase("8")]
        [TestCase("9")]
        [TestCase("0")]
        public void Multiply(string str)
        {
            UIControl mult = MainWindow.FindControl(By.AutomationId("92"));
            UIControl equals = MainWindow.FindControl(By.AutomationId("121"));
            Random r = new Random();
            int i = (int)r.Next(1000);
            int j = (int)r.Next(1000);
            Display.Click().Send(i.ToString());
            mult.Click();
            Display.Click().Send(j.ToString());
            equals.Click();
            AssertThat.AreEqual(Display, Display.Name, (i * j).ToString());
        }
    }
}
