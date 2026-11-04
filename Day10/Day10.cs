using AOC2022.Common;

namespace AOC2022.Day10;


public class Day10(bool useTestInput = false) : IPuzzle
{
    private string[] commands = InputReader.ReadLines(10, useTestInput);
    private int x = 1;
    // Remembers the value of X at the end of each cycle (used in Part 1 only)
    private readonly List<int> xValues = [];
    // CRT / Screen (Part 2 only)
    private readonly bool[,] crt = new bool[6, 40]; // true = lit, initialized at false = dark by default
    private int cycle = 0; // (Part 2 only)

    public object Part1()
    {
        foreach (string command in commands)
        {
            if (command == "noop")
            {
                xValues.Add(x);
            } else if (command.StartsWith("addx "))
            {
                int value = int.Parse(command.Split("addx ")[1]);

                xValues.Add(x);
                xValues.Add(x);
                x += value;
            }
        }

        return xValues.Select((xValue, i) => (i+1) % 40 == 20 ? (i+1)*xValue : 0).Sum();
    }

    public object Part2()
    {
        // Using a stack makes it easier to pick commands on the fly
        Stack<string> cmdStack = new(commands.Reverse());
        // The command currently under execution
        string curCmd = cmdStack.Pop();
        // The number of cycles remaining for the current command before completion
        int reqCycles = curCmd.StartsWith("addx ") ? 2 : 1;

        while (cycle < crt.GetLength(0) * crt.GetLength(1))
        {
            // Light the corresponding pixel if the sprite's horizontal value corresponds to pixel being drawn
            crt[cycle/crt.GetLength(1), cycle%crt.GetLength(1)] = Math.Abs(x - cycle%crt.GetLength(1)) <= 1;

            // Current cycle completed
            reqCycles--;
            cycle++;

            // Check if last instruction ended, and apply its effect
            if (reqCycles == 0)
            {
                if (curCmd.StartsWith("addx "))
                {
                    int value = int.Parse(curCmd.Split("addx ")[1]);
                    x += value;
                }
                // Do nothing for "noop"

                // When an instruction ends, try to pick up the next one in the stack
                if (cmdStack.Count >= 1)
                {
                    curCmd = cmdStack.Pop();
                    reqCycles = curCmd.StartsWith("addx ") ? 2 : 1;
                }
            }

        }

        // Return a string with linebreaks for each line in the crt
        return string.Join("", crt.Cast<bool>().Select((px, i) => (i%crt.GetLength(1) == 0 ? "\n" : "") + (px ? "#" : ".")));
    }
}
