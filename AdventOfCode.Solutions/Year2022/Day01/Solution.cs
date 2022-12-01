namespace AdventOfCode.Solutions.Year2022.Day01;

class Solution : SolutionBase
{
    public Solution() : base(01, 2022, "Calorie Counting", false) { }

    private List<int> ParseInput() => Input
        .SplitByParagraph()
        .Select(singleElf => singleElf.ToIntArray("\n").Sum())
        .ToList();

    protected override string SolvePartOne()
    {
        var caloriesPerElf = ParseInput();

        return caloriesPerElf.Max().ToString();
    }

    protected override string SolvePartTwo()
    {
        List<int> caloriesPerElf = ParseInput();

        caloriesPerElf.Sort();

        return caloriesPerElf.TakeLast(3).Sum().ToString();
    }
}
