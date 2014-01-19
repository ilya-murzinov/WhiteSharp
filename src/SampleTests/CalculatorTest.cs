using System;
using NUnit.Framework;
using WhiteSharp;

namespace SampleTests
{
    public class CalculatorScreenObject
    {
        private UIWindow window;
        private UIControl help;
        private UIControl about;

        private UIControl display;
        private UIControl clearEverything;

        private UIControl[] numbers = new UIControl[10];

        private UIControl equals;
        private UIControl add;
        private UIControl substract;
        private UIControl multiply;
        private UIControl divide;

        protected CalculatorScreenObject()
        {
            window = new UIWindow("Calculator");
            help = window.FindControl("Item 3");
            display = window.FindControl("150");
            for (int i = 0; i < 10; i++)
            {
                numbers[i] = window.FindControl((130 + i).ToString());
            }
            clearEverything = window.FindControl("82");
            equals = window.FindControl("121");
            add = window.FindControl("93");
            substract = window.FindControl("94");
            multiply = window.FindControl("92");
            divide = window.FindControl("91");            
        }

        private static CalculatorScreenObject instance;
        public static CalculatorScreenObject Instance
        {
            get { return instance = new CalculatorScreenObject(); }
        }

        public CalculatorScreenObject CheckDisplayTextEquals(string text)
        {
            AssertThat.AreEqual(display, text, display.GetText());
            return this;
        }
        public CalculatorScreenObject ClearScreen()
        {
            clearEverything.Click();
            return this;
        }
        public CalculatorScreenObject Press(int number)
        {
            foreach (char ch in number.ToString())
            {
                numbers[int.Parse(ch.ToString())].Click();
            }
            return this;
        }
        public CalculatorScreenObject PressEquals()
        {
            equals.Click();
            return this;
        }
        public CalculatorScreenObject PressAdd()
        {
            add.Click();
            return this;
        }
        public CalculatorScreenObject PressSubstract()
        {
            substract.Click();
            return this;
        }
        public CalculatorScreenObject PressMultiply()
        {
            multiply.Click();
            return this;
        }
        public CalculatorScreenObject PressDivide()
        {
            divide.Click();
            return this;
        }
        public AboutDialog OpenAboutCalculator()
        {
            help.Click();
            about = help.FindChild("Item 302");
            about.Click();
            return AboutDialog.Instance;
        }
    }

    public class AboutDialog
    {
        private UIWindow window;

        private UIControl btnOk;

        private static AboutDialog instance;
        public static AboutDialog Instance
        {
            get { return instance ?? (instance = new AboutDialog()); }
        }

        private AboutDialog()
        {
            window = new UIWindow("Calculator").ModalWindow("About Calculator");
            btnOk = window.FindControl("1");
        }

        public CalculatorScreenObject Close()
        {
            btnOk.Click();
            return CalculatorScreenObject.Instance;
        }
    }

    [TestFixture]
    public class CalculatorTest : TestBaseNUnit
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        public void NumberButtonsTest(int number)
        {
            CalculatorScreenObject.Instance
                .Press(number)
                .CheckDisplayTextEquals(number.ToString())
                .ClearScreen();            
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
            var r = new Random();
            int i = r.Next(1000);
            int j = r.Next(1000);
            CalculatorScreenObject.Instance
                .Press(i)
                .PressAdd()
                .Press(j)
                .PressEquals()
                .CheckDisplayTextEquals((i+j).ToString())
                .ClearScreen();
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
        public void Substract(string str)
        {
            var r = new Random();
            int i = r.Next(1000);
            int j = r.Next(1000);
            CalculatorScreenObject.Instance
                .Press(i)
                .PressSubstract()
                .Press(j)
                .PressEquals()
                .CheckDisplayTextEquals((i - j).ToString())
                .ClearScreen();
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
            var r = new Random();
            int i = r.Next(1000);
            int j = r.Next(1000);
            CalculatorScreenObject.Instance
                .Press(i)
                .PressMultiply()
                .Press(j)
                .PressEquals()
                .CheckDisplayTextEquals((i * j).ToString())
                .ClearScreen();
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
        public void Divide(string str)
        {
            var r = new Random();
            int i = r.Next(1000);
            int j = r.Next(1000);
            CalculatorScreenObject.Instance
                .Press(i)
                .PressDivide()
                .Press(j)
                .PressEquals()
                .CheckDisplayTextEquals((i / j).ToString())
                .ClearScreen();
        }
        [TestCase]
        public void OpenAboutCalculator()
        {
            CalculatorScreenObject.Instance.OpenAboutCalculator().Close();
        }
    }
}