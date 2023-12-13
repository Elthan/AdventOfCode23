namespace AdventOfCode;

internal class Day12
{
    public static void Part1()
    {
        var springs = File.ReadLines("input_day12.txt")
            .Select(line => new Springs(line.Split(' ')[0],
                line.Split(' ')[1].Split(',').Select(int.Parse).ToArray()))
            .Select(FindPermutations);

    }

    public static void Part2() { }

    private static int FindPermutations(Springs spring)
    {
        var (conditionRecord, permutations) = spring;

        return 0;
    }

    private record Springs(string ConditionRecord, int[] Permutations);
}