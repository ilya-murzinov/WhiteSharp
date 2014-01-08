using System.Diagnostics;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;

namespace WhiteSharp.UnitTests
{
    [TestFixture]
    public class UnitTestsNUnit : TestBaseNUnit
    {
        [TestCase("adlkfsh"), ExpectedException(typeof(WindowNotFoundException))]
        [TestCase("КАЛЬКУЛЯТОР")]
        [TestCase("ккалькулятор")]
        [TestCase("кАльКУл")]
        [TestCase("^%$iofkjndsf9)&*")]
        public void GetWindowNegativeTest(string title)
        {
            new UIWindow(title);
        }

        [TestCase("")]
        [TestCase("лятор")]
        [TestCase("Кальк")]
        [TestCase("алькул")]
        [TestCase("К")]
        public void GetWindowPositiveTest(string title)
        {
            new UIWindow(title);
        }

        [TestCase("", 44)]
        [TestCase("1", 27)]
        public void CountTest(string id, int count)
        {
            By.Window = Window;
            var finder = By.AutomationIdContains(id);
            Assert.IsTrue(finder.Result.Count == count);
        }

        [TestCase("112333"), ExpectedException(typeof(ControlNotFoundException))]
        [TestCase("gfjs")]
        [TestCase("Fjf")]
        public void AutomationIdNegativeTest(string id)
        {
            By.Window = Window;
            var finder = By.AutomationIdContains(id);
            Assert.IsTrue(finder.Result.TrueForAll(x => x.Current.AutomationId.Contains(id)));
        }

        [TestCase("Button")]
        [TestCase("CalcFrame")]
        public void ClassNamePositiveTest(string id)
        {
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
            By.Window = Window;
            var c = new UIControl(By.ClassName(id1).Result.First(), Desktop.Instance);
            c.FindChild(By.AutomationId(id2));
        }
    }
}
