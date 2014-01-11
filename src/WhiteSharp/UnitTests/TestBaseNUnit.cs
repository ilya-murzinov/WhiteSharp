using System;
using System.Diagnostics;
using NUnit.Framework;
using TestStack.White;
using System.IO;

namespace WhiteSharp.UnitTests
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
            if (TestContext.CurrentContext.Result.Status == TestStatus.Failed)
            {
                string name = TestContext.CurrentContext.Test.Name;
                new ScreenCapture().CaptureScreenShot()
                    .Save(ResultsPath + name.Substring(0, name.IndexOf("(", StringComparison.Ordinal)) + ".bmp");
            }
            Logging.Write(TestContext.CurrentContext.Result.Status.ToString().ToUpper() + "!");
        }

        public UIWindow Window;
        public static string WindowTitle = "MainWindow";
        public static string Path = @"..\..\..\..\\TestApps\\WpfTestApplication.exe";
        public static string ResultsPath = "\\Results";
        private readonly Process proc = Process.Start(Path);

        public TestContext TestContext { get; set; }

        [TestFixtureSetUp]
        public void Iint()
        {
            Application app = Application.Attach(proc);
            app.WaitWhileBusy();
            Window = new UIWindow(WindowTitle);
            Settings.Default.Timeout = 1000;
        }

        [TestFixtureTearDown]
        public void Cleanup()
        {
            Process.GetProcessById(proc.Id).Kill();
        }
    }
}