namespace AdventOfCode;

internal static class Day6
{
    public static void Part1()
    {
        var list = File.ReadLines("input_day6.txt")
            .Select(line => line.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToList();
        var result = list[0].Zip(list[1], (t, d) => new Race(int.Parse(t), int.Parse(d)))
            .Select(race => FindWinner(race, true) - FindWinner(race) + 1).Aggregate((h, l) => h * l);
        Console.WriteLine(result);
    }

    public static void Part2()
    {
        var list = File.ReadLines("input_day6.txt")
            .Select(line => line.Split(':')[1].Replace(" ", "")).ToList();
        var race = new Race(long.Parse(list[0]), long.Parse(list[1]));
        var result = FindWinner(race, true) - FindWinner(race) + 1;
        Console.WriteLine(result);
    }

    private static long FindWinner(Race race, bool reverse = false)
    {
        if (reverse)
        {
            for (var hold = race.Time; hold > 0; hold--)
            {
                var distance = hold * (race.Time - hold);
                if (distance > race.Score) return hold;
            }
        }
        else
        {
            for (var hold = 0; hold < race.Time; hold++)
            {
                var distance = hold * (race.Time - hold);
                if (distance > race.Score) return hold;
            }
        }

        return 0;
    }

    private record Race(long Time, long Score)
    {
        public override string ToString() => $"{Time}:{Score}";
    }
}