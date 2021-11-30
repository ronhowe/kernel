using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace ncrunch
{
    [TestClass]
    public class ExplorationTests
    {
        [TestMethod]
        public void Run()
        {
            Trace.TraceInformation("Let's go exploring!");
            Assert.IsTrue(true);
        }
    }
}
