using System;
using NUnit.Framework;
using WhiteExtension;
using WhiteExtension.UnitTests;

namespace SampleTests
{
    [TestFixture]
    public class SampleTest : TestBaseNUnit
    {
        [Test]
        public void TestMethod1()
        {
            var w = new UIWindow("Calc");
        }
    }
}
