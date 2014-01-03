using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;
using System.Linq;

namespace WhiteSharp.UnitTests
{
    [TestClass]
    public class TestBaseMSTest
    {
        public TestContext TestContext
        {
            get;
            set;
        }

        [AssemblyInitialize]
        public static void Init(TestContext t)
        {
            Process.Start(@"C:\Windows\System32\calc.exe");
            Thread.Sleep(3000);
            Window = Desktop.Instance.Windows().Find(x => x.Title.Contains("Calculator"));
            Config.Timeout = 100;
            Window.DisplayState = DisplayState.Restored;
            Window.Focus();
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            Process.GetProcessesByName("calc").ToList().ForEach(x => x.Kill());
        }

        [TestCleanup]
        public void Screen()
        {
        }

        public static Window Window;
    }
}
