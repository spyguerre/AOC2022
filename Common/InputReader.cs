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

    public static char[,] ReadMatrix(int day, bool useTestInput = false)
    {
        string[] lines = ReadLines(day, useTestInput);
        int n = lines.Length;
        int p = n == 0 ? 0 : lines[0].Length;
        char[,] res = new char[n, p];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < p; j++)
            {
                res[i, j] = lines[i][j];
            }
        }

        return res;
    }

    public static int[,] ReadIntegerMatrix(int day, bool useTestInput = false)
    {
        char[,] charMatrix = ReadMatrix(day, useTestInput);
        int n = charMatrix.GetLength(0);
        int p = charMatrix.GetLength(1);
        int[,] res = new int[n, p];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < p; j++)
            {
                res[i, j] = int.Parse(charMatrix[i, j].ToString());
            }
        }
        return res;
    }
}
