using System;
using System.Diagnostics;
using NUnit.Framework;
using TestStack.White;
using WhiteSharp.Tests.ScreenObjects;

namespace WhiteSharp.Tests
{
    /// <summary>
    /// This class sets up test enviroment.
    /// It runs test application and takes screenshot after every failed test.
    /// </summary>
    [TestFixture(Ignore = true)]
    public class TestBaseNUnit
    {
        public static string Path = @"..\..\..\..\\TestApps\\WpfTestApplication.exe";
        public static string ResultsPath = "\\Results";
        private readonly Process _proc = Process.Start(Path);

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
            Logging.Info(TestContext.CurrentContext.Result.Status.ToString().ToUpper() + "!");
        }

        public TestContext TestContext { get; set; }

        [TestFixtureSetUp]
        public void Init()
        {
            Application app = Application.Attach(_proc);
            app.WaitWhileBusy();
            Settings.Default.Timeout = 1000;
        }

        [TestFixtureTearDown]
        public void Cleanup()
        {
            Process.GetProcessById(_proc.Id).Kill();
        }
    }
}