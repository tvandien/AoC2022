using static AdventOfCode.Solutions.Year2022.Day15.Solution;

namespace AdventOfCode.Solutions.Year2022.Day15;

class Solution : SolutionBase
{
    public Solution() : base(15, 2022, "Beacon Exclusion Zone", false) { }

    public class Sensor
    {
        public int SensorX { get; set; }
        public int SensorY { get; set; }

        public int BeaconX { get; set; }
        public int BeaconY { get; set; }

        private static int WordToInt(string word)
        {
            var parts = word
                .Replace(",", "")
                .Replace(":", "")
                .Split("=");

            return int.Parse(parts[1]);
        }

        public Sensor(string input) 
        {
            var words = input.Split(" ");
            SensorX = WordToInt(words[2]);
            SensorY = WordToInt(words[3]);
            BeaconX = WordToInt(words[8]);
            BeaconY = WordToInt(words[9]);
        }

        public int GetManhattanDistance()
        {
            return Math.Abs(SensorX - BeaconX) + Math.Abs(SensorY - BeaconY);
        }

    }

    public static int GetEffectOnLine(List<Sensor> sensors, int y)
    {
        var ranges = new List<(int, int)>();
        foreach (var sensor in sensors)
        {
            var distance = sensor.GetManhattanDistance();
            var distanceToLine = Math.Abs(y - sensor.SensorY);

            if (distanceToLine > distance)
            {
                continue;
            }

            var freedomInLine = distance - distanceToLine;

            ranges.Add((sensor.SensorX - freedomInLine, sensor.SensorX + freedomInLine));
        }

        int min = ranges.Min(a => a.Item1);
        int max = ranges.Max(a => a.Item2);

        int count = 0;
        for (int i = min; i <= max; i++)
        {
            if (sensors.Any(a => a.BeaconY == y && a.BeaconX == i))
            {
                continue;
            }

            if (ranges.Any(a => a.Item1 <= i && i <= a.Item2))
            {
                var inRange = ranges.First(a => a.Item1 <= i && i <= a.Item2);
                count++;
            }
        }

        return count;
    }

    public static long GetTuningFrequency(List<Sensor> sensors, int y)
    {
        var ranges = new List<(int, int)>();
        foreach (var sensor in sensors)
        {
            var distance = sensor.GetManhattanDistance();
            var distanceToLine = Math.Abs(y - sensor.SensorY);

            if (distanceToLine > distance)
            {
                continue;
            }

            var freedomInLine = distance - distanceToLine;

            ranges.Add((sensor.SensorX - freedomInLine, sensor.SensorX + freedomInLine));
        }

        int min = Math.Max(0, ranges.Min(a => a.Item1));
        int max = Math.Min(4_000_000, ranges.Max(a => a.Item2));

        for (int i = min; i <= max; i++)
        {
            if (ranges.Any(a => a.Item1 <= i && i <= a.Item2))
            {
                var inRange = ranges.First(a => a.Item1 <= i && i <= a.Item2);
                i = inRange.Item2;
                continue;
            }
            
            if (sensors.Any(a => a.BeaconY == y && a.BeaconX == i))
            {
                continue;
            }
            return i * 4_000_000L + y;
        }

        return -1;
    }

    protected override string SolvePartOne()
    {
        var sensorReadings = Input
            .SplitByNewline()
            .Select(a => new Sensor(a))
            .ToList();

        var count = GetEffectOnLine(sensorReadings, 2000000);
        return count.ToString();
    }

    protected override string SolvePartTwo()
    {
        var sensorReadings = Input
            .SplitByNewline()
            .Select(a => new Sensor(a))
            .ToList();

        for (int y = 0; y < 4_000_000; y++)
        {
            var result = GetTuningFrequency(sensorReadings, y);
            if (result != -1)
            {
                return result.ToString();
            }

        }
        return "???";
    }
}
