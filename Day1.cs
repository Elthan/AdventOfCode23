using System.Text.RegularExpressions;

namespace AdventOfCode;

internal class Day1
{
    public static void Part1()
    {
        var sum = File.ReadLines("input_day1.txt")
            .Sum(line => long.Parse($"{Regex.Match(line, @"(\d)").Groups[0].Value}" +
                                    $"{Regex.Match(line, @"(\d)", RegexOptions.RightToLeft).Groups[0].Value}"));
        Console.WriteLine(sum);
    }

    public static void Part2()
    {
        var sum = File.ReadLines("input_day1.txt")
            .Sum(line => {
                var res = $"{Regex.Match(line, @"(\d|one|two|three|four|five|six|seven|eight|nine)").Groups[0].Value}" +
                          $"{Regex.Match(line, @"(\d|one|two|three|four|five|six|seven|eight|nine)", RegexOptions.RightToLeft).Groups[0].Value}";
                var converted =
                    res.Replace("one", "1")
                        .Replace("two", "2")
                        .Replace("three", "3")
                        .Replace("four", "4")
                        .Replace("five", "5")
                        .Replace("six", "6")
                        .Replace("seven", "7")
                        .Replace("eight", "8")
                        .Replace("nine", "9");
                return long.Parse(converted);
            }
        );

        Console.WriteLine(sum);
    }
}