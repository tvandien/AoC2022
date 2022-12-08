namespace AdventOfCode.Solutions.Year2022.Day08;

class Solution : SolutionBase
{
    public Solution() : base(08, 2022, "Treetop Tree House", false) { }

    private List<List<int>> GetTreeGrid()
    {
        return Input
            .SplitByNewline()
            .Select(StringToListOfInt)
            .ToList();
    }

    private static List<int> StringToListOfInt(string input)
    {
        return input
            .ToCharArray()
            .Select(b => int.Parse(b.ToString()))
            .ToList();
    }

    private bool IsTreeVisibleFromEdge(List<List<int>> grid, int row, int column)
    {
        // left
        if (grid[row]
            .GetRange(0, column)
            .All(a => a < grid[row][column])) 
        {
            return true;
        }

        // right
        if (grid[row]
            .GetRange(column + 1, grid[row].Count - column - 1)
            .All(a => a < grid[row][column]))
        {
            return true;
        }

        // up
        if (grid.GetRange(0, row)
            .Select(a => a[column])
            .All(a => a < grid[row][column]))
        {
            return true;
        }

        // down
        if (grid.GetRange(row + 1, grid.Count - row - 1)
            .Select(a => a[column])
            .All(a => a < grid[row][column]))
        {
            return true;
        }

        return false;
    }

    private int ScenicScore(List<List<int>> grid, int row, int column)
    {
        var houseHeight = grid[row][column];

        // left
        var left = 0;
        for (int x = column - 1; x >= 0; x--) {
            var treeHeight = grid[row][x];

            left++;

            if (treeHeight >= houseHeight)
            {
                break;
            }
        }

        // right
        var right = 0;
        for (int x = column + 1; x < grid[row].Count; x++)
        {
            var treeHeight = grid[row][x];

            right++;

            if (treeHeight >= houseHeight)
            {
                break;
            }
        }

        // up
        var up = 0;
        for (int y = row - 1; y >= 0; y--)
        {
            var treeHeight = grid[y][column];

            up++;

            if (treeHeight >= houseHeight)
            {
                break;
            }
        }

        // down
        var down = 0;
        for (int y = row + 1; y < grid.Count; y++)
        {
            var treeHeight = grid[y][column];

            down++;

            if (treeHeight >= houseHeight)
            {
                break;
            }
        }

        return left * right * up * down;
    }

    protected override string SolvePartOne()
    {
        var grid = GetTreeGrid();
        var visible = new List<List<bool>>();
        for (int row = 0; row < grid.Count; row++)
        {
            visible.Add(new());
            for (int column = 0; column < grid[row].Count; column++)
            {
                visible[row].Add(IsTreeVisibleFromEdge(grid, row, column));
            }
        }
        return visible.Sum(a => a.Count(b => b)).ToString();
    }

    protected override string SolvePartTwo()
    {
        var grid = GetTreeGrid();
        var scenicScores = new List<List<int>>
        {
            new()
        };
        for (int row = 1; row < grid.Count - 1; row++)
        {
            scenicScores.Add(new());
            scenicScores[row].Add(0);
            for (int column = 1; column < grid[row].Count - 1; column++)
            {
                scenicScores[row].Add(ScenicScore(grid, row, column));
            }
            scenicScores[row].Add(0);
        }
        scenicScores.Add(new());

        return scenicScores.SelectMany(a => a).Max().ToString();
    }
}
