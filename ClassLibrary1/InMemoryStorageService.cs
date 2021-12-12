namespace ClassLibrary1
{
    public static class InMemoryStorageService
    {
        private static Packet EmptyPacket = PacketFactory.Empty();

        public static async Task<Packet> IO(Packet packet)
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

            Tag.Line($"receivedPacket={receivedPacket}");

            Tag.Why("PostOuput");

            packet.Received = true;

            packet.Color = receivedPacket.Color;

            Tag.Why("IOComplete");

            return packet;
        }

        public static async Task Write(Packet packet)
        {
            Tag.Where("Input");

            Tag.Why("InputStart");

            EmptyPacket = packet;

            Tag.Why("InputComplete");
        }

        public static async Task<Packet> Read(Guid id)
        {
            Tag.Where("Output");

            Tag.Why("OutputStart");

            Tag.What($"id={id}");

            var emptyPacket = EmptyPacket;

            Tag.Why("OutputComplete");

            return emptyPacket;
        }
    }
}