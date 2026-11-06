using AOC2022.Common;

namespace AOC2022.Day12;


public class Day12 : IPuzzle
{
    private readonly char[,] charGrid; // Always contains data from map in input
    private (int height, int? cost, int? origin)[,] grid = new (int height, int? cost, int? origin)[0, 0]; // Can be reset from the original input data using ResetGrid()
    private readonly int n, p;
    private (int i, int j) start, end;
    private (int i, int j)[] lowestTiles = []; // (Part 2 only)
    private readonly (int i, int j)[] dirs = [
        (0, 1),  // Right
        (-1, 0), // Up
        (0, -1), // Left
        (1, 0)   // Down
    ];

    public Day12(bool useTestInput = false)
    {
        charGrid = InputReader.ReadMatrix(12, useTestInput);
        (n, p) = (charGrid.GetLength(0), charGrid.GetLength(1));
        
        // Fill lowestTiles
        List<(int i, int j)> lowestTilesList = [];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < p; j++)
            {
                if (((char[]) ['a', 'S']).Contains(charGrid[i, j]))
                {
                    lowestTilesList.Add((i, j));
                }
            }
        }
        lowestTiles = [.. lowestTilesList];

        ResetGrid();
    }

    private void ResetGrid()
    {
        grid = new (int height, int? cost, int? origin)[n, p];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < p; j++)
            {
                switch (charGrid[i, j])
                {
                    case 'S':
                        grid[i, j].height = 'a';
                        start = (i, j);
                        break;
                    case 'E':
                        grid[i, j].height = 'z';
                        end = (i, j);
                        break;
                    default:
                        grid[i, j].height = charGrid[i, j];
                        break;
                };
                
                grid[i, j].height -= 'a'; // a => 0, z => 25
            }
        }
    }

    // BFS that fills cost and origin from grid field to find the shortest path
    private void Explore()
    {
        Queue<(int i, int j)> queue = new([start]);
        grid[start.i, start.j].cost = 0;


        while (grid[end.i, end.j].origin is null && queue.Count >= 1) // Stop as soon as we find the first (= shortest) path to the end
        {
            var pos = queue.Dequeue();
            var node = grid[pos.i, pos.j];

            for (int d = 0; d < 4; d++)
            {
                var dir = dirs[d];
                (int i, int j) adjPos = (pos.i + dir.i, pos.j + dir.j);
                if (adjPos.i < 0 || adjPos.j < 0 || adjPos.i >= n || adjPos.j >= p) continue;
                var adjNode = grid[adjPos.i, adjPos.j];

                // Constraint to move to adjNode is respected, and either adjNode is not yet explored
                if (adjNode.height <= node.height + 1 && adjNode.cost is null)
                {
                    adjNode.cost = node.cost + 1;
                    adjNode.origin = (d+2) % 4; // Origin is the opposite direction
                    grid[adjPos.i, adjPos.j] = adjNode;
                    queue.Enqueue(adjPos);
                }
            }
        }
    }

    // Returns the shortest path found after exploration. Returns null if exploration didn't allow to reach the end from the start.
    private (int i, int j)[]? GetPath()
    {
        // First ensure that the end has been reached (i.e. that a path exists)
        if (grid[end.i, end.j].cost is null)
        {
            return null;
        }

        List<(int i, int j)> path = [end];

        while (path.Last() != start)
        {
            var lastPos = path.Last();
            var lastNode = grid[lastPos.i, lastPos.j];
            (int i, int j) orgPos = (lastPos.i + dirs[(int) lastNode.origin!].i, lastPos.j + dirs[(int) lastNode.origin!].j);
            path.Add((orgPos.i, orgPos.j));
        }

        path.Reverse();

        return [.. path];
    }

    public object Part1()
    {
        Explore();

        return GetPath()!.Length - 1;
    }

    public object Part2()
    {
        int shortestPathLength = int.MaxValue;
        
        foreach (var s in lowestTiles)
        {
            ResetGrid();
            start = s;
            Explore();

            var shortestPath = GetPath();
            int pathFoundLength = shortestPath is null ? int.MaxValue : shortestPath.Length - 1;
            shortestPathLength = Math.Min(shortestPathLength, pathFoundLength);
        }
        return shortestPathLength;
    }
}
