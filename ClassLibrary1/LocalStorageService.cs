using System.Text;
using System.Text.Json;

namespace ClassLibrary1
{
    public static class LocalStorageService
    {
        public static async Task<Photon> IO(Photon photon)
        {
            Tag.Where("IO");

            Tag.Why("IOStart");

            Tag.Why("PreInputCall");

            Tag.What($"photon={photon}");

            await Write(photon);

            Tag.Why("PostInputCall");

            photon.Sent = true;

            Tag.ToDo("FixPreprocessorDirectiveBug12345");

#if false
            // Only Shows In Debug
            await ReadTextAsync($"{photon.Id}.json");
#endif

            Tag.Why("PreOuput");

            var receivedPhoton = await Read(photon.Id);

            Tag.What($"receivedPhoton={receivedPhoton}");

            Tag.Why("PostOuput");

            photon.Received = true;

            photon.Color = receivedPhoton.Color;

            Tag.Why("IOComplete");

            return photon;
        }

        public static async Task Write(Photon photon)
        {
            Tag.Where("Input");

            Tag.Why("InputStart");

            string fileName = $"{Path.GetTempPath()}\\{photon.Id}.json";

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

            await JsonSerializer.SerializeAsync(createStream, photon, optionsCopy);

            Tag.Why("PostSerializeAsyncCall");

            await createStream.DisposeAsync();

            Tag.ToDo("RemovePostSerializeAsync");
            await ReadTextAsync(fileName);

            Tag.Why("InputComplete");
        }

        public static async Task<Photon> Read(Guid id)
        {
            Tag.Where("Output");

            Tag.Why("OutputStart");

            Tag.What($"id={id}");

            string fileName = $"{Path.GetTempPath()}\\{id}.json";

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

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Photon deserializedPhoton = await JsonSerializer.DeserializeAsync<Photon>(openStream, options);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (deserializedPhoton == null)
            {
                Tag.Error("DeserializationException");
                throw new Exception("DeserializationException");
            }

            Tag.Why("PostDeserializeAsync");

            Tag.ToDo("SimulateReadCorruptionFeature12345");

            if (false)
            {
#pragma warning disable CS0162 // Unreachable code detected
                deserializedPhoton.Id = Guid.NewGuid();
#pragma warning restore CS0162 // Unreachable code detected
            }

            Tag.What($"deserializedPhoton={deserializedPhoton}");

            Tag.Why("OutputComplete");

            return deserializedPhoton;
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
#pragma warning disable CS0162 // Unreachable code detected
                    Tag.Line($"{sb}");
#pragma warning restore CS0162 // Unreachable code detected
                }

                return sb.ToString();
            });

            task.Start();

            return task;
        }
    }
}