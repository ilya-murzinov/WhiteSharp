using System.Diagnostics;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;

namespace WhiteExtension.UnitTests
{
    /// <summary>
    /// Summary description for Tests
    /// </summary>
    [TestFixture]
    internal class UnitTests
    {
        [TestFixtureSetUp]
        public void Init()
        {
            Process.Start(@"C:\Windows\System32\calc.exe");
            Thread.Sleep(3000);
            Window = Desktop.Instance.Windows().Find(x => x.Title.Contains("Calculator"));
            Config.Timeout = 100;
            Window.DisplayState = DisplayState.Restored;
            Window.Focus();
        }

        [TestFixtureTearDown]
        public void Cleanup()
        {
            Process.GetProcessesByName("calc").ToList().ForEach(x => x.Kill());
        }

        public Window Window;

        [TestCase("adlkfsh"), ExpectedException(typeof(WindowNotFoundException))]
        [TestCase("КАЛЬКУЛЯТОР")]
        [TestCase("ккалькулятор")]
        [TestCase("кАльКУл")]
        [TestCase("^%$iofkjndsf9)&*")]
        public void GetWindowNegativeTest(string title)
        {
            Logging.Start(title);
            new UIWindow(title);
        }

        [TestCase("")]
        [TestCase("лятор")]
        [TestCase("Кальк")]
        [TestCase("алькул")]
        [TestCase("К")]
        public void GetWindowPositiveTest(string title)
        {
            Logging.Start(title);
            new UIWindow(title);
        }
        
        [TestCase("", 44)]
        [TestCase("1", 27)]
        public void CountTest(string id, int count)
        {
            Logging.Start(id, count.ToString());
            By.Window = Window;
            var finder = By.AutomationIdContains(id);
            Assert.IsTrue(finder.Result.Count == count);
        }

        [TestCase("112333"), ExpectedException(typeof(ControlNotFoundException))]
        [TestCase("gfjs")]
        [TestCase("Fjf")]
        public void AutomationIdNegativeTest(string id)
        {
            Logging.Start(id);
            By.Window = Window;
            var finder = By.AutomationIdContains(id);
            Assert.IsTrue(finder.Result.TrueForAll(x=>x.Current.AutomationId.Contains(id)));
        }

        [TestCase("Button")]
        [TestCase("CalcFrame")]
        public void ClassNamePositiveTest(string id)
        {
            Logging.Start(id);
            By.Window = Window;
            var finder = By.ClassName(id);
            Assert.IsTrue(finder.Result.TrueForAll(x => x.Current.ClassName.Equals(id)));
        }

        [TestCase("1")]
        [TestCase("2")]
        [TestCase("4")]
        [TestCase("6")]
        public void AutomationIdContainsPositiveTest(string id)
        {
            Logging.Start(id);
            By.Window = Window;
            var finder = By.AutomationIdContains(id);
            Assert.IsTrue(finder.Result.TrueForAll(x => x.Current.AutomationId.Contains(id)));
        }

        [TestCase("Правка")]
        [TestCase("Вид")]
        [TestCase("Справка")]
        [TestCase("Очистка памяти")]
        public void ByName(string id)
        {
            Logging.Start(id);
            By.Window = Window;
            var finder = By.Name(id);
            Assert.IsTrue(finder.Result.Count == 1);
        }

        [TestCase("82", "122")]
        [TestCase("80", "123")]
        [TestCase("122", "80")]
        [TestCase("123", "81")]
        [TestCase("121", "80")]
        public void Find(string id1, string id2)
        {
            Logging.Start(id1, id2);
            By.Window = Window;
            var c = new UIControl(By.AutomationId(id1).Result.First(), Desktop.Instance);
            Assert.True(By.AutomationId(id2).Result.First().Current.AutomationId.Equals(id2));
        }

        [TestCase("CalcFrame", "80")]
        [TestCase("CalcFrame", "121")]
        [TestCase("CalcFrame", "122")]
        [TestCase("CalcFrame", "123")]
        public void GetChild(string id1, string id2)
        {
            Logging.Start(id1, id2);
            By.Window = Window;
            var c = new UIControl(By.ClassName(id1).Result.First(), Desktop.Instance);
            c.FindChild(By.AutomationId(id2));
        }
    }
}