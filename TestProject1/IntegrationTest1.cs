using ClassLibrary1.Common;
using ClassLibrary1.Domain.Entities;
using ClassLibrary1.Domain.ValueObjects;
using ClassLibrary1.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class IntegrationTest1
    {
        [TestInitialize()]
        public async Task TestInitialize()
        {
            await Task.Run(() => Tag.How("IntegrationTest1"));
            await Task.Run(() => Tag.Where("TestInitialize"));
        }

        [TestMethod]
        public async Task Run()
        {
            Tag.Where("Run");

            Tag.Why("RunStart");

            Packet packet = new() { Id = Guid.NewGuid(), Color = PacketColor.Blue };

            PacketService service = new();

            Tag.Why("PreIO");

            service.IO(packet);

            Tag.Why("PostIO");

            Tag.What(packet.ToString());

            Tag.Why("RunComplete");
        }

        [TestMethod]
        public async Task PostEndpoint()
        {
            Tag.Where("Post");

            await EndpointCallHelper.RunAsync(Endpoints.POST, false);
        }

        [TestMethod]
        public async Task IOEndpoint()
        {
            Tag.Where("IOEndpoint");

            await EndpointCallHelper.RunAsync(Endpoints.BIOS, true);
        }

        //[TestMethod]
        //public async Task Write()
        //{
        //    Tag.Where("Write");

        //    await EndpointCallHelper.RunAsync(Endpoints.BIOS, true);
        //}
    }
}
