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
        }

        private readonly Process proc = Process.Start(@"C:\Windows\System32\calc.exe");

        public TestContext TestContext { get; set; }

        [TestFixtureSetUp]
        public void Iint()
        {
            Application app = Application.Attach(proc);
            app.WaitWhileBusy();
        }

        [TestFixtureTearDown]
        public void Cleanup()
        {
            Process.GetProcessById(proc.Id).Kill();
        }
    }
}