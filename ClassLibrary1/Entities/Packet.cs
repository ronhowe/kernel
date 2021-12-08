using ClassLibrary1.Domain.Common;
using ClassLibrary1.Domain.Events;
using ClassLibrary1.Domain.ValueObjects;

namespace ClassLibrary1.Domain.Entities
{
    public class Packet : AuditableEntity, IHasDomainEvent
    {
        public Guid Id { get; set; } = Guid.Empty;

        public Guid ReferenceId { get; set; } = Guid.Empty;

        public Color Color { get; set; } = Color.Black;

        private bool _received;

        public bool Received
        {
            get => _received;
            set
            {
                if (value == true && _received == false)
                {
                    DomainEvents.Add(new PacketReceivedEvent(this));
                }

                _received = value;
            }
        }

        private bool _sent;

        public bool Sent
        {
            get => _sent;
            set
            {
                if (value == true && _sent == false)
                {
                    DomainEvents.Add(new PacketSentEvent(this));
                }

                _sent = value;
            }
        }

        public override string ToString()
        {
            return $"{Id}.{ReferenceId}.{Color}.{Sent}.{Received}";
        }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
