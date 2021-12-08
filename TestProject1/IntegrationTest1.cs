using ClassLibrary1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class IntegrationTest1
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            Trace.WriteLine("@IntegrationTest.cs1");

            Trace.WriteLine("@TestInitialize()");
        }

        [TestMethod]
        public async Task HealthCheck()
        {
            Trace.WriteLine("@HealthCheck()");

            // @TODO @RefactorRunAsync
            await EndpointCallHelper.RunAsync(EndpointMap.HealthCheckEndpoint, false);

            // @TODO @Assert
        }

        [TestMethod]
        public async Task Read()
        {
            Trace.WriteLine("@Read()");

            await EndpointCallHelper.RunAsync(EndpointMap.IoEndpoint, true);

            // @TODO @Assert
        }

        [TestMethod]
        public async Task Write()
        {
            Trace.WriteLine("@Write()");

            await EndpointCallHelper.RunAsync(EndpointMap.IoEndpoint, true);

            // @TODO @Assert
        }
    }
}
