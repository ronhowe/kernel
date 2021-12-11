using ClassLibrary1.Domain.Entities;

public class PacketService : IPacketService
{
    public void IO(Packet packet)
    {
        packet.Sent = true;
        packet.Received = true;
    }
}