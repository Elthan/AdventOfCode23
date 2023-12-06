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
        var seeds = file
            .ElementAt(0)
            .Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .Chunk(2)
            .Select(chunk => new Seed(chunk[0], chunk[1]))
            .ToList();
        var input = string.Join('\n', file.Skip(1));
        var maps = Regex.Matches(input, @"(\w+\-to\-\w+ map:)[\d+\s]+")
            .Select(match => match.Groups[0].Value)
            .Select(group => new Map(
                group.Split(':', StringSplitOptions.RemoveEmptyEntries)[0],
                group.Split(':', StringSplitOptions.RemoveEmptyEntries)[1]
                    .Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => new Mapping(
                        long.Parse(line.Split(' ')[0]),
                        long.Parse(line.Split(' ')[1]),
                        long.Parse(line.Split(' ')[2])))
                    .ToArray()));

        var seedSegments = new HashSet<Seed>(seeds);
        foreach (var map in maps)
        {
            var mappedSeeds = new List<Seed>();
            foreach (var mapping in map.Mappings)
            {
                var newSegments = new List<Seed>();
                foreach (var seed in seedSegments)
                {
                    var mappingMax = mapping.SourceStart + mapping.RangeLength;
                    
                    // Contains overlap
                    var overlap = Math.Min(seed.Max(), mappingMax) - Math.Max(seed.Start, mapping.SourceStart);
                    if (overlap > 0)
                    {
                        var preSeed = seed with { Range = mapping.SourceStart - seed.Start };
                        var overlapSeed = new Seed(
                            mapping.DestStart + Math.Max(seed.Start - mapping.SourceStart, 0),
                            overlap);
                        var postSeed = new Seed(mappingMax, seed.Max() - mappingMax);

                        mappedSeeds.Add(overlapSeed);
                        if (preSeed.Range > 0) newSegments.Add(preSeed);
                        if (postSeed.Range > 0) newSegments.Add(postSeed);
                    }
                    else
                    {
                        newSegments.Add(seed);
                    }
                }

                seedSegments = new HashSet<Seed>(newSegments);
            }
            seedSegments.UnionWith(mappedSeeds);
        }

        Console.WriteLine(seedSegments.Min(seed => seed.Start));
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

    private record Seed(long Start, long Range)
    {
        public override string ToString() => $"{Start}:{Range}:{Start + Range}";
        public long Max() => Start + Range;
    }

    private record Map(string Name, Mapping[] Mappings)
    {
        public override string ToString() => $"{Name}";
    }

    private record Mapping(long DestStart, long SourceStart, long RangeLength)
    {
        public override string ToString() => $"{DestStart}:{SourceStart}:{RangeLength}";
    }
}