using ClassLibrary1.Domain.Common;
using ClassLibrary1.Domain.ValueObjects;

namespace ClassLibrary1.Domain.Entities
{
    public class TodoList : AuditableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public Color Color { get; set; } = Color.White;

        public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
    }
}
