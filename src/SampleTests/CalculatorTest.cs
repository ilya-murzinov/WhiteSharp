using System;
using NUnit.Framework;
using WhiteSharp;
using Shouldly;
using WhiteSharp.Controls;

namespace SampleTests
{
    public class CalculatorScreenObject
    {
        private Window window;
        private Button help;
        private Button about;

        private TextBox display;
        private Button clearEverything;

        private Button[] numbers = new Button[10];

        private Button equals;
        private Button add;
        private Button substract;
        private Button multiply;
        private Button divide;

        protected CalculatorScreenObject()
        {
            window = new Window("Calculator");
            help = window.FindControl<Button>("Item 3");
            display = window.FindControl<TextBox>("150");
            for (int i = 0; i < 10; i++)
            {
                numbers[i] = window.FindControl<Button>((130 + i).ToString());
            }
            clearEverything = window.FindControl<Button>("82");
            equals = window.FindControl<Button>("121");
            add = window.FindControl<Button>("93");
            substract = window.FindControl<Button>("94");
            multiply = window.FindControl<Button>("92");
            divide = window.FindControl<Button>("91");            
        }

        private static CalculatorScreenObject instance;
        public static CalculatorScreenObject Instance
        {
            get { return instance ?? (instance = new CalculatorScreenObject()); }
        }

        public CalculatorScreenObject CheckDisplayTextEquals(string text)
        {
            Math.Round(double.Parse(display.GetText()), 15).ToString().ShouldBe(text);
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
            about = help.FindControl<Button>("Item 302");
            about.Click();
            return AboutDialog.Instance;
        }
    }

    public class AboutDialog
    {
        private Window window;

        private Control btnOk;

        private static AboutDialog instance;
        public static AboutDialog Instance
        {
            get { return instance ?? (instance = new AboutDialog()); }
        }

        private AboutDialog()
        {
            window = new Window("Calculator");
            btnOk = window.FindControl<Button>("1");
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
            double i = r.Next(1000);
            double j = r.Next(1000);
            CalculatorScreenObject.Instance
                .Press((int) i)
                .PressDivide()
                .Press((int) j)
                .PressEquals()
                .CheckDisplayTextEquals(Math.Round(i / j, 15).ToString())
                .ClearScreen();
        }
        [TestCase]
        public void OpenAboutCalculator()
        {
            CalculatorScreenObject.Instance.OpenAboutCalculator().Close();
        }
    }
}