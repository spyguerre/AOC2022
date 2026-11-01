using AOC2022.Common;

namespace AOC2022.Day03;


public class Day03(bool useTestInput = false) : IPuzzle
{
    // List of priorities in each compartment of each rucksack
    private int[][][] rucksacks = [.. ((int[][]) [.. InputReader.ReadLines(3, useTestInput).Select(line => (int[]) [.. line.ToCharArray().Select(c => c - 97 < 0 ? c - 65 + 26 + 1 : c - 97 + 1)])]).Select(rucksack => (int[][]) [rucksack[..(rucksack.Length/2)], rucksack[(rucksack.Length/2)..]])];


    public int Part1()
    {
        int totalPriorities = 0;

        foreach (int[][] rucksack in rucksacks)
        {
            // Make a Set of the first compartment
            var fstCompartment = new HashSet<int>(rucksack[0]);
            
            // Then iterate over the second compartment to check which one is also in the first
            foreach (int priority in rucksack[1])
            {
                if (fstCompartment.Contains(priority)) {
                    totalPriorities += priority;
                    break;
                }
            }
        }

        return totalPriorities;
    }

    public int Part2()
    {
        int totalPriorities = 0;

        for (int group = 0; group < rucksacks.Length/3; group++)
        {
            // Make sets for rucksacks of the first two elves of each group
            var fstElf = new HashSet<int>(rucksacks[3*group][0].Concat(rucksacks[3*group][1]));
            var sndElf = new HashSet<int>(rucksacks[3*group+1][0].Concat(rucksacks[3*group+1][1]));

            // Then iterate over the third elve's rucksack to check which priority is the badge
            foreach (int priority in rucksacks[3*group+2][0].Concat(rucksacks[3*group+2][1]))
            {
                if (fstElf.Contains(priority) && sndElf.Contains(priority))
                {
                    totalPriorities += priority;
                    break;
                }
            }
        }

        return totalPriorities;
    }
}
