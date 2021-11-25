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
            // TODO - Use async
            var client = new HttpClient();

            var response = client.GetAsync("https://localhost:7078/weatherforecast").Result;

            response.EnsureSuccessStatusCode();

            var responseBody = response.Content.ReadAsStringAsync().Result;

            Trace.WriteLine(responseBody);
        }

        private static void WritePostHeader()
        {
            var header = String.Format("POST @ {0}", DateTime.Now);
            Trace.WriteLine(header);
        }
    }
}