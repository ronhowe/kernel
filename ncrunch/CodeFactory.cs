using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace ncrunch
{
    [TestClass]
    public class CodeFactory
    {
        [TestMethod]
        public void POST()
        {
            WritePostHeader();
            Assert.IsTrue(true);
        }

        private static void WritePostHeader()
        {
            Trace.WriteLine("POST");
            Trace.WriteLine(DateTime.Now);
        }
    }
}