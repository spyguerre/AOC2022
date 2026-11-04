using AOC2022.Common;

namespace AOC2022.Day09;


public class Day09(bool useTestInput = false) : IPuzzle
{
    // List of instructions in input
    private (char dirChar, int motionLength)[] instructions = [.. InputReader.ReadLines(9, useTestInput).Select(line => (char.Parse(line.Split(" ")[0]), int.Parse(line.Split(" ")[1])))];
    // List of unique tiles visited by the tail
    private HashSet<(int i, int j)> visited = [];

    // Current tile of the head and tail (Part 1 only)
    private (int i, int j) headPos = new(0, 0);
    private (int i, int j) tailPos = new(0, 0);

    // All knots in the rope (including head and tail, Part 1 only)
    private (int i, int j)[] knots = [.. Enumerable.Repeat((0, 0), 10)];

    private static Dictionary<char, (int i, int j)> dirMap = new() {
        {'R', new(0, 1)},
        {'U', new(-1, 0)},
        {'L', new(0, -1)},
        {'D', new(1, 0)}
    };

    public object Part1()
    {
        foreach ((char dirChar, int motionLength) instruction in instructions)
        {
            (int i, int j) dir = dirMap[instruction.dirChar];

            for (int k = 0; k < instruction.motionLength; k++)
            {
                // Move the tail to the current head position if needed
                // Explaination: The next tail position will always be the current head position, if the tail needs to move (dist to head >= 2 tiles in any direction)
                if (Math.Abs(headPos.i + dir.i - tailPos.i) >= 2 || Math.Abs(headPos.j + dir.j - tailPos.j) >= 2)
                {
                    tailPos.i = headPos.i;
                    tailPos.j = headPos.j;
                }
                
                // Move the head
                headPos.i += dir.i;
                headPos.j += dir.j;

                visited.Add(tailPos);
            }
        }

        return visited.Count;
    }

    public object Part2()
    {
        // Iterate over instructions
        foreach ((char dirChar, int motionLength) instruction in instructions)
        {
            (int i, int j) headDir = dirMap[instruction.dirChar];

            // Iterate over the number of moves in that instructions
            for (int k = 0; k < instruction.motionLength; k++)
            {
                // Move the head first and independantly
                knots[0].i += headDir.i;
                knots[0].j += headDir.j;

                // Iterate over the knots to move them one by one
                for (int l = 1; l < knots.Length; l++)
                {
                    // At this point, assume all previous knots in the rope have already been moved
                    // The current knot will move at most one tile in each direction
                    // Let's compute the vector for this knot as below: In both axes:
                    // As long as the previous knot is at least 2 tiles away in any direction, move the knot towards the previous knot (by one tile if needed, else 0)
                    (int i, int j) curKnotDir = (
                        Math.Abs(knots[l-1].i - knots[l].i) >= 2 || Math.Abs(knots[l-1].j - knots[l].j) >= 2 ? Math.Sign(knots[l-1].i - knots[l].i) : 0,
                        Math.Abs(knots[l-1].i - knots[l].i) >= 2 || Math.Abs(knots[l-1].j - knots[l].j) >= 2 ? Math.Sign(knots[l-1].j - knots[l].j) : 0
                    );

                    // Move the current knot according to the vector
                    knots[l].i += curKnotDir.i;
                    knots[l].j += curKnotDir.j;
                }

                visited.Add(knots.Last());
            }
        }

        return visited.Count;
    }
}
