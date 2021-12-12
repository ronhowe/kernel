using ClassLibrary1.Common;
using ClassLibrary1.Domain.ValueObjects;
using ClassLibrary1.Services;
using Figgle;

Tag.How("Program");

Tag.Where("Main");

while (true)
{
    Console.ResetColor();

    var application = new NewApplication();

    var color = PacketColor.Blue;

    await application.Run(color);

    Console.WriteLine(Tag.Line(FiggleFonts.Standard.Render(color)));

    await Task.Delay(1000);
}
