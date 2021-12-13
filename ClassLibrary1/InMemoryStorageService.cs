namespace ClassLibrary1
{
    public static class InMemoryStorageService
    {
        private static Photon EmptyPacket = PhotonFactory.Empty();

        public static async Task<Photon> IO(Photon packet)
        {
            Tag.Where("IO");

            Tag.Why("IOStart");

            Tag.Why("PreInputCall");

            Tag.What($"packet={packet}");

            await Write(packet);

            Tag.Why("PostInputCall");

            packet.Sent = true;

            Tag.Why("PreOuput");

            var receivedPacket = await Read(packet.Id);

            Tag.What($"receivedPacket={receivedPacket}");

            Tag.Why("PostOuput");

            packet.Received = true;

            packet.Color = receivedPacket.Color;

            Tag.Why("IOComplete");

            return packet;
        }

        public static async Task Write(Photon packet)
        {
            Tag.Where("Input");

            Tag.Why("InputStart");

            await Task.Run(() => EmptyPacket = packet);

            Tag.Why("InputComplete");
        }

        public static async Task<Photon> Read(Guid id)
        {
            Tag.Where("Output");

            Tag.Why("OutputStart");

            Tag.What($"id={id}");

            var emptyPacket = await Task.Run(() => EmptyPacket);

            Tag.Why("OutputComplete");

            return emptyPacket;
        }
    }
}