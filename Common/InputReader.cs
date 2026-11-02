namespace AOC2022.Common;


public class InputReader
{
    public static string[] ReadLines(int day, bool useTestInput = false)
    {
        return File.ReadAllLines(Path.Join($"Day{day:D2}", useTestInput ? "testInput.txt" : "input.txt"));
    }

    public static int?[] ReadIntegers(int day, bool useTestInput = false)
    {
        return [.. ReadLines(day, useTestInput).Select(line => int.TryParse(line, out int lineInt) ? (int?)lineInt : null)];
    }

    public static char[][] ReadChars(int day, bool useTestInput = false)
    {
        return [.. ReadLines(day, useTestInput).Select(line => line.ToCharArray())];
    }
}
