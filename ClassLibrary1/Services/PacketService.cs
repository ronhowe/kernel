using ClassLibrary1.Common;
using ClassLibrary1.Domain.Entities;

public class PacketService : IPacketService
{
    public async Task IO(Packet packet)
    {
        Tag.Why("PreLocalStorageService");

        await LocalStorageService.IO(packet);

        Tag.Why("PostLocalStorageService");
    }
}