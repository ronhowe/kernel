using ClassLibrary1.Domain.Common;
using ClassLibrary1.Domain.Entities;

namespace ClassLibrary1.Domain.Events
{
    public class TodoItemCompletedEvent : DomainEvent
    {
        public TodoItemCompletedEvent(TodoItem item)
        {
            Item = item;
        }

        public TodoItem Item { get; }
    }
}
