using ClassLibrary1.Domain.Entities;

namespace ClassLibrary1.Services
{
    internal interface IWritePacketService
    {
        public void Write(Packet p);
    }
}
