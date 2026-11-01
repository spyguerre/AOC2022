using AOC2022.Common;

namespace AOC2022.Day02;


public class Day02(bool useTestInput = false) : IPuzzle
{
    private string[][] rounds = [.. InputReader.ReadLines(2, useTestInput).Select(line => line.Split(" "))];

    private int getRoundScore(string[] round)
    {
        int shapeScore = round[1] switch
        {
            "A" => 1,
            "B" => 2,
            "C" => 3,
            _ => throw new InvalidDataException($"\"{round[1]}\" is not a valid shape.")
        };

        int outcomeScore = 0;
        if (round[0] == round[1])
        { // Draw
            outcomeScore = 3;
        } else if (round.SequenceEqual(["A", "B"]) || round.SequenceEqual(["B", "C"]) || round.SequenceEqual(["C", "A"]))
        { // Win
            outcomeScore = 6;
        }
        // Else keep 0 for a Loss

        return shapeScore + outcomeScore;
    }

    public int Part1()
    {
        int totalScore = 0;

        foreach (string[] round in rounds)
        {
            round[1] = round[1] switch
            {
                "X" => "A",
                "Y" => "B",
                "Z" => "C",
                _ => throw new InvalidDataException($"\"{round[1]}\" is not a valid play.")
            };
            totalScore += getRoundScore(round);
        }

        return totalScore;
    }

    public int Part2()
    {
        int totalScore = 0;

        foreach (string[] round in rounds)
        {
            round[1] = (round[0], round[1]) switch
            { // Mapping between instructions and shape to play
                ("A", "X") => "C",
                ("B", "X") => "A",
                ("C", "X") => "B",
                (_, "Y") => round[0],
                ("A", "Z") => "B",
                ("B", "Z") => "C",
                ("C", "Z") => "A",
                (_, _) => throw new InvalidDataException($"[{string.Join(", ", round)}] is not a valid round.")
            };

            totalScore += getRoundScore(round);
        }

        return totalScore;
    }
}
