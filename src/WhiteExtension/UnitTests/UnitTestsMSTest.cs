using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;

namespace WhiteExtension.UnitTests
{
    /// <summary>
    /// Summary description for Tests
    /// </summary>
    [TestClass]
    public class UnitTestsMSTest : TestBaseMSTest
    {
        [TestMethod]
        public void Test()
        {
            GetWindowPositiveTest("Calc");
            Find("121", "122");
        }
        
        public void GetWindowNegativeTest(string title)
        {
            Logging.Start(title);
            new UIWindow(title);
        }

        public void GetWindowPositiveTest(string title)
        {
            Logging.Start(title);
            new UIWindow(title);
        }
        
        public void CountTest(string id, int count)
        {
            By.Window = Window;
            var finder = By.AutomationIdContains(id);
            Assert.IsTrue(finder.Result.Count == count);
        }

        public void AutomationIdNegativeTest(string id)
        {
            By.Window = Window;
            var finder = By.AutomationIdContains(id);
            Assert.IsTrue(finder.Result.TrueForAll(x=>x.Current.AutomationId.Contains(id)));
        }

        public void ClassNamePositiveTest(string id)
        {
            By.Window = Window;
            var finder = By.ClassName(id);
            Assert.IsTrue(finder.Result.TrueForAll(x => x.Current.ClassName.Equals(id)));
        }

        public void AutomationIdContainsPositiveTest(string id)
        {
            By.Window = Window;
            var finder = By.AutomationIdContains(id);
            Assert.IsTrue(finder.Result.TrueForAll(x => x.Current.AutomationId.Contains(id)));
        }

        public void ByName(string id)
        {
            By.Window = Window;
            var finder = By.Name(id);
            Assert.IsTrue(finder.Result.Count == 1);
        }

        public void Find(string id1, string id2)
        {
            By.Window = Window;
            var c = new UIControl(By.AutomationId(id1).Result.First(), Desktop.Instance);
            Assert.IsTrue(By.AutomationId(id2).Result.First().Current.AutomationId.Equals(id2));
        }

        public void GetChild(string id1, string id2)
        {
            By.Window = Window;
            var c = new UIControl(By.ClassName(id1).Result.First(), Desktop.Instance);
            c.FindChild(By.AutomationId(id2));
        }
    }
}