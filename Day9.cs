namespace AdventOfCode;

internal class Day9
{
    public static void Part1()
    {
        var sum = File.ReadLines("input_day9.txt")
            .Select(line => line.Split(' ').Select(long.Parse).ToList())
            .Select(line => line.Last() + FindNextNumber(line))
            .Sum();
        Console.WriteLine(sum);
    }

    public static void Part2()
    {
        var sum = File.ReadLines("input_day9.txt")
            .Select(line => line.Split(' ').Select(long.Parse).ToList())
            .Select(line => line.First() - FindNextNumber(line, true))
            .Sum();
        Console.WriteLine(sum);
    }

    private static long FindNextNumber(IList<long> sequence, bool previous = false)
    {
        var differences = sequence.Skip(1).Select((_, index) => sequence[index + 1] - sequence[index]).ToList();
        if (differences.Distinct().Count() == 1) return differences[0];
        return previous ? differences.First() - FindNextNumber(differences, previous) : differences.Last() + FindNextNumber(sequence, previous);
    }
}