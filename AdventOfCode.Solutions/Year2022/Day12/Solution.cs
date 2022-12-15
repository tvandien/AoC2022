namespace AdventOfCode.Solutions.Year2022.Day12;

class Solution : SolutionBase
{
    public Solution() : base(12, 2022, "Hill Climbing Algorithm", false) { }

    public class Node
    {
        public int Value { get; set; }
        public List<Edge> Edges { get; set; } = new();
        public (int, int) Coordinate { get; set; }
        public bool Start { get; set; }
        public bool End { get; set; }
        public int Distance { get; set; } = 0;

        public Node(char value, (int, int) coordinate, bool start = false, bool end = false)
        {
            this.Value = (int)value;
            this.Coordinate = coordinate;
            this.Start = start;
            this.End = end;
        }

        public List<Node> GetNeighbors()
        {
            return Edges
                .Select(a => a.B)
                .ToList();
        }

        public Edge GetEdgeToNode(Node b)
        {
            return Edges.First(a => a.B.Coordinate == b.Coordinate);
        }
    }

    public class Edge
    {
        public Node A { get; set; }
        public Node B { get; set; }
        public int Weight { get; set; }

        public Edge(Node a, Node b, int weight)
        {
            this.A = a;
            this.B = b;
            this.Weight = weight;
        }
    }

    public class Graph
    {
        public List<Node> Nodes { get; set; } = new();
        public List<Edge> Edges { get; set; } = new();

        public Dictionary<(int, int), Node> NodeDict { get; set; } = new();

        private Node SelectOrCreateNode(int x, int y, char value)
        {
            char actualValue = value switch
            {
                'S' => 'a',
                'E' => 'z',
                _ => value
            };

            if (NodeDict.TryGetValue((x, y), out Node? node))
            {
                return node;
            }

            var newNode = new Node(actualValue, (x, y), value == 'S', value == 'E');
            Nodes.Add(newNode);
            NodeDict.Add((x, y), newNode);

            return newNode;
        }

        private bool inBounds(int x, int y, string[] input)
        {
            return (x >= 0 && x < input[0].Length && y >= 0 && y < input.Length);
        }

        public Graph(string[] input, int maxDistance)
        {
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    var currentNode = SelectOrCreateNode(x, y, input[y][x]);

                    foreach (var (dx, dy) in new List<(int, int)> { (-1, 0), (1, 0), (0, -1), (0, 1) })
                    {
                        if (!inBounds(x + dx, y + dy, input)) continue;

                        var otherNode = SelectOrCreateNode(x + dx, y + dy, input[y + dy][x + dx]);

                        var here = currentNode.Value;
                        var there = otherNode.Value;
                        if ((int)currentNode.Value + maxDistance >= (int)otherNode.Value)
                        {
                            currentNode.Edges.Add(new Edge(currentNode, otherNode, weight: 1));
                        }
                    }
                }
            }
        }

        public int GetDistanceFromStartToEnd()
        {
            var start = Nodes.First(a => a.Start);
            var end = Nodes.First(a => a.End);

            var toVisit = new List<Node>() { start };
            var visited = new List<Node>() { };

            while (toVisit.Count > 0)
            {
                var visiting = toVisit.First();
                toVisit.RemoveAt(0);

                var newNodes = visiting
                    .GetNeighbors()
                    .Where(a => !visited.Exists(b => b.Coordinate == a.Coordinate) && !toVisit.Exists(b => b.Coordinate == a.Coordinate));

                foreach (var node in newNodes)
                {
                    node.Distance = visiting.Distance + visiting.GetEdgeToNode(node).Weight;
                    toVisit.Add(node);
                }

                visited.Add(visiting);
            }

            return end.Distance;
        }

        public void PrintGraph()
        {
            for (int y = 0; y <= Nodes.Max(a => a.Coordinate.Item2); y++)
            {
                string output = "";
                for (int x = 0; x <= Nodes.Max(a => a.Coordinate.Item1); x++)
                {
                    output += $"{Nodes.First(a => a.Coordinate == (x, y)).Distance,2} ";
                }
                Console.WriteLine(output);
            }
        }
    }

    protected override string SolvePartOne()
    {
        var graph = new Graph(Input.SplitByNewline(), maxDistance: 1);

        return graph.GetDistanceFromStartToEnd().ToString();
    }

    protected override string SolvePartTwo()
    {
        var input = Input.Replace("S", "a");

        List<int> solutions = new();

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == 'a')
            {
                var newInput = $"{input[0..i]}S{input[(i + 1)..]}";

                var graph = new Graph(newInput.SplitByNewline(), maxDistance: 1);

                solutions.Add(graph.GetDistanceFromStartToEnd());
            }
        }
        solutions = solutions.Where(a => a != 0).ToList();
        solutions.Sort();
        return solutions.Min().ToString();
    }
}