using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Net;

namespace ncrunch
{
    [TestClass]
    public class CodeFactory
    {
        [TestMethod]
        public void Post()
        {
            WriteTestHeader("Post");
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Run()
        {
            WriteTestHeader("Run");
            Assert.IsTrue(true);

        }

        [TestMethod]
        public void Integration()
        {
            WriteTestHeader("Integration");

            using var sut = new SystemUnderTest();
            using var client = sut.CreateClient();
            using var response = client.GetAsync("/weatherforecast");

            var responseBody = response.Result.Content.ReadAsStringAsync().Result;

            Trace.WriteLine(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
        }


        private static void WriteTestHeader(string tag)
        {
            Trace.WriteLine(String.Format("{0} @ {1}", tag, DateTime.Now));
        }
    }
}