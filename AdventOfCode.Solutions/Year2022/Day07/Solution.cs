namespace AdventOfCode.Solutions.Year2022.Day07;

class Solution : SolutionBase
{
    public Solution() : base(07, 2022, "No Space Left On Device", false) { }

    public class File
    {
        public string Name { get; set; }
        public long Size { get; set; }

        public File(string name, long size)
        {
            this.Name = name;
            this.Size = size;
        }

        public override string ToString()
        {
            return $"{Name} (file, size={Size})";
        }
    }

    public class Directory
    {
        public string Name { get; set; }

        public Directory? Parent { get; set; }
        public List<Directory> Subdirectories { get; set; } = new();
        public List<File> Files { get; set; } = new();

        public Directory(string name, Directory parent) 
        { 
            this.Name = name; 
            this.Parent = parent;
        }

        public void ParseContent(string[] contents)
        {

            for (int i = 1; i < contents.Length; i++)
            {
                if (contents[i].StartsWith("$ ls"))
                {
                    continue;
                }

                if (contents[i].StartsWith("$ cd .."))
                {
                    Parent?.ParseContent(contents[i..]);
                    break;
                }

                if (contents[i].StartsWith("$ cd"))
                {
                    Subdirectories
                        .First(a => a.Name == ParseCdLine(contents[i]))
                        .ParseContent(contents[i..]);
                    break;
                }

                if (contents[i].StartsWith("dir"))
                {
                    Subdirectories.Add(new Directory(contents[i][4..], this));
                    continue;
                }

                var line = contents[i].Split(" ");
                Files.Add(new File(line[1], long.Parse(line[0])));
            }

        }

        public long Size()
        {
            return Subdirectories.Sum(a => a.Size()) + Files.Sum(a => a.Size);
        }

        public override string ToString()
        {
            return $"{Name} (dir, size={Size()})";
        }
    }

    private static string ParseCdLine(string line)
    {
        if (!line.StartsWith("$ cd ")) throw new Exception($"Unexpected cd line: {line}");

        return line.Substring(5);
    }

    protected override string SolvePartOne()
    {
        var root = new Directory("/", null);
        root.ParseContent(Input.SplitByNewline());

        var flattened = root.Subdirectories;
        long limit = 100000;
        long totalSize = flattened.Where(a => a.Size() < limit).Sum(a => a.Size());

        while (flattened.Any(a => a.Subdirectories.Any()))
        {
            flattened = flattened.SelectMany(a => a.Subdirectories).ToList();
            totalSize += flattened.Where(a => a.Size() < limit).Sum(a => a.Size());
        }

        return totalSize.ToString();
    }

    protected override string SolvePartTwo()
    {
        var root = new Directory("/", null);
        root.ParseContent(Input.SplitByNewline());

        var flattened = root.Subdirectories;
        long limit = 30000000 - (70000000 - root.Size());

        List<long> deletionCandidates = flattened
            .Where(a => a.Size() > limit)
            .Select(a => a.Size()).ToList();

        while (flattened.Any(a => a.Subdirectories.Any()))
        {
            flattened = flattened.SelectMany(a => a.Subdirectories).ToList();
            deletionCandidates.AddRange(
                flattened
                .Where(a => a.Size() > limit)
                .Select(a => a.Size()));
        }

        return deletionCandidates.Min().ToString();
    }
}
