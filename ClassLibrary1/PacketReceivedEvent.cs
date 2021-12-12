namespace ClassLibrary1
{
    public class PacketReceivedEvent : DomainEvent
    {
        public PacketReceivedEvent(Packet packet)
        {
            Packet = packet;
        }

        public Packet Packet { get; }
    }
}
