using ClassLibrary1.Common;
using ClassLibrary1.Domain.Entities;
using System.Text;
using System.Text.Json;

public class LocalStorageService
{
    public static async Task<Packet> IO(Packet packet)
    {
        Tag.Where("IO");

        Tag.Why("IOStart");

        Tag.Why("PreInputCall");

        Tag.What($"packet={packet}");

        await Input(packet);

        Tag.Why("PostInputCall");

        packet.Sent = true;

        Tag.ToDo("FixPreprocessorDirectiveBug12345");

#if false
            // Only Shows In Debug
            await ReadTextAsync($"{packet.Id}.json");
#endif

        Tag.Why("PreOuput");

        var receivedPacket = await Output(packet.Id);

        Tag.Line($"receivedPacket={receivedPacket}");

        Tag.Why("PostOuput");

        packet.Received = true;

        packet.Color = receivedPacket.Color;

        Tag.Why("IOComplete");

        return packet;
    }

    public static async Task Input(Packet packet)
    {
        Tag.Where("Input");

        Tag.Why("InputStart");

        string fileName = $"{packet.Id}.json";

        Tag.Why("SerializedJsonFile");

        Tag.What($"fileName={fileName}");

        using FileStream createStream = File.Create(fileName);

        Tag.What($"createStream.Name={createStream.Name}");

        JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        };

        JsonSerializerOptions optionsCopy = new(options);

        Tag.Why("PreSerializeAsyncCall");

        await JsonSerializer.SerializeAsync(createStream, packet, optionsCopy);

        Tag.Why("PostSerializeAsyncCall");

        await createStream.DisposeAsync();

        Tag.ToDo("RemovePostSerializeAsync");
        await ReadTextAsync(fileName);

        Tag.Why("InputComplete");
    }

    public static async Task<Packet> Output(Guid id)
    {
        Tag.Where("Output");

        Tag.Why("OutputStart");

        Tag.What($"id={id}");

        string fileName = $"{id}.json";

        Tag.What($"fileName={fileName}");

        Tag.ToDo("RemovePreDeserializeAsyncRead");
        await ReadTextAsync(fileName);

        using FileStream openStream = File.OpenRead(fileName);

        Tag.What($"openStream.Name={openStream.Name}");

        JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        };

        JsonSerializerOptions optionsCopy = new(options);

        Tag.Why("PreDeserializeAsync");

        Packet deserializedPacket = await JsonSerializer.DeserializeAsync<Packet>(openStream, options);

        Tag.Why("PostDeserializeAsync");

        Tag.ToDo("SimulateReadCorruptionFeature12345");

        if (false)
        {
            deserializedPacket.Id = Guid.NewGuid();
        }

        Tag.What($"deserializedPacket={deserializedPacket}");

        Tag.Why("OutputComplete");

        return deserializedPacket;
    }

    private static Task<string> ReadTextAsync(string filePath)
    {
        var task = new Task<string>(() =>
        {
            using FileStream sourceStream = new(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true);

            StringBuilder sb = new();

            byte[] buffer = new byte[0x1000];
            int numRead;

            while ((numRead = sourceStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string text = Encoding.ASCII.GetString(buffer, 0, numRead);
                sb.Append(text);
            }

            Tag.ToDo("RefactorTagBlockFunctionalityFeature12345");

            if (false)
            {
                Tag.Line($"{sb}");
            }

            return sb.ToString();
        });

        task.Start();

        return task;
    }
}
