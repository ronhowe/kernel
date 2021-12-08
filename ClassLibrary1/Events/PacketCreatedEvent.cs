using ClassLibrary1.Domain.Common;
using ClassLibrary1.Domain.Entities;

namespace ClassLibrary1.Domain.Events
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
