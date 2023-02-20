using System;
using System.Reflection;

namespace AdventOfCode.Solutions.Year2022.Day16;

class Solution : SolutionBase
{
    public Solution() : base(16, 2022, "Proboscidea Volcanium", false) { }



    public class Node
    {
        public string Name { get; set; }
        public int Rate { get; set; }
        public List<Edge> Edges { get; set; } = new();
        public int Distance { get; set; } = 0;
        public int Delay { get; set; } = 1;
        public int ActivationDelay { get; set; } = 1;

        public Node(string name, int rate)
        {
            Name = name;
            Rate = rate;
        }

        public List<Node> GetNeighbors()
        {
            return Edges
                .Select(a => a.A.Name == Name ? a.B : a.A)
                .ToList();
        }

        public void RemoveEdgeToNode(Node node, Graph graph)
        {
            Edges.RemoveAll(a => (a.A.Name == node.Name || a.B.Name == node.Name));
            graph.Edges.RemoveAll(a => (a.A.Name == node.Name || a.B.Name == node.Name));
        }

        public override string ToString()
        {
            return $"{Name} <-> {string.Join(", ", GetNeighbors().Select(a => $"{a.Name}"))} ({Rate})";
        }
    }

    public class Edge
    {
        public Node A { get; set; }
        public Node B { get; set; }
        public int Weight { get; set; }
        public bool Bidirectional { get; set; }

        public Edge(Node a, Node b, int weight = 1, bool bidirectional = false)
        {
            A = a;
            B = b;
            Weight = weight;
            Bidirectional = bidirectional;
        }

        public override string ToString()
        {
            return $"{A.Name} {(Bidirectional ? "<->" : "->")} {B.Name}";
        }
    }

    public class Graph
    {
        public List<Edge> Edges { get; set; } = new();

        public Dictionary<string, Node> Nodes { get; set; } = new();

        private Node SelectOrCreateNode(string name, int rate)
        {
            if (Nodes.TryGetValue(name, out Node? node))
            {
                if (node.Rate == -1)
                {
                    node.Rate = rate;
                }
                return node;
            }

            var newNode = new Node(name, rate);
            Nodes.Add(name, newNode);

            return newNode;
        }

        private static void ParseLine(string line, out string name, out int rate, out string[] neighbours)
        {
            var words = line
                .Replace("="," ")
                .Replace(";", "")
                .Split(" ");
            name = words[1];
            rate = int.Parse(words[5]);
            neighbours = string
                .Join("", words[10..])
                .Split(",");
        }

        public Graph(string[] input)
        {
            foreach(var line in input)
            {
                ParseLine(line, out string name, out int rate, out string[] neighbours);

                var currentNode = SelectOrCreateNode(name, rate);

                foreach (var neighbour in neighbours)
                {
                    var neighbourNode = SelectOrCreateNode(neighbour, -1);

                    if (neighbourNode.GetNeighbors().Any(a => a.Name == currentNode.Name)) continue;

                    var edge = new Edge(currentNode, neighbourNode, 1, true);
                    currentNode.Edges.Add(edge);
                    neighbourNode.Edges.Add(edge);
                    Edges.Add(edge);
                }
            }

            Reduce();
        }

        public void Reduce()
        {
            foreach (var node in Nodes.Values)
            {
                if (node.Rate > 0) continue;
                if (node.Edges.Count > 2) continue;

                var neighbours = node.GetNeighbors().ToList();

                var newEdge = new Edge(neighbours[0], neighbours[1], node.Edges.Sum(a => a.Weight));

                neighbours[0].RemoveEdgeToNode(node, this);
                neighbours[1].RemoveEdgeToNode(node, this);
                neighbours[0].Edges.Add(newEdge);
                neighbours[1].Edges.Add(newEdge);

                Nodes.Remove(node.Name);
            }
        }

        private Dictionary<int, string> CopyAndAdd(Dictionary<int, string> input, string toAdd, int atTime)
        {
            var output = new Dictionary<int, string>();
            foreach (var (key, val) in input)
            {
                output[key] = val;
            }
            output[atTime] = toAdd;

            return output;
        }

        public Dictionary<int, string> FindBestSolution(Dictionary<int, string> minutes, Dictionary<int, string> active, string lastLocation, int time)
        {
            if (time > 10)
            {
                int score = active.Sum(a => (30 - a.Key) * Nodes[a.Value].Rate);

                if (score < 400)
                {
                    return active;
                }
            }
            if (time >= 30)
            {
                return active;
            }

            string location = minutes[time];
            var node = Nodes[location];
            List<Dictionary<int, string>> options = new();

            if (node.Rate > 0 && !active.ContainsValue(location))
            {
                options.Add(
                    FindBestSolution(
                        CopyAndAdd(minutes, location, time + 1),
                        CopyAndAdd(active, location, time),
                        location,
                        time + 1)
                    );
            }

            var paths = node.GetNeighbors();

            foreach (var path in paths)
            {
                if (path.Name == lastLocation) continue;
                var edge = path.Edges.First(a => a.A.Name == path.Name ? a.B.Name == location : a.A.Name == location);

                options.Add(
                    FindBestSolution(
                        CopyAndAdd(minutes, path.Name, time + edge.Weight),
                        active,
                        location,
                        time + edge.Weight)
                    );
            }

            if (!options.Any())
            {
                return active;
            }

            var scores = options.Select(a => a.Sum(a => (30 - a.Key) * Nodes[a.Value].Rate)).ToList();
            var index = scores.IndexOf(scores.Max());

            return options[index];
        }
    }


    protected override string SolvePartOne()
    {
        var graph = new Graph(Input.SplitByNewline());
        var start = new Dictionary<int, string>
        {
            { 1, "AA" }
        };
        var result = graph.FindBestSolution(start, new Dictionary<int, string>(), "", 1);

        var score = result.Sum(a => (30 - a.Key) * graph.Nodes[a.Value].Rate);

        return score.ToString();
    }

    protected override string SolvePartTwo()
    {
        return "";
    }
}
