using ClassLibrary1.Common;
using ClassLibrary1.Domain.Entities;
using ClassLibrary1.Domain.ValueObjects;
using ClassLibrary1.Entities;
using ClassLibrary1.Infrastructure;
using Figgle;
using System.Net.Http.Json;
using System.Text.Json;

namespace ClassLibrary1.Services
{
    public class Application
    {
        public async Task Run(string uriString, PacketColor color)
        {
            Tag.Where("Run");

            Tag.What($"uriString={uriString}");
            Tag.What($"color={color}");

            HttpClient httpClient = new() { BaseAddress = new Uri(uriString) };

            Packet sentPacket = PacketFactory.Create(color);

            try
            {
                Tag.Why("PrePostAsJsonAsyncCall");

                var httpResponse = await httpClient.PostAsJsonAsync(Endpoints.BIOS, sentPacket);

                Tag.Why("PostPostAsJsonAsyncCall");

                Tag.Line(FiggleFonts.Standard.Render(httpResponse.StatusCode.ToString()));

                Tag.Why("PreGetFromJsonAsyncCall");

                var receivedPacket = await httpClient.GetFromJsonAsync<Packet>($"{Endpoints.BIOS}?id={sentPacket.Id}");

                Tag.Why("PostGetFromJsonAsyncCall");

                Tag.ToDo("ImplementSentAndReceivedProperties");

                Tag.Line($"sentPacket={sentPacket}");
                Tag.Line($"receivedPacket={receivedPacket}");
            }
            catch (HttpRequestException ex)
            {
                Tag.Error("HttpRequestException");
                Tag.Error(ex.Message);
                Tag.Comment("Runbook12345"); // Runbook stating to turn on website.
                throw ex;
            }
            catch (NotSupportedException ex)
            {
                Tag.Error("ContentTypeNotSupported");
                Tag.Error(ex.Message);
                throw ex;
            }
            catch (JsonException ex)
            {
                Tag.Error("InvalidJson");
                Tag.Error(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                Tag.Error("UnknownException");
                Tag.Error(ex.Message);
                throw;
            }
        }
    }
}