namespace AdventOfCode;

internal enum Type
{
    Character,
    Digit,
    Gear,
    Dot
}

internal class Day3
{
    public static void Part1()
    {
        var file = File.ReadLines("input_day3.txt");
        var grid = file.Select(line => line.Trim().ToCharArray()).ToArray();
        var sum = 0;
        for (var row = 0; row < grid.Length; row++)
        {
            for (var column = 0; column < grid[row].Length; column++)
            {
                var shouldBeCounted = false;
                var symbol = grid[row][column];
                var symbolType = CharToType(symbol);
                if (symbolType != Type.Digit) continue;

                // Find all other digits in line, turn into number
                var partNumber = GetNumber(column, symbol, grid[row]);
                var value = int.Parse(partNumber);
                // Get all neighbors
                for (var col = column; col < column + partNumber.Length; col++)
                {
                    // Left
                    if (CheckIfShouldBeCounted(grid, row, col - 1)) shouldBeCounted = true;
                    // Right
                    if (CheckIfShouldBeCounted(grid, row, col + 1)) shouldBeCounted = true;
                    // LeftUp
                    if (CheckIfShouldBeCounted(grid, row - 1, col - 1)) shouldBeCounted = true;
                    // Up
                    if (CheckIfShouldBeCounted(grid, row - 1, col)) shouldBeCounted = true;
                    // RightUp
                    if (CheckIfShouldBeCounted(grid, row - 1, col + 1)) shouldBeCounted = true;
                    // RightDown
                    if (CheckIfShouldBeCounted(grid, row + 1, col + 1)) shouldBeCounted = true;
                    // Down
                    if (CheckIfShouldBeCounted(grid, row + 1, col)) shouldBeCounted = true;
                    // LeftDown
                    if (CheckIfShouldBeCounted(grid, row + 1, col - 1)) shouldBeCounted = true;
                }

                column += partNumber.Length;
                if (shouldBeCounted) sum += value;
            }
        }

        Console.WriteLine($"Sum: {sum}");
    }

    private static string GetNumber(int index, char symbol, IReadOnlyList<char> line)
    {
        var partNumber = "";
        var symbolType = Type.Digit;
        while (symbolType == Type.Digit)
        {
            partNumber += $"{symbol}";
            if (index + 1 >= line.Count) break;
            symbol = line[++index];
            symbolType = CharToType(symbol);
        }

        return partNumber;
    }

    public static void Part2()
    {
        var nodes = File.ReadLines("input_day3.txt")
            .Select((line, row) =>
            {
                var nodes = new List<Node>();
                for (var col = 0; col < line.Length; col++)
                {
                    var c = line[col];
                    var type = CharToType(c);
                    switch (type)
                    {
                        case Type.Digit:
                        {
                            var number = GetNumber(col, c, line.ToArray());
                            var coords = number.Select((_, index) => new Coords(row, col + index)).ToArray();
                            col += number.Length - 1;
                            nodes.Add(new Node(coords, Type.Digit, number));
                            break;
                        }
                        case Type.Gear:
                            nodes.Add(new Node(new[] {new Coords(row, col)}, Type.Gear, $"{c}"));
                            break;
                        case Type.Character:
                        case Type.Dot:
                        default:
                            break;
                    }
                }

                return nodes;
            }).SelectMany(n => n).ToList();
        
        var sumGears = (from gear in nodes.Where(n => n.Type == Type.Gear)
            let gRow = gear.Coords[0].Row
            let gCol = gear.Coords[0].Col
            select nodes.Where(n => n.Type == Type.Digit)
                .Where(n => n.Coords.Any(coords => gRow - 1 <= coords.Row && coords.Row <= gRow + 1 &&
                                                   gCol - 1 <= coords.Col && coords.Col <= gCol + 1))
                .Select(n => int.Parse(n.Value))
                .ToArray()
            into gearValue
            where gearValue.Length > 1
            select gearValue[0] * gearValue[1]).Sum();

        Console.WriteLine(sumGears);
    }

    private static bool CheckIfShouldBeCounted(IReadOnlyList<char[]> grid, int i, int j)
    {
        try
        {
            var type = CharToType(grid[i][j]);
            return type is Type.Character or Type.Gear;
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        }
        catch (ArgumentOutOfRangeException)
        {
            return false;
        }
    }

    private static Type CharToType(char c)
    {
        if (c.Equals('.')) return Type.Dot;
        if (c.Equals('*')) return Type.Gear;
        if (char.IsDigit(c)) return Type.Digit;
        return Type.Character;
    }

    private record Node(Coords[] Coords, Type Type, string Value)
    {
        public override string ToString()
        {
            var coords = Coords.Select(c => c.ToString());
            return $"{string.Join(' ', coords)}: {Type} - {Value}";
        }
    }

    private record Coords(int Row, int Col)
    {
        public override string ToString() => $"{Row}:{Col}";
    }
}
