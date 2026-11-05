using System.Numerics;

namespace AOC2022.Day11;


public class Monkey
{
    private readonly string name;
    public readonly Queue<int> items;
    private readonly Func<BigInteger, BigInteger> operation;
    private Func<int, Monkey> test;
    public int divisor;
    private int lcm; // Least common multiple for all monkeys divisors
    private int activity;
    private bool divideByThree;

    public Monkey(string name, int[] startingItems, Func<BigInteger, BigInteger> operation)
    {
        this.name = name;
        items = new([.. startingItems]);
        this.operation = operation;
        test = _ => throw new NotImplementedException($"The test function has not been set for Monkey {this.name}");
        activity = 0;
        divideByThree = true;
        divisor = 0;
    }

    public void SetTest(Func<int, Monkey> test, int divisor)
    {
        this.test = test;
        this.divisor = divisor;
    }

    public void SetLCM(int lcm)
    {
        this.lcm = lcm;
    }

    public void SetDivideByThree(bool divideByThree)
    {
        this.divideByThree = divideByThree;
    }

    private void InspectNextItem()
    {
        // Extract the first item from the monkey's queue
        BigInteger item = items.Dequeue();

        // Apply operation to worry level
        item = operation(item);

        // Item inspected: Increment activity
        activity++;

        if (divideByThree) item /= 3; // Relief divides by 3 (Part 1 only)
        else item %= lcm; // Keep the worry level within integer range after operation, while preserving the remainder for all the divisors (Part 2 only)

        // Throw the item according to the test
        test((int) item).CatchItem((int) item);
    }

    public void TakeTurn()
    {
        while (items.Count >= 1)
        {
            InspectNextItem();
        }
    }

    public void CatchItem(int item)
    {
        items.Enqueue(item);
    }

    public int GetActivity()
    {
        return activity;
    }
}
