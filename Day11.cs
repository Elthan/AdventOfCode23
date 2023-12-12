using System.Text.RegularExpressions;

namespace AdventOfCode;

internal class Day11
{
    public static void Part1()
    {
        var count = 0;
        var rx = new Regex("#");
        var lines = File.ReadLines("input_day11.txt")
            .Select(line => rx.Replace(line, _ => $"{count++}"))
            .Select(line => line.All(c => c is '.') ? $"{line}\n{line}" : line)
            .Select(line => line.Split('\n'))
            .SelectMany(s => s);

        Console.WriteLine(string.Join('\n', lines));
        Console.WriteLine(lines.Count());
    }
}