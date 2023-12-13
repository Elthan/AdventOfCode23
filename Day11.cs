namespace AdventOfCode;

internal class Day11
{
    public static void Part1()
    {
        var lines = DoubleLines(File.ReadLines("input_day11.txt"));
        var flipped = DoubleLines(FlipArray(lines).Select(line => new string(line)));
        var space = FlipArray(flipped);

        var galaxy = new List<(int, int)>();
        for (var i = 0; i < space.Length; i++)
        {
            for (var j = 0; j < space[i].Length; j++)
            {
                if (space[i][j] != '#') continue;
                galaxy.Add((i, j));
            }
        }

        var result = galaxy.Combinations((galaxyA, galaxyB) =>
            Math.Abs(galaxyB.Item1 - galaxyA.Item1) + Math.Abs(galaxyB.Item2 - galaxyA.Item2));

        Console.WriteLine(result.Sum());
    }

    public static void Part2()
    {
        var lines = MillionMultiplyLines(File.ReadLines("input_day11.txt"));
        var flipped = MillionMultiplyLines(FlipArray(lines).Select(line => new string(line)));
        var space = FlipArray(flipped);

        var galaxy = new List<(long, long)>();
        for (var i = 0; i < space.Length; i++)
        {
            for (var j = 0; j < space[i].Length; j++)
            {
                if (space[i][j] != '#') continue;
                galaxy.Add((i, j));
            }
        }

        var result = galaxy.Combinations((galaxyA, galaxyB) =>
        {
            var expanded = 0;
            var minX = Math.Min(galaxyA.Item1, galaxyB.Item1);
            var minY = Math.Min(galaxyA.Item2, galaxyB.Item2);
            var maxX = Math.Max(galaxyA.Item1, galaxyB.Item1);
            var maxY = Math.Max(galaxyA.Item2, galaxyB.Item2);
            for (var i = minX; i < maxX; i++) if (space[i][minY] is '-') expanded++;
            for (var i = minY; i < maxY; i++) if (space[minX][i] is '-') expanded++;

            return Math.Abs(galaxyB.Item1 - galaxyA.Item1)
                 + Math.Abs(galaxyB.Item2 - galaxyA.Item2)
                 + expanded * 999999;
        });

        Console.WriteLine(result.Sum());
    }

    private static char[][] FlipArray(IReadOnlyList<char[]> array)
    {
        var result = new char[array[0].Length][];
        for (var i = 0; i < array[0].Length; i++)
        {
            result[i] = new char[array.Count];
            for (var j = 0; j < array.Count; j++)
            {
                result[i][j] = array[j][i];
            }
        }
        return result;
    }

    private static char[][] DoubleLines(IEnumerable<string> lines)
    {
        return lines.Select(line => line.All(c => c is '.') ? $"{line}\n{line}" : line)
            .Select(line => line.Split('\n'))
            .SelectMany(s => s)
            .Select(line => line.ToArray())
            .ToArray();
    }

    private static char[][] MillionMultiplyLines(IEnumerable<string> lines)
    {
        return lines.Select(line => line.All(c => c is '.' or '-') ? new string('-', line.Length) : line)
            .Select(line => line.Split('\n'))
            .SelectMany(s => s)
            .Select(line => line.ToArray())
            .ToArray();
    }
}

public static class Day11Extensions
{
    public static IEnumerable<TR> Combinations<T, TR>(this IList<T> source, Func<T, T, TR> func)
    {
        for (var i = 0; i < source.Count - 1; i++)
        {
            for (var j = i + 1; j < source.Count; j++)
            {
                yield return func(source[i], source[j]);
            }
        }
    } 
}