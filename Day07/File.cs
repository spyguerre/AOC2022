using System.Data;

namespace AOC2022.Day07;


public class File(string name, File? parent, bool isFolder)
{
    // The file size
    private int size = 0;
    // The file name
    public readonly string name = name;
    // Whether the object is a file or a folder
    public readonly bool isFolder = isFolder;
    // The parent folder. Stays null for the root directory.
    public readonly File? parent = parent;
    // If the file is a folder, contains the list of children files
    private readonly Dictionary<string, File> children = [];

    public int getSize()
    {
        return size;
    }

    public void AddSize(int s)
    {
        size += s;

        parent?.AddSize(s);
    }

    public Dictionary<string, File> GetChildren()
    {
        return children;
    }

    public void AddChild(File file)
    {
        if (!isFolder)
        {
            throw new DataException("Tried to add a child to a file that is not a folder.");
        }
        children.Add(file.name, file);
        AddSize(file.size);
    }
}
