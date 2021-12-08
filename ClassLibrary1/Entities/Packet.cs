using ClassLibrary1.Domain.Common;
using ClassLibrary1.Domain.Enums;
using ClassLibrary1.Domain.Events;
using ClassLibrary1.Domain.ValueObjects;

namespace ClassLibrary1.Domain.Entities
{
    public class Packet : AuditableEntity, IHasDomainEvent
    {
        public Guid Id { get; set; }

        public Guid ReferenceId { get; set; }

        public Color Color { get; set; } = Color.White;

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

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
