namespace ClassLibrary1
{
    public class PacketSentEvent : DomainEvent
    {
        public PacketSentEvent(Packet packet)
        {
            Packet = packet;
        }

        public Packet Packet { get; }
    }
}
