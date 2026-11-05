using System.Data;
using System.Numerics;
using System.Text.RegularExpressions;
using AOC2022.Common;

namespace AOC2022.Day11;


public class Day11 : IPuzzle
{
    private readonly Dictionary<string, Monkey> monkeys = [];

    public Day11(bool useTestInput = false)
    {
        string[] lines = [.. InputReader.ReadLines(11, useTestInput).Concat((string[]) [""])]; //  Add newline to make last monkey's reading easier

        string? name = null;
        int[]? startingItems = null;
        Func<BigInteger, BigInteger>? operation = null;

        int? divisor = null;
        string? monkeyTrueName = null;
        string? monkeyFalseName = null;

        // First instanciate the monkeys with their name, starting items and operations only
        foreach (string line in lines)
        {
            if (line.StartsWith("Monkey "))
            {
                Match match = Regex.Match(line, @"Monkey (\w+):");
                name = match.Groups[1].Value;
            } else if (line.Contains("Starting items: "))
            {
                string itemsString = line.Split("Starting items: ")[1];
                startingItems = [.. itemsString.Split(", ").Select(int.Parse)];
            } else if (line.Contains("Operation: new = ")) {
                string[] opParts = line.Split("Operation: new = ")[1].Split(" ");
                operation = old =>
                {
                    BigInteger a = opParts[0] == "old" ? old : BigInteger.Parse(opParts[0]);
                    BigInteger b = opParts[2] == "old" ? old : BigInteger.Parse(opParts[2]);
                    return char.Parse(opParts[1]) switch {'*' => a * b, '+' => a + b, _ => throw new DataException($"Unsupported operator: '{opParts[1]}'")};
                };
            } else if (line == "")
            {
                monkeys.Add(name!, new(name!, startingItems!, operation!));
            }
        }
    
        // Then, set the test function for each monkey
        foreach (string line in lines)
        {
            if (line.StartsWith("Monkey "))
            {
                Match match = Regex.Match(line, @"Monkey (\w+):");
                name = match.Groups[1].Value;
            } else if (line.Contains("Test: divisible by "))
            {
                divisor = int.Parse(line.Split("Test: divisible by ")[1]);
            } else if (line.Contains("If true: throw to monkey "))
            {
                monkeyTrueName = line.Split("If true: throw to monkey ")[1];
            } else if (line.Contains("If false: throw to monkey "))
            {
                monkeyFalseName = line.Split("If false: throw to monkey ")[1];
            } else if (line == "")
            {
                // Capture values into local variables to avoid losing values with references
                string capturedTrueName = monkeyTrueName!;
                string capturedFalseName = monkeyFalseName!;
                int capturedDivisor = (int) divisor!;

                monkeys[name!].SetTest(item => item%capturedDivisor == 0 ? monkeys[capturedTrueName] : monkeys[capturedFalseName], capturedDivisor);
            }
        }

        // Finally, calc the LCM then set it for all monkeys (all divisors are prime numbers anyways)
        int lcm = 1;
        monkeys.Values.ToList().ForEach(m => lcm *= m.divisor);
        monkeys.Values.ToList().ForEach(m => m.SetLCM(lcm));
    }

    private void PlayRound()
    {
        foreach (string monkeyName in monkeys.Keys.Order())
        {
            Monkey monkey = monkeys[monkeyName];
            monkey.TakeTurn();
        }
    }

    public object Part1()
    {
        // Set DivideByThree rule for each monkey
        monkeys.Values.ToList().ForEach(m => m.SetDivideByThree(true));

        for (int round = 0; round < 20; round++)
        {
            PlayRound();
        }

        int[] activities = [.. monkeys.Values.Select(m => m.GetActivity()).Order().Reverse()];

        return activities[0] * activities[1];
    }

    public object Part2()
    {
        // Disable DivideByThree rule for each monkey
        monkeys.Values.ToList().ForEach(m => m.SetDivideByThree(false));

        for (int round = 0; round < 10000; round++)
        {
            PlayRound();
        }

        int[] activities = [.. monkeys.Values.Select(m => m.GetActivity()).Order().Reverse()];

        return (BigInteger) activities[0] * (BigInteger) activities[1]; // 32390280728 too high
    }
}
