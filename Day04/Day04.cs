using System.Text.RegularExpressions;
using AOC2022.Common;

namespace AOC2022.Day04;


public class Day04 : IPuzzle
{
    private readonly int[][][] pairs;

    public Day04(bool useTestInput = false)
    {
        List<int[][]> pairsList = [];

        foreach (string line in InputReader.ReadLines(4, useTestInput))
        {
            var match = Regex.Match(line, @"(\d+)-(\d+),(\d+)-(\d+)");
            int[][] pair = [
                [int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)],
                [int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)]
            ];
            pairsList.Add(pair);
        }

        pairs = [.. pairsList];
    }

    public int Part1()
    {
        int count = 0;

        foreach (int[][] pair in pairs)
        {
            if (
                pair[0][0] <= pair[1][0] && pair[1][1] <= pair[0][1] // First range fully contains the second
             || pair[1][0] <= pair[0][0] && pair[0][1] <= pair[1][1] // Second range fully contains the first
            )
            {
                count++;
            }
        }

        return count;
    }

    public int Part2()
    {
        int count = 0;

        foreach (int[][] pair in pairs)
        {
            if (
                pair[0][0] <= pair[1][0] && pair[1][0] <= pair[0][1] // First range contains the first value of the second range
             || pair[0][0] <= pair[1][1] && pair[1][1] <= pair[0][1] // First range contains the second value of the second range
             || pair[1][0] <= pair[0][0] && pair[0][0] <= pair[1][1] // Second range contains the first value of the second range
            )
            {
                count++;
            }
        }

        return count;
    }
}
