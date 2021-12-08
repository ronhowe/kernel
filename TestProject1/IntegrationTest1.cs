using ClassLibrary1.Infrastructure;
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
        public async Task Post()
        {
            Trace.WriteLine("@Post()");

            // @TODO @RefactorRunAsync
            await EndpointCallHelper.RunAsync(Endpoints.POST, false);

            // @TODO @Assert
        }

        [TestMethod]
        public async Task Read()
        {
            Trace.WriteLine("@Read()");

            await EndpointCallHelper.RunAsync(Endpoints.BIOS, true);

            // @TODO @Assert
        }

        [TestMethod]
        public async Task Write()
        {
            Trace.WriteLine("@Write()");

            await EndpointCallHelper.RunAsync(Endpoints.BIOS, true);

            // @TODO @Assert
        }
    }
}
