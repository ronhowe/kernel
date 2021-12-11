using ClassLibrary1.Common;
using ClassLibrary1.Domain.ValueObjects;
using ClassLibrary1.Services;
using Figgle;

Tag.How("Program");

Tag.Where("Main");

while (true)
{
    Console.ResetColor();

    var application = new Application();

    Tag.Why("PreRunCall");

    await application.Run(PacketColor.Blue);

    Tag.Why("PostRunCall");

    Console.ForegroundColor = ConsoleColor.Blue;

    Console.WriteLine(Tag.Line(FiggleFonts.Standard.Render(PacketColor.Blue)));

    await Task.Delay(1000);
}
