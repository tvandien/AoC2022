namespace AdventOfCode.Solutions.Year2022.Day05;

class Solution : SolutionBase
{
    public Solution() : base(05, 2022, "Supply Stacks", false) { }

    public class CargoStack
    {
        public List<List<char>> stacks = new();
        public string Commands { get; set; }

        private void ParseState(string state)
        {
            var lines = state.SplitByNewline();
            foreach (var line in lines)
            {
                if (line.StartsWith(" 1 ")) continue;

                for (int i = 1; i < line.Length; i+=4)
                {
                    int stackId = (i - 1) / 4;
                    if (stacks.Count == stackId) stacks.Add(new());

                    if (line[i] == ' ') continue;

                    stacks[stackId].Add(line[i]);
                }
            }
        }

        public CargoStack(string input)
        {
            var paragraphs = input.SplitByParagraph();
            var (state, commands) = (paragraphs[0], paragraphs[1]);

            ParseState(state);
            Commands = commands;
        }

        private void ExecuteCommand(bool reversePickup, int count, int from, int to)
        {
            var pickup = stacks[from - 1].Take(count);

            if (reversePickup)
            {
                pickup = pickup.Reverse();
            }

            stacks[to - 1].InsertRange(0, pickup);
            stacks[from - 1].RemoveRange(0, count);
        }

        public void ExecuteCommands(bool reversePickup = true)
        {
            foreach (var command in Commands.SplitByNewline())
            {
                var commandParts = command.Split(" ");
                var (count, from, to) =
                    (int.Parse(commandParts[1]), 
                    int.Parse(commandParts[3]), 
                    int.Parse(commandParts[5]));

                ExecuteCommand(reversePickup, count, from, to);
            }
        }

        public string GetSecretMessage()
        {
            return new String(stacks.Select(a => a.First()).ToArray());
        }
    }

    protected override string SolvePartOne()
    {
        var stacks = new CargoStack(Input);
        stacks.ExecuteCommands();
        return stacks.GetSecretMessage();
    }

    protected override string SolvePartTwo()
    {
        var stacks = new CargoStack(Input);
        stacks.ExecuteCommands(false);
        return stacks.GetSecretMessage();
    }
}
