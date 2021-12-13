using ClassLibrary1;
using Figgle;

Tag.How("Program");

Tag.Where("Main");

while (true)
{
    var color = Color.Blue;

    Tag.Why("PreRunCall");

    await global::ClassLibrary1.Application.Run(Constant.ApiEndpoint, color);

    Tag.Why("PostRunCall");

    Console.WriteLine(Tag.Line(FiggleFonts.Standard.Render(color)));

    await Task.Delay(1000);

    Console.ResetColor();
}
