using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace ncrunch
{
    [TestClass]
    public class CodeFactory
    {
        [TestMethod]
        public void Post()
        {
            WritePostHeader();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Run()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7078")
            };

            var response = client.GetAsync(client.BaseAddress).Result;

            Trace.WriteLine(response.StatusCode);
        }

        private static void WritePostHeader()
        {
            var header = String.Format("POST @ {0}", DateTime.Now);
            Trace.WriteLine(header);
        }
    }
}