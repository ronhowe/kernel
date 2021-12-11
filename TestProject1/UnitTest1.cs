using Azure;
using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ClassLibrary1.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize()]
        public async Task TestInitialize()
        {
            await Task.Run(() => Tag.How("UnitTest1"));

            await Task.Run(() => Tag.Where("TestInitialize"));
        }

        [TestMethod]
        public async Task Main()
        {
            await Task.Run(() => Tag.Where("Main"));
        }

        [TestMethod]
        public async Task Tags()
        {
            await Task.Run(() => Tag.Who("Who"));
            await Task.Run(() => Tag.What("What"));
            await Task.Run(() => Tag.Where("Where"));
            await Task.Run(() => Tag.When("When"));
            await Task.Run(() => Tag.Why("Why"));
            await Task.Run(() => Tag.How("How"));
            await Task.Run(() => Tag.Warning("Warning"));
            await Task.Run(() => Tag.Error("Error"));
            await Task.Run(() => Tag.Secret("Secret"));
            await Task.Run(() => Tag.Comment("Comment"));
        }

    }
}
