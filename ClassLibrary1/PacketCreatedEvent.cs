namespace ClassLibrary1
{
    public class PacketCreatedEvent : DomainEvent
    {
        public PacketCreatedEvent(Packet packet)
        {
            Packet = packet;
        }

        public Packet Packet { get; }
    }
}
