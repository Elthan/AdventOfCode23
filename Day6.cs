namespace AdventOfCode;

internal static class Day6
{
    public static void Part1()
    {
        var list = File.ReadLines("input_day6.txt")
            .Select(line => line.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToList();
        var result = list[0].Zip(list[1], (t, d) => new Race(int.Parse(t), int.Parse(d)))
            .Select(race =>
            {
                var firstWinner = FindWinner(race);
                return race.Time - firstWinner - firstWinner + 1;
            }).Aggregate((h, l) => h * l);
        Console.WriteLine(result);
    }

    public static void Part2()
    {
        var list = File.ReadLines("input_day6.txt")
            .Select(line => line.Split(':')[1].Replace(" ", "")).ToList();
        var race = new Race(long.Parse(list[0]), long.Parse(list[1]));
        var firstWinner = FindWinner(race);
        var result = race.Time - firstWinner - firstWinner + 1;
        Console.WriteLine(result);
    }

    private static long FindWinner(Race race)
    {
        for (var hold = 1; hold < race.Time; hold++)
        {
            var distance = hold * (race.Time - hold);
            if (distance > race.Score) return hold;
        }
        return 0;
    }

    private record Race(long Time, long Score)
    {
        public override string ToString() => $"{Time}:{Score}";
    }
}