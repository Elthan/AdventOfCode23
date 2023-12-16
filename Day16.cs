namespace AdventOfCode;

internal class Day16
{
    public static void Part1()
    {
        var board = File.ReadLines("input_day16.txt").Select(line => line.ToArray()).ToArray();
        var visited = new HashSet<(int, int, Direction)>();
        Traverse(board, (0, 0), Direction.East, visited);
        var energised = new HashSet<(int, int)>();
        energised.UnionWith(visited.Select(tuple => (tuple.Item1, tuple.Item2)));
        Console.WriteLine(energised.Count);
    }

    public static void Part2()
    {
        var board = File.ReadLines("input_day16.txt").Select(line => line.ToArray()).ToArray();

        var energisedMax = new List<int>();
        for (var i = 0; i < board.Length; i++)
        {
            var visited = new HashSet<(int, int, Direction)>();
            Traverse(board, (i, 0), Direction.East, visited);
            var energised = new HashSet<(int, int)>();
            energised.UnionWith(visited.Select(tuple => (tuple.Item1, tuple.Item2)));
            energisedMax.Add(energised.Count);
        }
        for (var i = 0; i < board.Length; i++)
        {
            var visited = new HashSet<(int, int, Direction)>();
            Traverse(board, (i, board[0].Length - 1), Direction.West, visited);
            var energised = new HashSet<(int, int)>();
            energised.UnionWith(visited.Select(tuple => (tuple.Item1, tuple.Item2)));
            energisedMax.Add(energised.Count);
        }
        for (var i = 0; i < board[0].Length; i++)
        {
            var visited = new HashSet<(int, int, Direction)>();
            Traverse(board, (0, i), Direction.South, visited);
            var energised = new HashSet<(int, int)>();
            energised.UnionWith(visited.Select(tuple => (tuple.Item1, tuple.Item2)));
            energisedMax.Add(energised.Count);
        }
        for (var i = 0; i < board[0].Length; i++)
        {
            var visited = new HashSet<(int, int, Direction)>();
            Traverse(board, (board.Length - 1, i), Direction.North, visited);
            var energised = new HashSet<(int, int)>();
            energised.UnionWith(visited.Select(tuple => (tuple.Item1, tuple.Item2)));
            energisedMax.Add(energised.Count);
        }

        Console.WriteLine(energisedMax.Max());
    }

    private static void Traverse(char[][] board, (int, int) position, Direction direction, ISet<(int, int, Direction)> visited)
    {
        while (true)
        {
            var (row, col) = position;
            var value = board.GetValueAt(row, col);

            if (value == null) return;
            if (!visited.Add((position.Item1, position.Item2, direction))) return;

            switch (value)
            {
                case '-' when direction is Direction.North or Direction.South:
                    Traverse(board, DirectionToNext(position, Direction.West), Direction.West, visited);
                    position = DirectionToNext(position, Direction.East);
                    direction = Direction.East;
                    continue;
                case '|' when direction is Direction.East or Direction.West:
                    Traverse(board, DirectionToNext(position, Direction.North), Direction.North, visited);
                    position = DirectionToNext(position, Direction.South);
                    direction = Direction.South;
                    continue;
                default:
                {
                    var newDirection = value switch
                    {
                        '/' => direction switch
                        {
                            Direction.North => Direction.East,
                            Direction.East => Direction.North,
                            Direction.South => Direction.West,
                            Direction.West => Direction.South,
                            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                        },
                        '\\' => direction switch
                        {
                            Direction.North => Direction.West,
                            Direction.East => Direction.South,
                            Direction.South => Direction.East,
                            Direction.West => Direction.North,
                            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                        },
                        _ => direction
                    };

                    position = DirectionToNext(position, newDirection);
                    direction = newDirection;
                    continue;
                }
            }
        }
    }

    private static (int, int) DirectionToNext((int, int) current, Direction direction) =>
        direction switch
        {
            Direction.North => (current.Item1 - 1, current.Item2),
            Direction.East => (current.Item1, current.Item2 + 1),
            Direction.South => (current.Item1 + 1, current.Item2),
            Direction.West => (current.Item1, current.Item2 - 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private enum Direction
    {
        North, East, South, West
    }
}