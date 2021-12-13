using ClassLibrary1;
using Figgle;

Tag.How("Program");

Tag.Where("Main");

while (true)
{
    var color = Color.Blue;

    Tag.Why("PreRunCall");

    await Application.Run(Constant.LocalApiEndpoint, color);

    Tag.Why("PostRunCall");

    Console.WriteLine(Tag.Line(FiggleFonts.Standard.Render(color)));

    await Task.Delay(1000);

    Console.ResetColor();
}
