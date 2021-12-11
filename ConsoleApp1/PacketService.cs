using ClassLibrary1.Domain.Entities;

internal class PacketService : IPacketService
{
    public void IO(Packet packet)
    {
        packet.Sent = true;
        packet.Received = true;
    }
}