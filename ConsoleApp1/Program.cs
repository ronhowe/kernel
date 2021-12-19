using ClassLibrary1;
using Figgle;

Tag.How("Program");

Tag.Where("Main");

string host;

if (args.Length == 0)
{
    host = "https://localhost:9999";
}
else
{
    host = args[0];
}

var color = Color.Blue;

while (true)
{
    Console.ResetColor();

    Tag.Why("PreRunCall");

    Tag.ToDo("RefactorApplicationRun");
    //await Application.Run(host, color);

    Tag.Why("PostRunCall");

    Console.WriteLine(Tag.Shout(color));

    Console.ForegroundColor = ConsoleColor.Blue;

    await Task.Delay(1000);
}