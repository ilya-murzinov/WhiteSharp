using System.Diagnostics;
using NUnit.Framework;
using TestStack.White;
using WhiteSharp;

namespace SampleTests
{
    [TestFixture(Ignore=true)]
    public class TestBaseNUnit
    {
        [SetUp]
        public void Start()
        {
            Logging.Start(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void Stop()
        {
            //Take screenshot if test failed
            if (TestContext.CurrentContext.Result.Status == TestStatus.Failed)
            {
                string name = TestContext.CurrentContext.Test.Name;
                new ScreenCapture().CaptureScreenShot()
                    .Save("Results\\" + name.Substring(0, name.IndexOf("(")) + ".bmp");
            }

            //Clear screen
            MainWindow.FindControl(By.AutomationId("81")).Click();
            Assert.True(Display.Name.Equals("0"));

            Logging.Write(TestContext.CurrentContext.Result.Status.ToString().ToUpper() + "!");
        }

        private readonly Process proc = Process.Start(@"C:\Windows\System32\calc.exe");
        public static UIWindow MainWindow;
        public UIControl Display;

        public TestContext TestContext { get; set; }

        [TestFixtureSetUp]
        public void Iint()
        {
            Application app = Application.Attach(proc);
            app.WaitWhileBusy();
            MainWindow = new UIWindow("Calculator");
            Display = MainWindow.FindControl(By.AutomationId("150"));
            Assert.True(Display.Name.Equals("0"));
        }

        [TestFixtureTearDown]
        public void Cleanup()
        {
            Process.GetProcessById(proc.Id).Kill();
        }
    }
}