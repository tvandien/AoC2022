namespace AdventOfCode.Solutions.Year2022.Day10;

class Solution : SolutionBase
{
    public Solution() : base(10, 2022, "Cathode-Ray Tube", false) { }

    public class Tubes
    {
        public List<long> RegisterX = new();
        public Tubes() 
        {
            RegisterX.Add(1);
        }

        public void ProcessInput(string[] input)
        {
            foreach (var line in input)
            {
                switch (line[0..4])
                {
                    case "noop":
                        RegisterX.Add(RegisterX.Last());
                        break;
                    case "addx":
                        RegisterX.Add(RegisterX.Last());
                        RegisterX.Add(RegisterX.Last() + GetIncrementFromAddXLine(line));
                        break;
                    default:
                        throw new Exception($"Unknown command: {line}");
                }
            }
        }

        private static long GetIncrementFromAddXLine(string input)
        {
            return long.Parse(input.Split(" ")[1]);
        }

        public long GetSignalStrengthAtCycle(int cycle)
        {
            return RegisterX[cycle - 1] * cycle;
        }

        public long GetSumOfSignalStrengths()
        {
            long result = 0;

            for (int c = 20; c <= 220; c+=40)
            {
                result += GetSignalStrengthAtCycle(c);
            }

            return result;
        }

        public string ComputeImage()
        {
            string output = "\r\n";
            
            for (int row = 0; row < 6; row++)
            {
                for (int column = 0; column < 40; column++)
                {
                    if (Math.Abs(RegisterX[row * 40 + column] - column) <= 1)
                    {
                        output += "#";
                    }
                    else
                    {
                        output += " ";
                    }
                }
                output += "\r\n";
            }

            return output;
        }
    }

    protected override string SolvePartOne()
    {
        var tubes = new Tubes();
        tubes.ProcessInput(Input.SplitByNewline());
        return tubes.GetSumOfSignalStrengths().ToString();
    }

    protected override string SolvePartTwo()
    {
        var tubes = new Tubes();
        tubes.ProcessInput(Input.SplitByNewline());
        return tubes.ComputeImage();
    }
}
