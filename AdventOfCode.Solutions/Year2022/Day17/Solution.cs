namespace AdventOfCode.Solutions.Year2022.Day17;

class Solution : SolutionBase
{
    public Solution() : base(17, 2022, "Pyroclastic Flow", true) { }

    private readonly int[,,] Shapes = new int[,,]
    {
        {
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 1, 1, 1, 1, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
        },
        {
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 1, 0, 0, 0, 2 },
            { 2, 0, 0, 1, 1, 1, 0, 0, 2 },
            { 2, 0, 0, 0, 1, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
        },
        {
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 1, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 1, 0, 0, 2 },
            { 2, 0, 0, 1, 1, 1, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
        },
        {
            { 2, 0, 0, 1, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 1, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 1, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 1, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
        },
        {
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 1, 1, 0, 0, 0, 2 },
            { 2, 0, 0, 1, 1, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
        },
    };

    private int[,] ShapeCoordinates = new int[,]
    {
        // x, y, width, height
        { 3, 3, 4, 1 },
        { 3, 1, 3, 3 },
        { 3, 1, 3, 3 },
        { 3, 0, 1, 4 },
        { 3, 2, 2, 2 },
    };
    int[,] game;
    int[,] protoGame =
    {
        { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
        { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
        { 2, 0, 0, 0, 0, 0, 0, 0, 2 },
        { 4, 3, 3, 3, 3, 3, 3, 3, 4 },
    };

    int shapeX, shapeY, shapeWidth, shapeHeight;

    int pruned = 0;

    private int FindFirstNonEmptyRow()
    {
        for (int row = 0; row < game.GetLength(0); row++)
        {
            for (int column = 1; column < game.GetLength(1) - 1; column++)
            {
                if (game[row, column] > 0)
                {
                    return row;
                }
            }
        }

        return -1;
    }

    private void Prune(int limit = 1000, int toPrune = 900)
    {
        if (game.GetLength(0) > limit)
        {
            var newGame = new int[game.GetLength(0) - toPrune, game.GetLength(1)];
            for (int row = 0; row < game.GetLength(0) - toPrune; row++)
            {
                for (int column = 0; column < game.GetLength(1); column++)
                {
                    newGame[row, column] = game[row, column];
                }
            }
            game = newGame;
            pruned += toPrune;
        }
    }

    private void RemoveEmptyRows()
    {
        var firstNonEmptyRow = FindFirstNonEmptyRow();

        if (firstNonEmptyRow > 0)
        {
            var newGame = new int[game.GetLength(0) - firstNonEmptyRow, game.GetLength(1)];
            for (int row = firstNonEmptyRow; row < game.GetLength(0); row++)
            {
                for (int column = 0; column < game.GetLength(1); column++)
                {
                    newGame[row - firstNonEmptyRow, column] = game[row, column];
                }
            }
            game = newGame;
        }
    }

    int shapeIndex = 0;
    private int[,] GetNextShape()
    {
        var shape = new int[Shapes.GetLength(1), Shapes.GetLength(2)];

        for (int row = 0; row < shape.GetLength(0); row++)
        {
            for (int column = 0; column < shape.GetLength(1); column++)
            {
                shape[row, column] = Shapes[shapeIndex, row, column];
            }
        }


        shapeX = ShapeCoordinates[shapeIndex, 0];
        shapeY = ShapeCoordinates[shapeIndex, 1];
        shapeWidth = ShapeCoordinates[shapeIndex, 2];
        shapeHeight = ShapeCoordinates[shapeIndex, 3];

        shapeIndex = (shapeIndex + 1) % Shapes.GetLength(0);

        return shape;
    }

    private void PrependShape(int[,] shape)
    {
        var newGame = new int[game.GetLength(0) + shape.GetLength(0), game.GetLength(1)];

        for (int row = 0; row < shape.GetLength(0); row++)
        {
            for (int column = 0; column < shape.GetLength(1); column++)
            {
                newGame[row, column] = shape[row, column];
            }
        }

        for (int row = 0; row < game.GetLength(0); row++)
        {
            for (int column = 0; column < game.GetLength(1); column++)
            {
                newGame[row + shape.GetLength(0), column] = game[row, column];
            }
        }

        game = newGame;
    }

    private bool CanSafelyMoveInDirection(int dy, int dx)
    {
        for (int row = 0; row < shapeHeight; row++)
        {
            for (int column = 0; column < shapeWidth; column++)
            {
                if (game[row + shapeY, column + shapeX] == 1)
                {
                    if (game[row + shapeY + dy, column + shapeX + dx] > 1)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private void Move(int dy, int dx)
    {
        var shape = new int[shapeHeight, shapeWidth];
        for (int row = 0; row < shapeHeight; row++)
        {
            for (int column = 0; column < shapeWidth; column++)
            {
                if (game[row + shapeY, column + shapeX] == 1)
                {
                    shape[row, column] = 1;
                    game[row + shapeY, column + shapeX]--;
                }
            }
        }

        for (int row = 0; row < shapeHeight; row++)
        {
            for (int column = 0; column < shapeWidth; column++)
            {
                game[row + shapeY + dy, column + shapeX + dx] += shape[row, column];
            }
        }
        shapeX += dx;
        shapeY += dy;
    }

    private void FinalizeLocation()
    {
        for (int row = 0; row < shapeHeight; row++)
        {
            for (int column = 0; column < shapeWidth; column++)
            {
                if (game[row + shapeY, column + shapeX] == 1)
                {
                    game[row + shapeY, column + shapeX] = 5;
                }
            }
        }
    }

    public void PrintGame()
    {
        for (int row = 0; row < game.GetLength(0); row++)
        {
            string output = "";
            for (int column = 0; column < game.GetLength(1); column++)
            {
                output += game[row, column] switch
                {
                    0 => '.',
                    1 => '@',
                    2 => '|',
                    3 => '_',
                    4 => '+',
                    5 => "#",
                    _ => throw new Exception($"Unable to parse game at ({column}, {row}): {game[row, column]})")
                };
            }
            Console.WriteLine(output);
        }
        Console.WriteLine();
    }

    List<long> gameSizeAfterNBlocks = new();
    public void Simulate(string input, int count)
    {
        game = protoGame;

        int inputIndex = 0;
        for (int i = 0; i < count; i++) {
            //PrintGame();
            RemoveEmptyRows();
            //Prune();
            gameSizeAfterNBlocks.Add(game.GetLength(0) - 1 + pruned);
            PrependShape(GetNextShape());
            //PrintGame();

            while (true)
            {
                var dx = input[inputIndex] switch
                {
                    '<' => -1,
                    '>' => 1,
                    _ => throw new Exception($"Unknown input symbol: {input[inputIndex]}")
                };

                if (CanSafelyMoveInDirection(0, dx))
                {
                    Move(0, dx);
                }
                //PrintGame();
                inputIndex = (inputIndex + 1) % input.Length;

                if (CanSafelyMoveInDirection(1, 0))
                {
                    Move(1, 0);
                    //PrintGame();
                } else
                {
                    FinalizeLocation();
                    break;
                }
            }
        }

    }

    protected override string SolvePartOne()
    {
        //Simulate(Input.Trim(), 2022);
        //RemoveEmptyRows();
        //return (game.GetLength(0) - 1).ToString();
        return "";
    }

    protected override string SolvePartTwo()
    {
        Simulate(Input.Trim(), 10_000);
        RemoveEmptyRows();
        var result2 = (game.GetLength(0) - 1);

        StringBuilder sb = new();

        for (int i = 0; i < gameSizeAfterNBlocks.Count; i++)
        {
            sb.Append($"{i}\t{gameSizeAfterNBlocks[i]}\r\n");
        }

        return $"2: {result2}";
    }
}
