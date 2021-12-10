using ClassLibrary1.Domain.Common;
using ClassLibrary1.Domain.Events;
using ClassLibrary1.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace ClassLibrary1.Domain.Entities
{
    public class Packet : AuditableEntity, IHasDomainEvent
    {
        [JsonConstructor]
        public Packet()
        {
            Id = Guid.Empty;
            ReferenceId = Guid.Empty;
            Color = PacketColor.Black;
        }

        public Guid Id { get; set; }

        public Guid ReferenceId { get; set; }

        public PacketColor Color { get; set; }

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
