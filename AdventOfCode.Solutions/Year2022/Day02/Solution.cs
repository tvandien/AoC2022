namespace AdventOfCode.Solutions.Year2022.Day02;

class Solution : SolutionBase
{
    public Solution() : base(02, 2022, "Rock Paper Scissors", false) { }

    private enum GameMove
    {
        Rock = 0, Paper = 1, Scissors = 2
    }

    private enum GameResult
    {
        Lose = 0, Draw = 3, Win = 6
    }

    private class Game
    {
        public GameMove OpponentsMove { get; set; }
        public GameMove MyMove { get; set; }
        public GameResult Result { get; set; }

        public override string ToString()
        {
            return $"I play {MyMove} vs {OpponentsMove}. I {Result}. Game is worth {Score()} points.";
        }

        public Game(string inputLine, bool secondInputIsGameResult = false)
        {
            var input = inputLine.Split(" ");
            var (firstInput, secondInput) = (input[0], input[1]);
            if (secondInputIsGameResult)
            {
                OpponentsMove = GameMoveFromString(firstInput);
                Result = GameResultFromString(secondInput);
                MyMove = GetMyMoveFromOpponentsMoveAndResult();
            }
            else
            {
                OpponentsMove = GameMoveFromString(firstInput);
                MyMove = GameMoveFromString(secondInput);
                Result = GetGameResultFromMoves();
            }

        }

        private static GameResult GameResultFromString(string input) =>
            input switch
            {
                "X" => GameResult.Lose,
                "Y" => GameResult.Draw,
                "Z" => GameResult.Win,
                _ => throw new Exception($"Unknown input: {input}")
            };

        private static GameMove GameMoveFromString(string input) =>
            input switch
            {
                "A" => GameMove.Rock,
                "X" => GameMove.Rock,
                "B" => GameMove.Paper,
                "Y" => GameMove.Paper,
                "C" => GameMove.Scissors,
                "Z" => GameMove.Scissors,
                _ => throw new Exception($"Unknown input: {input}")
            };

        private GameResult GetGameResultFromMoves()
        {
            if (MyMove == OpponentsMove) 
                return GameResult.Draw;

            if ((MyMove - OpponentsMove + 3) % 3 == 1) 
                return GameResult.Win;

            return GameResult.Lose;
        }

        private GameMove GetMyMoveFromOpponentsMoveAndResult()
        {
            if (Result == GameResult.Draw) 
                return OpponentsMove;

            if (Result == GameResult.Win) 
                return (GameMove)(((int)OpponentsMove + 1) % 3);

            return (GameMove)((((int)OpponentsMove) + 2) % 3);
        }

        public int Score()
        {
            return (int)MyMove + 1 + (int)Result;
        }
    }

    protected override string SolvePartOne()
    {
        var games = Input
            .SplitByNewline()
            .Select(a => new Game(a));

        var scores = games.Select(a => a.Score());

        var score = scores.Sum();

        return score.ToString();
    }

    protected override string SolvePartTwo()
    {
        var games = Input
            .SplitByNewline()
            .Select(a => new Game(a, true));

        var scores = games.Select(a => a.Score());

        var score = scores.Sum();

        return score.ToString();
    }
}
