using System.Text.RegularExpressions;
using AOC2022.Common;

namespace AOC2022.Day05;


public class Day05 : IPuzzle
{
    private readonly Stack<char>[] crates;
    private readonly int[][] instructions;

    public Day05(bool useTestInput = false)
    {
        bool inInstructions = false;
        List<int[]> instructionsList = [];
        List<List<char>> crateLists = [];

        foreach (string line in InputReader.ReadLines(5, useTestInput))
        {
            if (line.Equals(""))
            {
                inInstructions = true;
                continue;
            }

            if (inInstructions)
            {
                Match match = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)");
                instructionsList.Add([int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value)]);
            } else
            {
                char[] crateChars = [.. line.Where((_, i) => i % 4 == 1)];
                
                // If necessary, initialize the right number of Lists
                if (crateLists.Count < crateChars.Length)
                {
                    crateLists.AddRange([.. Enumerable.Range(0, crateChars.Length - crateLists.Count).Select(_ => new List<char>())]);
                }

                // For each match, add the corresponding crate to its stack
                int stackNumber = 0;
                foreach (char c in crateChars)
                {
                    if (c != ' ')
                    {
                        crateLists[stackNumber].Insert(0, c);
                    }
                    stackNumber++;
                }
            }
        }

        instructions = [.. instructionsList];
        crates = [.. crateLists.Select(s => new Stack<char>(s))];
    }


    public object Part1()
    {
        foreach (int[] instruction in instructions)
        {
            for (int k = 0; k < instruction[0]; k++)
            {
                crates[instruction[2]-1].Push(crates[instruction[1]-1].Pop());
            }
        }
        return string.Join("", crates.Select(s => s.Peek()));
    }

    public object Part2()
    {
        var intermediateStack = new Stack<char>();

        foreach (int[] instruction in instructions)
        {
            // Move crates one by one to the intermediate stack (which end up in reversed order)
            for (int k = 0; k < instruction[0]; k++)
            {
                intermediateStack.Push(crates[instruction[1]-1].Pop());
            }

            // Then move them one by one from the intermediate stack to the new stack (which end up in the original order)
            while (intermediateStack.Count > 0)
            {
                crates[instruction[2]-1].Push(intermediateStack.Pop());
            }
        }
        return string.Join("", crates.Select(s => s.Peek()));
    }
}
