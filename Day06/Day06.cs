using AOC2022.Common;

namespace AOC2022.Day06;


public class Day06(bool useTestInput = false) : IPuzzle
{
    private char[] datastream = InputReader.ReadChars(6, useTestInput)[0];

    public object Part1()
    {
        var queue = new Queue<char>();
        int i = 0;

        while (i < datastream.Length)
        {
            if (queue.Count >= 4)
            {
                queue.Dequeue();
            }
            queue.Enqueue(datastream[i]);

            // Check whether the queue contains 4 different elements
            if (queue.Count == 4 && new HashSet<char>(queue).Count == 4)
            {
                break;
            }

            i++;
        }

        return i+1;
    }

    public object Part2()
    {
        var queue = new Queue<char>();
        int i = 0;

        while (i < datastream.Length)
        {
            if (queue.Count >= 14)
            {
                queue.Dequeue();
            }
            queue.Enqueue(datastream[i]);

            // Check whether the queue contains 4 different elements
            if (queue.Count == 14 && new HashSet<char>(queue).Count == 14)
            {
                break;
            }

            i++;
        }

        return i+1;
    }
}
