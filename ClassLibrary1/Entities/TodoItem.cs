using ClassLibrary1.Domain.Common;
using ClassLibrary1.Domain.Enums;
using ClassLibrary1.Domain.Events;

namespace ClassLibrary1.Domain.Entities
{
    public class TodoItem : AuditableEntity, IHasDomainEvent
    {
        public int Id { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public TodoList List { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public int ListId { get; set; }

        public string Title { get; set; } = DefaultValues.Undefined;

        public string Note { get; set; } = DefaultValues.Undefined;

        public PriorityLevel Priority { get; set; }

        public DateTime? Reminder { get; set; }

        private bool _done;
        public bool Done
        {
            get => _done;
            set
            {
                if (value == true && _done == false)
                {
                    DomainEvents.Add(new TodoItemCompletedEvent(this));
                }

                _done = value;
            }
        }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
