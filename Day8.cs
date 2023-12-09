using System.Text.RegularExpressions;

namespace AdventOfCode;

internal class Day8
{
    public static void Part1()
    {
        var file = File.ReadLines("input_day8.txt").ToList();
        var instructions = file[0].Select(c => c == 'L' ? Direction.Left : Direction.Right).ToList();
        var nodes = new Dictionary<string, Node>();
        foreach (var line in file.Skip(2))
        {
            var matches = Regex.Matches(line, @"(\w{3}) = \((\w{3}), (\w{3})\)")[0].Groups;
            var currentNodeName = matches[1].Value;
            var leftNodeName = matches[2].Value;
            var rightNodeName = matches[3].Value;
            nodes[currentNodeName] = new Node(currentNodeName, leftNodeName, rightNodeName);
        }

        var steps = 0; 
        var tempNode = nodes["AAA"];
        for (var i = 0; i < instructions.Count;)
        {
            var direction = instructions[i++];
            if (i >= instructions.Count - 1) i = 0;
            steps++;
            tempNode = direction == Direction.Left ? nodes[tempNode.Left] : nodes[tempNode.Right];
            if (tempNode.Name == "ZZZ") break;
        }

        //Console.WriteLine($"{string.Join(' ', instructions)}\n\n{string.Join('\n', nodes)}");
        Console.Write(steps);
    }

    private enum Direction {
        Left, Right
    }

    private record Node(string Name, string Left, string Right)
    {
        public override string ToString() => $"{Name} = ({Left}, {Right})";
        public virtual bool Equals(Node? other) => Name.Equals(other?.Name);
        public override int GetHashCode() => HashCode.Combine(Name, Left, Right);
    }
}