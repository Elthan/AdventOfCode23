namespace AdventOfCode;

internal class Day4
{
    public static void Part1()
    {
        var res = File.ReadLines("input_day4.txt")
            .Select(line => line.Split(':')[1])
            .Select(line => line.Replace("|", "").Replace("  ", " ").Split(' ').Where(num => num.Length > 0))
            .Select(line => line.GroupBy(int.Parse).Where(group => group.Skip(1).Any()))
            .Where(group => group.Any())
            .Sum(group => Math.Pow(2, group.Count() - 1));

        Console.WriteLine(res);
    }

    public static void Part2()
    {
        // Make groups of index + duplicate numbers
        var groups = File.ReadLines("input_day4.txt")
            .Select((line, index) => (index + 1, line.Split(':')[1]))
            .Select(line => (line.Item1, line.Item2.Replace("|", "").Replace("  ", " ").Split(' ').Where(num => num.Length > 0)))
            .Select(line => (line.Item1, line.Item2.GroupBy(int.Parse).Where(group => group.Skip(1).Any())));

        var scratchcards = new Dictionary<int, int>();
        var numSc = 0;
        foreach (var line in groups)
        {
            scratchcards.TryGetValue(line.Item1, out var duplicates);
            duplicates += 1;
            var numWinningNumbers = line.Item2.Count();
            var wins = duplicates * numWinningNumbers;
            for (var i = 1; i <= numWinningNumbers; i++) scratchcards.AddOrUpdate(line.Item1 + i, duplicates);
            numSc += duplicates;
        }

        Console.WriteLine(numSc);
    }
}

internal static class SantasLittleHelper
{
    public static void AddOrUpdate<TKey>(this Dictionary<TKey, int> dict, TKey key, int value)
        where TKey : notnull
    {
        if (dict.TryAdd(key, value)) return;
        var curVal = dict[key];
        dict[key] = value + curVal;
    }
}