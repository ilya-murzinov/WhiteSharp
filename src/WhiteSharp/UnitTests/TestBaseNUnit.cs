using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Drawing;
using NUnit.Framework;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;

namespace WhiteSharp.UnitTests
{
    [TestFixture]
    public class TestBaseNUnit
    {
        public TestContext TestContext
        {
            get;
            set;
        }

        public Window Window;
        
        [TestFixtureSetUp]
        public void Iint()
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
        [SetUp]
        public void Start()
        {
            Logging.Start(TestContext.CurrentContext.Test.Name);
        }
        [TearDown]
        public void Stop()
        {
            if (TestContext.CurrentContext.Result.Status == TestStatus.Failed)
            {
                string name = TestContext.CurrentContext.Test.Name.ToString();
                new TestStack.White.ScreenCapture().CaptureScreenShot().Save("Results\\"+name.Substring(0,name.IndexOf("("))+".bmp");
            }
            Logging.Write(TestContext.CurrentContext.Result.Status.ToString().ToUpper()+"!");
        }
    }
}
