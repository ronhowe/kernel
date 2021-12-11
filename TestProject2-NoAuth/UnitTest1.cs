using ClassLibrary1.Common;
using ClassLibrary1.Domain.ValueObjects;
using ClassLibrary1.Entities;
using Figgle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestProject2_NoAuth
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            Tag.Where("TestMethod1");

            const string baseAddress = "https://localhost:7087";

            Tag.What($"baseAddress={baseAddress}");

            HttpClient client = new() { BaseAddress = new Uri(baseAddress) };

            try
            {
                Tag.Why("PrePostAsJsonAsyncCall");

                var response = await client.PostAsJsonAsync("/weatherforecast", PacketFactory.Create(PacketColor.Red));

                Tag.Why("PostPostAsJsonAsyncCall");

                Tag.Line(FiggleFonts.Standard.Render(response.StatusCode.ToString()));
            }
            catch (HttpRequestException ex)
            {
                Tag.Error("HttpRequestError");
                Tag.Error(ex.Message);
            }
            catch (NotSupportedException ex)
            {
                Tag.Error("ContentTypeNotSupported");
                Tag.Error(ex.Message);
            }
            catch (JsonException ex)
            {
                Tag.Error("InvalidJson");
                Tag.Error(ex.Message);
            }
        }
    }
}