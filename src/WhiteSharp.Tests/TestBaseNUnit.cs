using System;
using System.Diagnostics;
using NUnit.Framework;
using TestStack.White;

namespace WhiteSharp.Tests
{
    /// <summary>
    ///     This class sets up test enviroment.
    ///     It runs test application and takes screenshot after every failed test.
    /// </summary>
    [TestFixture(Ignore = true)]
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
            Logging.Info(TestContext.CurrentContext.Result.Status.ToString().ToUpper() + "!");
        }

        public static string Path = @"..\..\..\..\\TestApps\\WpfTestApplication.exe";
        public static string ResultsPath = "\\Results";
        private Process _proc;

        public TestContext TestContext { get; set; }

        [TestFixtureSetUp]
        public void Init()
        {
            _proc = Process.Start(Path);

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