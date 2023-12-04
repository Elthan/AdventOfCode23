using System.Text.RegularExpressions;

namespace AdventOfCode;

internal class Day2Part1
{
    readonly Dictionary<string, int> limits = new()
    {
        { "red", 12 },
        { "green", 13 },
        { "blue", 14 },
    };

    public void Run()
    {
        var sumIds = 0;
        foreach (var line in File.ReadLines("input_day2.txt"))
        {
            var id = int.Parse(Regex.Match(line, @"Game (\d+):").Groups[1].Value);
            var games = line.Split(':')[1];
            var matches = Regex.Matches(games.Trim(), @"(\d+) (\w+)[;|,]*");
            sumIds += IsOverLimit(matches) ? 0 : id;
        }

        Console.WriteLine(sumIds);
    }

    private bool IsOverLimit(MatchCollection matches)
    {
        foreach (var match in matches.Cast<Match>())
        {
            var number = match.Groups[1].Value;
            var colour = match.Groups[2].Value;
            if (int.Parse(number) > limits[match.Groups[2].Value])
            {
                Console.WriteLine($"Is over limit! {number} {colour}");
                        return true;
            }
        }
        return false;
    }
}

internal class Day2Part2
{
    public static void Run()
    {
        var sumIds = 0L;
        foreach (var line in File.ReadLines("input_day2.txt"))
        {
            var id = int.Parse(Regex.Match(line, @"Game (\d+):").Groups[1].Value);
            var games = line.Split(':')[1];
            var matches = Regex.Matches(games.Trim(), @"(\d+) (\w+)[;|,]*");
            sumIds += FindPower(matches);
        }

        Console.WriteLine(sumIds);
    }

    private static long FindPower(MatchCollection matches)
    {
        var max_red = 0;
        var max_green = 0;
        var max_blue = 0;
        foreach (var match in matches.Cast<Match>())
        {
            var number = int.Parse(match.Groups[1].Value);
            var colour = match.Groups[2].Value;
            if (colour.Equals("red") && number > max_red) max_red = number;
            else if (colour.Equals("blue") && number > max_blue) max_blue = number;
            else if (colour.Equals("green") && number > max_green) max_green = number;
        }

        return max_red * max_green * max_blue;
    }
}