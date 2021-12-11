using ClassLibrary1.Domain.Entities;
using ClassLibrary1.Domain.ValueObjects;

Console.Clear();

Packet packet = new() { Id = Guid.NewGuid(), Color = PacketColor.Green };

PacketService service = new();

service.IO(packet);

Console.ForegroundColor = ConsoleColor.Green;

Console.WriteLine(packet);
