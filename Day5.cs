using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode;

internal class Day5
{
    public static void Part1()
    {
        var file = File.ReadLines("input_day5.txt").ToList();
        var seeds = file.ElementAt(0).Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
        var input = string.Join('\n', file.Skip(1));
        var maps = Regex.Matches(input, @"(\w+\-to\-\w+ map:)[\d+\s]+")
            .Select(match => match.Groups[0].Value)
            .Select(group => new Map(group.Split(':', StringSplitOptions.RemoveEmptyEntries)[0],
                group.Split(':', StringSplitOptions.RemoveEmptyEntries)[1].Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => new Mapping(
                        long.Parse(line.Split(' ')[0]),
                        long.Parse(line.Split(' ')[1]),
                        long.Parse(line.Split(' ')[2]))).ToArray()));
        var lowestLoc = seeds.Select(seed => Traverse(seed, maps.ToArray(), 0)).Prepend(long.MaxValue).Min();
        Console.WriteLine(lowestLoc);
    }

    public static void Part2()
    {
        var file = File.ReadLines("input_day5.txt").ToList();
        var seeds = file.ElementAt(0).Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
        var input = string.Join('\n', file.Skip(1));
        var maps = Regex.Matches(input, @"(\w+\-to\-\w+ map:)[\d+\s]+")
            .Select(match => match.Groups[0].Value)
            .Select(group => new Map(group.Split(':', StringSplitOptions.RemoveEmptyEntries)[0],
                group.Split(':', StringSplitOptions.RemoveEmptyEntries)[1].Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => new Mapping(
                        long.Parse(line.Split(' ')[0]),
                        long.Parse(line.Split(' ')[1]),
                        long.Parse(line.Split(' ')[2]))).ToArray()));
        var lowestLoc = seeds.Chunk(2).Select(chunk => Tuple.Create(chunk[0], chunk[1]))
            .Select(seedPair => Enumerable.Range((int) seedPair.Item1, (int) seedPair.Item2)
                .Select(seed => Traverse(seed, maps.ToArray(), 0)).Prepend(long.MaxValue).Min()).Min();
        Console.WriteLine(lowestLoc);
    }

    private static long Traverse(long current, IReadOnlyList<Map> maps, int depth)
    {
        while (true)
        {
            if (depth >= maps.Count) return current;
            current = GetMapping(current, maps[depth]);
            depth++;
        }
    }

    private static long GetMapping(long current, Map map)
    {
        foreach (var mapping in map.Mappings)
        {
            if (mapping.SourceStart <= current && current <= mapping.SourceStart + mapping.RangeLength)
            {
                return mapping.DestStart + (current - mapping.SourceStart);
            }
        }

        return current;
    }

    private record Map(string Name, Mapping[] Mappings)
    {
        public override string ToString() => $"{Name} - {string.Join(' ', Mappings.ToList())}";
    }

    private record Mapping(long DestStart, long SourceStart, long RangeLength)
    {
        public override string ToString() => $"{DestStart}:{SourceStart}:{RangeLength}";
    }
}