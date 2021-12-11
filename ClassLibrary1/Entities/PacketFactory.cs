using ClassLibrary1.Domain.Entities;
using ClassLibrary1.Domain.ValueObjects;

namespace ClassLibrary1.Entities
{
    public static class PacketFactory
    {
        public static Packet Create(PacketColor color)
        {
            return new() { Id = Guid.NewGuid(), Color = color };
        }
    }
}
