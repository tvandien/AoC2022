namespace AdventOfCode.Solutions.Year2022.Day11;

class Solution : SolutionBase
{
    public Solution() : base(11, 2022, "Monkey in the Middle", false) { }

    public class Operation
    {
        public string Left { get; set; }
        public string Right { get; set; }
        public string Operator { get; set; }

        public Operation(string input) 
        {
            var parts = input.Trim().Split(" ");
            Left = parts[3];
            Operator = parts[4];
            Right = parts[5];
        }

        public long Solution(long old, long divisionFactor = 1)
        {
            long left = Left switch
            {
                "old" => old,
                _ => long.Parse(Left)
            };

            long right = Right switch
            {
                "old" => old,
                _ => long.Parse(Right)
            };

            return Operator switch
            {
                "+" => (left + right) / divisionFactor,
                "*" => (left * right) / divisionFactor,
                _ => throw new Exception($"Unknown operator {Operator}")
            };
        }

        public override string ToString()
        {
            return $"new = {Left} {Operator} {Right}";
        }
    }

    public class Test
    {
        public long Divider { get; set; }
        public int MonkeyIfTrue { get; set; }
        public int MonkeyIfFalse { get; set; }

        public Test(string[] input)
        {
            Divider = long.Parse(input[0].Split(" ").Last());
            MonkeyIfTrue = int.Parse(input[1].Split(" ").Last());
            MonkeyIfFalse = int.Parse(input[2].Split(" ").Last());
        }

        public override string ToString()
        {
            return $"Divide by {Divider}. If true, throw to monkey {MonkeyIfTrue}. If false, throw to monkey {MonkeyIfFalse}";
        }

        public Monkey GetMonkey(long item, List<Monkey> monkeys)
        {
            int NextMonkeyID = (item % Divider == 0) ? MonkeyIfTrue : MonkeyIfFalse;

            return monkeys.First(a => a.ID == NextMonkeyID);
        }
    }

    public class Monkey
    {
        public int ID { get; set; }
        public List<long> Items { get; set; }
        public Operation Operation { get; set; }
        public Test Test { get; set; }
        public long Inspections { get; set; } = 0;
        
        private List<long> ParseItems(string input)
        {
            return input
                .Replace(",", "")
                .Trim()
                .Split(" ")[2..]
                .Select(long.Parse)
                .ToList();
        }

        public Monkey(string[] input) 
        {
            ID = int.Parse(input[0][7..(input[0].Length - 1)]);
            Items = ParseItems(input[1]);
            Operation = new Operation(input[2]);
            Test = new Test(input[3..]);
        }

        public override string ToString()
        {
            return $"Monkey {ID}, " +
                $"Inspections: {Inspections}, " +
                $"Items: {String.Join(", ", Items)}, " +
                $"Operation: {Operation}, " +
                $"Test: {Test}";
        }

        public void DoSomeMonkeyBusiness(List<Monkey> monkeys, long divider = 0, long divisionFactor = 1)
        {
            while(Items.Count > 0)
            {
                var item = Operation.Solution(Items.First(), divisionFactor);

                if (divider != 0)
                {
                    item %= divider;
                }

                var monkey = Test.GetMonkey(item, monkeys);
                monkey.Items.Add(item);
                Items.RemoveAt(0);
                Inspections++;
            }
        }

    }

    protected override string SolvePartOne()
    {
        var monkeys = Input
            .SplitByParagraph()
            .Select(a => new Monkey(a.SplitByNewline()))
            .ToList();

        for (int i = 0; i < 20; i++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.DoSomeMonkeyBusiness(monkeys, divisionFactor: 3);
            }
        }

        var inspectionTimes = monkeys.Select(a => a.Inspections).OrderDescending().ToList();

        return (inspectionTimes[0] * inspectionTimes[1]).ToString();
    }

    protected override string SolvePartTwo()
    {
        var monkeys = Input
            .SplitByParagraph()
            .Select(a => new Monkey(a.SplitByNewline()))
            .ToList();

        var productOfDividers = monkeys
            .Select(a => a.Test.Divider)
            .Aggregate(1L, (a, b) => a * b);

        for (int i = 0; i < 10000; i++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.DoSomeMonkeyBusiness(monkeys, productOfDividers);
            }
        }

        var inspectionTimes = monkeys.Select(a => a.Inspections).OrderDescending().ToList();

        return (inspectionTimes[0] * inspectionTimes[1]).ToString();
    }
}
