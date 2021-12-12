namespace ClassLibrary1
{
    public static class PacketFactory
    {
        public static Packet Create(PacketColor color)
        {
            return new() { Id = Guid.NewGuid(), Color = color };
        }

        public static Packet Empty()
        {
            return new() { Id = Guid.Empty, Color = PacketColor.Black};
        }
    }
}
