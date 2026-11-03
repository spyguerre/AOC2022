using AOC2022.Common;

namespace AOC2022.Day08;


public class Day08 : IPuzzle
{
    private Tree[,] forest;
    private int n, p;

    public Day08(bool useTestInput = false)
    {
        int[,] heights = InputReader.ReadIntegerMatrix(8, useTestInput);
        n = heights.GetLength(0);
        p = heights.GetLength(1);
        forest = new Tree[n, p];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < p; j++)
            {
                forest[i, j] = new Tree(heights[i, j]);
            }
        }
    }

    private void MarkVisibleTrees()
    {
        // Mark trees that are visible horizontally
        for (int i = 0; i < n; i++)
        {
            // From left to right
            int maxHeight = -1;
            
            for (int j = 0; j < p; j++)
            {
                Tree curTree = forest[i, j];

                if (curTree.height > maxHeight)
                {
                    curTree.visible = true;
                    maxHeight = curTree.height;
                }
            }

            // From right to left
            maxHeight = -1;
            
            for (int j = p-1; j >= 0; j--)
            {
                Tree curTree = forest[i, j];

                if (curTree.height > maxHeight)
                {
                    curTree.visible = true;
                    maxHeight = curTree.height;
                }
            }
        }

        // Mark trees that are visible vertically
        for (int j = 0; j < p; j++)
        {
            // From top to bottom
            int maxHeight = -1;
            
            for (int i = 0; i < n; i++)
            {
                Tree curTree = forest[i, j];

                if (curTree.height > maxHeight)
                {
                    curTree.visible = true;
                    maxHeight = curTree.height;
                }
            }

            // From bottom to top
            maxHeight = -1;
            
            for (int i = n-1; i >= 0; i--)
            {
                Tree curTree = forest[i, j];

                if (curTree.height > maxHeight)
                {
                    curTree.visible = true;
                    maxHeight = curTree.height;
                }
            }
        }
    }

    private int GetScenicScore(int i0, int j0)
    {
        int[] viewingDistances = [0, 0, 0, 0]; // Right, up, left, down
        int houseHeight = forest[i0, j0].height;

        // Look right
        for (int j = j0+1; j < p; j++)
        {
            viewingDistances[0]++;

            if (forest[i0, j].height >= houseHeight) break;
        }

        // Look up
        for (int i = i0-1; i >= 0; i--)
        {
            viewingDistances[1]++;

            if (forest[i, j0].height >= houseHeight) break;
        }

        // Look left
        for (int j = j0-1; j >= 0; j--)
        {
            viewingDistances[2]++;

            if (forest[i0, j].height >= houseHeight) break;
        }

        // Look down
        for (int i = i0+1; i < n; i++)
        {
            viewingDistances[3]++;

            if (forest[i, j0].height >= houseHeight) break;
        }

        return viewingDistances[0] * viewingDistances[1] * viewingDistances[2] * viewingDistances[3];
    }

    private void CalcAllScenicScores()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < p; j++)
            {
                forest[i, j].scenicScore = GetScenicScore(i, j);
            }
        }
    }

    public object Part1()
    {
        MarkVisibleTrees();
        
        return forest.Cast<Tree>().Sum(t => t.visible ? 1 : 0);
    }

    public object Part2()
    {
        CalcAllScenicScores();

        return forest.Cast<Tree>().Max(t => t.scenicScore)!;
    }
}
