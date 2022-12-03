namespace AdventOfCode.Solutions.Year2022.Day03;

class Solution : SolutionBase
{
    public Solution() : base(03, 2022, "Rucksack Reorganization", false) { }

    public class ElfGroup
    {
        public string[] Rucksacks { get; set; }

        public ElfGroup(string[] rucksacks) 
        {
            Rucksacks = rucksacks;
        }

        public char FindOverlappingItem()
        {
            return Rucksacks[0]
                .First(a => Rucksacks[1].Contains(a) && Rucksacks[2].Contains(a));
        }
    }

    public class Rucksack
    {
        public string CompartmentOne { get; set; }
        public string CompartmentTwo { get; set; }

        public Rucksack(string contents)
        {
            CompartmentOne = contents[..(contents.Length / 2)];
            CompartmentTwo = contents[(contents.Length / 2)..];
        }

        public char FindOverlappingItem()
        {
            return CompartmentOne.First(a => CompartmentTwo.Contains(a));
        }
    }

    public static int ItemValue(char item)
    {
        return item switch
        {
            >= 'A' and <= 'Z' => item - 'A' + 27,
            >= 'a' and <= 'z' => item - 'a' + 1,
            _ => throw new Exception($"Invalid item: {item}")
        };
    }

    protected override string SolvePartOne()
    {
        return Input
            .SplitByNewline()
            .Select(a => new Rucksack(a))
            .Select(a => a.FindOverlappingItem())
            .Select(a => ItemValue(a))
            .Sum()
            .ToString();

    }

    protected override string SolvePartTwo()
    {
        return Input
            .SplitByNewline()
            .Chunk(3)
            .Select(a => new ElfGroup(a))
            .Select(a => a.FindOverlappingItem())
            .Select(a => ItemValue(a))
            .Sum()
            .ToString();
    }
}
