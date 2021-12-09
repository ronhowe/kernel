using ClassLibrary1.Common;
using ClassLibrary1.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class IntegrationTest1
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            Tag.Where("IntegrationTest1.cs1");

            Tag.Where("TestInitialize");
        }

        [TestMethod]
        public async Task Post()
        {
            Tag.Where("Post");

            // @TODO @RefactorRunAsync
            await EndpointCallHelper.RunAsync(Endpoints.POST, false);

            // @TODO @Assert
        }

        [TestMethod]
        public async Task Read()
        {
            Tag.Where("Read");

            await EndpointCallHelper.RunAsync(Endpoints.BIOS, true);

            // @TODO @Assert
        }

        [TestMethod]
        public async Task Write()
        {
            Tag.Where("Write");

            await EndpointCallHelper.RunAsync(Endpoints.BIOS, true);

            // @TODO @Assert
        }
    }
}
