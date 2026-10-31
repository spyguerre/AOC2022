using AOC2022.Common;


// Unknown number of arguments
if (args.Length <= 0)
{
    Console.WriteLine("No input argument. Syntax: 'dotnet run -- {(int) day} {(optional bool) useTestInput}'");
    Environment.Exit(1);
} else if (args.Length >= 3)
{
    Console.WriteLine("Unknown arguments. Syntax: 'dotnet run -- {(int) day} {(optional bool) useTestInput}'");
    Environment.Exit(1);
}


int day = int.Parse(args[0]);

string className = $"Day{day:D2}";
Type type = Type.GetType($"AOC2022.{className}.{className}")!;
IPuzzle[] instances;

if (args.Length == 1)
{
    instances = [
        (IPuzzle) Activator.CreateInstance(type, [false])!,
        (IPuzzle) Activator.CreateInstance(type, [false])!
    ];

} else
{
    instances = [
        (IPuzzle) Activator.CreateInstance(type, [bool.Parse(args[1])])!,
        (IPuzzle) Activator.CreateInstance(type, [bool.Parse(args[1])])!
    ];
}

Console.WriteLine($"Part 1: {instances[0].Part1()}");
Console.WriteLine($"Part 2: {instances[1].Part2()}");
