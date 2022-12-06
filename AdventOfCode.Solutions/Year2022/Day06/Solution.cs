namespace AdventOfCode.Solutions.Year2022.Day06;

class Solution : SolutionBase
{
    public Solution() : base(06, 2022, "Tuning Trouble", false) { }

    private static int FindGroupOfDistinctCharacters(string input, int distinctCharacterCount)
    {
        for (int i = 0; i < input.Length - distinctCharacterCount; i++)
        {
            if (input.Substring(i, distinctCharacterCount).Distinct().Count() == distinctCharacterCount)
            {
                return i + distinctCharacterCount;
            }
        }
        return -1;
    }
    protected override string SolvePartOne()
    {
        return FindGroupOfDistinctCharacters(Input, 4).ToString();
    }

    protected override string SolvePartTwo()
    {
        return FindGroupOfDistinctCharacters(Input, 14).ToString();
    }
}
