using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode.Solutions.Year2022.Day14;

class Solution : SolutionBase
{
    public Solution() : base(14, 2022, "Regolith Reservoir", false) { }

    public class CoordinateList
    {
        public List<(int, int)> Coordinates { get; set; }
        public CoordinateList(string input)
        {
            Coordinates = input
                .Split(" -> ")
                .Select(a => (a.Split(",")[0], a.Split(",")[1]))
                .Select(a => (int.Parse(a.Item1), int.Parse(a.Item2)))
                .ToList();
        }

        public void FillField(Reservoir reservoir)
        {
            for (int i = 0; i < Coordinates.Count - 1; i++)
            {
                int dx = Direction(Coordinates[i + 1].Item1 - Coordinates[i].Item1);
                int dy = Direction(Coordinates[i + 1].Item2 - Coordinates[i].Item2);

                var pos = (Coordinates[i].Item1, Coordinates[i].Item2);

                while (pos.Item1 != Coordinates[i + 1].Item1 || pos.Item2 != Coordinates[i + 1].Item2)
                {
                    reservoir.SetCoordinate(pos.Item1, pos.Item2, 1);
                    pos.Item1 += dx;
                    pos.Item2 += dy;
                }
                reservoir.SetCoordinate(pos.Item1, pos.Item2, 1);
            }
        }

        private int Direction(int input)
        {
            return input switch
            {
                < 0 => -1,
                > 0 => 1,
                0 => 0
            };
        }
    }
    public class Reservoir
    {
        public int[,] Field { get; set; }
        private int xMax { get; set; }
        private int yMax { get; set; }
        public long SandCounter { get; set; } = 0;
        public bool HasFloor { get; set; } = false;

        public Reservoir(string input, bool hasFloor = false)
        {
            List<CoordinateList> listOfCoordinateLists = input
                .SplitByNewline()
                .Select(a => new CoordinateList(a))
                .ToList();

            HasFloor = hasFloor;

            xMax = 2 * listOfCoordinateLists.Max(a => a.Coordinates.Max(b => b.Item1)) + 1;
            yMax = listOfCoordinateLists.Max(a => a.Coordinates.Max(b => b.Item2)) + (hasFloor ? 3 : 1);

            Field = new int[yMax, xMax];

            foreach (var coordinateList in listOfCoordinateLists)
            {
                coordinateList.FillField(this);
            }
        }

        public void SetCoordinate(int x, int y, int value)
        {
            Field[y, x] = value;
        }

        public int GetCoordinate(int x, int y)
        {
            if (HasFloor && y == yMax - 1) return 1;

            return Field[y, x];
        }

        public void DropSand(int sourceX, int sourceY, bool hasFloor = false)
        {
            while(true)
            {
                int x = sourceX, y = Math.Max(sourceY, 0);
                while (true)
                {
                    if (y + 1 >= yMax) return;

                    if (GetCoordinate(x, y) > 0) return;

                    if (GetCoordinate(x, y + 1) == 0)
                    {
                        y++;
                        continue;
                    }

                    if (GetCoordinate(x - 1, y + 1) == 0)
                    {
                        y++; x--;
                        continue;
                    }

                    if (GetCoordinate(x + 1, y + 1) == 0)
                    {
                        y++; x++;
                        continue;
                    }

                    Field[y, x] = 2;
                    SandCounter++;
                    break;
                }
            }
        }

        public void PrintWorld()
        {
            int xStart = -1, xEnd = Field.GetLength(1) + 1;
            for (int x = 0; x < Field.GetLength(1); x++) 
            {
                for (int y = 0; y < Field.GetLength(0) - 1; y++)
                {
                    if (xStart == -1 && GetCoordinate(x, y) > 0)
                    {
                        xStart = x - 2;
                    }

                    if (GetCoordinate(x, y) > 0)
                    {
                        xEnd = x + 2;
                    }

                }
            }
            for (int y = 0; y < Field.GetLength(0); y++)
            {
                string output = "";
                for (int x = xStart; x < Math.Min(xEnd, Field.GetLength(1)); x++)
                {
                    output += GetCoordinate(x, y) switch
                    {
                        0 => ".",
                        1 => "#",
                        2 => "+",
                        _ => throw new Exception($"Unable to parse field entry {Field[y, x]}")
                    };
                }
                Console.WriteLine(output);
            }
        }
    }
    protected override string SolvePartOne()
    {
        var reservoir = new Reservoir(Input);
        reservoir.DropSand(500, 0);
        //reservoir.PrintWorld();
        return reservoir.SandCounter.ToString();
    }

    protected override string SolvePartTwo()
    {
        var reservoir = new Reservoir(Input, true);
        reservoir.DropSand(500, 0, true);
        reservoir.PrintWorld();
        return reservoir.SandCounter.ToString();
    }
}
