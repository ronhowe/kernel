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
    public class NewApplication
    {
        public async Task Run(PacketColor color)
        {
            Tag.Where("Main");

            const string baseAddress = "https://localhost:7087";

            Tag.What($"baseAddress={baseAddress}");

            HttpClient httpClient = new() { BaseAddress = new Uri(baseAddress) };

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