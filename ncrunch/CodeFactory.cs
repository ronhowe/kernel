using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

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
        public void Unauthorized()
        {
            WriteTestHeader("Unauthorized");

            using var sut = new SystemUnderTest();
            using var client = sut.CreateClient();
            using var response = client.GetAsync("/weatherforecast");

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.Result.StatusCode);
        }


        [TestMethod]
        public void Authorized()
        {
            WriteTestHeader("Authorized");

            Assert.Inconclusive("TODO");
        }


        private static void WriteTestHeader(string tag)
        {
            Trace.WriteLine(String.Format("{0} @ {1}", tag, DateTime.Now));
        }
    }
}