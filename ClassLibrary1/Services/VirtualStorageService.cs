namespace ClassLibrary1
{
    public static class VirtualStorageService
    {
        private static Photon InMemoryPhoton = PhotonFactory.Empty();

        public static async Task<Photon> IO(Photon photon)
        {
            Tag.Where("IO");

            Tag.Why("IOStart");

            Tag.Why("PreInputCall");

            Tag.What($"photon={photon}");

            await Write(photon);

            Tag.Why("PostInputCall");

            photon.Sent = true;

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

            await Task.Run(() => InMemoryPhoton = photon);

            Tag.Why("InputComplete");
        }

        public static async Task<Photon> Read(Guid id)
        {
            Tag.Where("Output");

            Tag.Why("OutputStart");

            Tag.What($"id={id}");

            var photon = await Task.Run(() => InMemoryPhoton);

            Tag.Why("OutputComplete");

            return photon;
        }
    }
}