namespace AdventOfCode.Solutions.Year2022.Day09;

class Solution : SolutionBase
{
    public Solution() : base(09, 2022, "Rope Bridge", false) { }

    public class Rope
    {
        public List<(int, int)> RopeSegments { get; set; } = new();
        public List<(int, int)> TailLocations { get; set; } = new();

        public Rope(int segments) 
        {
            for (int i = 0; i < segments; i++)
            {
                RopeSegments.Add((0, 0));
            }
        }

        public void Move(string input)
        {
            char direction = input[0];
            int distance = int.Parse(input.Substring(2));

            var (dx, dy) = direction switch
            {
                'R' => (1, 0),
                'L' => (-1, 0),
                'U' => (0, 1),
                'D' => (0, -1),
                _ => throw new Exception($"Unknown direction: {direction}")
            };

            for (int i = 0; i < distance; i++)
            {
                RopeSegments[0] = (RopeSegments[0].Item1 + dx, RopeSegments[0].Item2 + dy);
                TailFollow();
            }

        }

        private void TailFollow()
        {
            for (int i = 1; i < RopeSegments.Count; i++)
            {
                int segmentX = RopeSegments[i].Item1;
                int segmentY = RopeSegments[i].Item2;
                int dx = RopeSegments[i - 1].Item1 - segmentX;
                int dy = RopeSegments[i - 1].Item2 - segmentY;

                if (dx != 0 && (Math.Abs(dx) > 1 || Math.Abs(dy) > 1))
                {
                    RopeSegments[i] = (segmentX + (dx / Math.Abs(dx)), RopeSegments[i].Item2);
                }

                if (dy != 0 && (Math.Abs(dy) > 1 || Math.Abs(dx) > 1))
                {
                    RopeSegments[i] = (RopeSegments[i].Item1, segmentY + (dy / Math.Abs(dy)));
                }

            }

            if (!TailLocations.Contains((RopeSegments.Last().Item1, RopeSegments.Last().Item2)))
            {
                TailLocations.Add((RopeSegments.Last().Item1, RopeSegments.Last().Item2));
            }

        }
    }

    protected override string SolvePartOne()
    {
        var rope = new Rope(2);
        var steps = Input.SplitByNewline();

        foreach(var step in steps)
        {
            rope.Move(step);
        }

        return rope.TailLocations.Count.ToString();
    }

    protected override string SolvePartTwo()
    {
        var rope = new Rope(10);
        var steps = Input.SplitByNewline();

        foreach (var step in steps)
        {
            rope.Move(step);
        }

        return rope.TailLocations.Count.ToString();
    }
}
