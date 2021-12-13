using ClassLibrary1;
using Figgle;

Tag.How("Program");

Tag.Where("Main");

string host;

if (args.Length == 0)
{
    host = "https://localhost:9999";
} else
{
    host = args[0];
}

Console.ForegroundColor = ConsoleColor.Blue;

while (true)
{
    var color = Color.Blue;

    Tag.Why("PreRunCall");

    await Application.Run(host, color);

    Tag.Why("PostRunCall");

    Console.WriteLine(Tag.Line(FiggleFonts.Standard.Render(color)));

    await Task.Delay(1000);

    Console.ResetColor();
}
