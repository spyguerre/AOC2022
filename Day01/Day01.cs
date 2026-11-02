using AOC2022.Common;

namespace AOC2022.Day01;


public class Day01(bool useTestInput = false) : IPuzzle
{
    private readonly int?[] calories = InputReader.ReadIntegers(1, useTestInput);
    private int[] summedCalories = [];


    private void CalcSummedCalories()
    {
        var summedCalList = new List<int>();

        int curSum = 0;
        foreach (int? cal in calories)
        {
            if (cal is null)
            {
                summedCalList.Add(curSum);
                curSum = 0;
            } else
            {
                curSum += (int) cal;
            }
        }

        summedCalories = [.. summedCalList];
    }

    public object Part1()
    {
        CalcSummedCalories();

        return summedCalories.Max();
    }

    public object Part2()
    {
        CalcSummedCalories();

        Array.Sort(summedCalories);
        Array.Reverse(summedCalories);

        return summedCalories[0] + summedCalories[1] + summedCalories[2];
    }
}
