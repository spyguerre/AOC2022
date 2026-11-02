using AOC2022.Common;

namespace AOC2022.Day07;


public class Day07 : IPuzzle
{
    // Input
    private readonly string[] log;
    // Root directory
    private readonly File fileTree;
    // Current working directory
    private File wd;

    public Day07(bool useTestInput = false)
    {
        log = InputReader.ReadLines(7, useTestInput);
        fileTree = new(name: "/", parent: null, isFolder: true);
        wd = fileTree;
    }

    private void CD(string newDir)
    {
        if (newDir == "/")
        {
            wd = fileTree;
        } else if (newDir == "..")
        {
            wd = wd.parent!;
        } else
        {
            wd = wd.GetChildren()[newDir];
        }
    }

    private void LS(string line) // Called whenever a line isn't a command
    {
        if (line.StartsWith("dir ")) // New directory
        {
            string dirName = line.Split("dir ")[1];
            wd.AddChild(new File(name: dirName, parent: wd, isFolder: true));
        } else // New file
        {
            int fileSize = int.Parse(line.Split(" ")[0]);
            string fileName = line.Split(" ")[1];
            File newFile = new(name: fileName, parent: wd, isFolder: false);
            wd.AddChild(newFile);
            newFile.AddSize(fileSize);
        }
    }

    private void BuildFileSystem()
    {
        foreach (string line in log)
        {
            if (line.StartsWith("$ cd "))
            {
                CD(line.Split("$ cd ")[1]);
            } else if (!line.StartsWith("$ "))
            {
                LS(line);
            }
        }
    }

    // BFS from the root directory, returning the list of files/folders corresponding to the input filter
    private List<File> FilterFiles(Func<File, bool> filter)
    {
        var queue = new Queue<File>([fileTree]);
        List<File> res = [];

        while (queue.Count > 0)
        {
            File cur = queue.Dequeue();

            if (cur.isFolder)
            {
                cur.GetChildren().Values.ToList().ForEach(queue.Enqueue);
            }

            if (filter(cur))
            {
                res.Add(cur);
            }
        }

        return res;
    }

    private static bool FolderFilter(File file, int minSize = 0, int maxSize = 70_000_000)
    {
        return file.isFolder
            && file.getSize() >= minSize
            && file.getSize() <= maxSize;
    }

    private static bool FolderFilter(File file, int minSize)
    {
        return file.isFolder && file.getSize() >= minSize;
    }

    public object Part1()
    {
        BuildFileSystem();
        return FilterFiles(f => FolderFilter(f, maxSize: 100000)).Sum(f => f.getSize());
    }

    public object Part2()
    {
        BuildFileSystem();
        return FilterFiles(f => FolderFilter(f, minSize: fileTree.getSize() - (70_000_000 - 30_000_000))).Min(f => f.getSize());
    }
}
