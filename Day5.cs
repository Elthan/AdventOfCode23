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

        // For each seed pair
        // Find the lowest match in overlapping numbers
        // Convert those numbers according to map, add to sections
        // Take rest into new section
        // Rest of numbers are the same
        // Repeat for next map
        // Lowest number in results is location

        var seedSegments = new HashSet<Seed>(seeds);
        foreach (var map in maps)
        {
            //Console.WriteLine(map);
            //var mappedSeeds = new List<Seed>();
            foreach (var mapping in map.Mappings)
            {
                //Console.WriteLine($"Mapping: {mapping}");
                var newSegments = new List<Seed>();
                foreach (var seed in seedSegments)
                {
                    //Console.WriteLine($"Seed: {seed}");
                    var mapMax = mapping.SourceStart + mapping.RangeLength;
                    // Mapping within seed, create three new seeds (beginning, overlap, end)
                    if (mapping.SourceStart > seed.Start && mapMax < seed.Max())
                    {
                        var preSeed = seed with { Range = mapping.SourceStart - seed.Start };
                        var overlapSeed = new Seed(mapping.DestStart, mapping.RangeLength);
                        var postSeed = new Seed(mapping.SourceStart + mapping.RangeLength, seed.Max() - preSeed.Range - mapping.RangeLength);
                        newSegments.Add(preSeed);
                        newSegments.Add(postSeed);
                        newSegments.Add(overlapSeed);
                        continue;
                    }

                    var overlap = Math.Min(seed.Max(), mapMax) - Math.Max(seed.Start, mapping.SourceStart);
                    //if (overlap == 0) Console.WriteLine($"Overlap 0 for {seed} - {mapping}");
                    //if (overlap > 0) Console.WriteLine($"Overlap for seed {seed} - {overlap}");
                    var newSeedStart = Math.Max(mapping.DestStart + (seed.Start - mapping.SourceStart), mapping.DestStart);
                    //if (newSeedStart == 0 && overlap > 0) Console.WriteLine($"New start 0 for {seed} - {mapping}");
                    if (overlap == seed.Range)
                    {
                        //Console.WriteLine($"Full overlap: new start: {newSeedStart} - {seed} - {mapping}");
                        // Full overlap
                        newSegments.Add(seed with { Start = newSeedStart });
                        continue;
                    }
                    
                    if (overlap <= 0)
                    {
                        // No overlap, continue as you were
                        newSegments.Add(seed);
                    }
                    else
                    {
                        // Partial overlap, split the seed
                        var overlapSeed = new Seed(newSeedStart, overlap);
                        // Start at either seed Start if overlap at end or seed start + overlap if overlap at start of seed
                        var restSeed = new Seed(seed.Start + (seed.Start >= mapping.SourceStart ? overlap : 0), seed.Range - overlap);
                        newSegments.Add(overlapSeed);
                        newSegments.Add(restSeed);
                    }
                }

                seedSegments = new HashSet<Seed>(newSegments);
            }
            //seedSegments.UnionWith(mappedSeeds);
            //Console.WriteLine($"mappedSeeds: {string.Join('\n', mappedSeeds)}");
        }

        Console.WriteLine();
        Console.WriteLine(string.Join('\n', seedSegments));
        Console.WriteLine();
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