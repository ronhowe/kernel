using ClassLibrary1.Domain.Entities;

internal interface IPacketService
{
    public Task IO(Packet packet);
}