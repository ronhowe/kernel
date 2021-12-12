using ClassLibrary1.Common;
using ClassLibrary1.Contants;
using ClassLibrary1.Domain.ValueObjects;
using ClassLibrary1.Services;
using Figgle;

Tag.How("Program");

Tag.Where("Main");

while (true)
{
    var application = new Application();

    var color = PacketColor.Blue;

    Tag.Why("PreRunCall");

    await application.Run(Constant.ApiEndpoint, color);

    Tag.Why("PostRunCall");

    Console.WriteLine(Tag.Line(FiggleFonts.Standard.Render(color)));

    await Task.Delay(1000);

    Console.ResetColor();
}
