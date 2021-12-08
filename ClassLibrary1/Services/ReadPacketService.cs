using ClassLibrary1.Domain.Entities;

namespace ClassLibrary1.Services
{
    public class ReadPacketService : IReadPacketService
    {
        public Packet Read()
        {
            return new Packet();
        }
    }
}
