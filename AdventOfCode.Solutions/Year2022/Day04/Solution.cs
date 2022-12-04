namespace AdventOfCode.Solutions.Year2022.Day04;

class Solution : SolutionBase
{
    public Solution() : base(04, 2022, "Camp Cleanup", false) { }

    public class SectorList
    {
        public List<int> Sectors = new();

        public SectorList(string sectors)
        {
            var startAndEnd = sectors.Split("-").Select(int.Parse).ToList();
            var (start, end) = (startAndEnd[0], startAndEnd[1]);

            for (int i = start; i <= end; i++)
            {
                Sectors.Add(i);
            }
        }
    }

    public static bool SectorListsOverlapAll(SectorList a, SectorList b)
    {
        return 
            a.Sectors.All(b.Sectors.Contains) ||
            b.Sectors.All(a.Sectors.Contains);
    }

    public static bool SectorListsOverlapAny(SectorList a, SectorList b)
    {
        return a.Sectors.Any(b.Sectors.Contains);
    }

    protected override string SolvePartOne()
    {
        return Input
            .SplitByNewline()
            .Select(a => a.Split(",").Select(b => new SectorList(b)).ToList())
            .Count(a => SectorListsOverlapAll(a[0], a[1]))
            .ToString();
    }

    protected override string SolvePartTwo()
    {
        return Input
            .SplitByNewline()
            .Select(a => a.Split(",").Select(b => new SectorList(b)).ToList())
            .Count(a => SectorListsOverlapAny(a[0], a[1]))
            .ToString();
    }
}
