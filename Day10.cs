internal class Day10
{
    public static void Part1()
    {
        var board = File.ReadLines("input_day10.txt").Select(line => line.ToArray()).ToArray();

        var startingPos = (0, 0);
        for (var row = 0; row < board.Length; row++)
        {
            for (var col = 0; col < board[row].Length; col++)
            {
                if (board[row][col] == 'S')
                {
                    startingPos = (row, col);
                }
            }
        }

        var tempPipe = GoToFirstPipe((startingPos.Item1, startingPos.Item2), board);
        var prevPipe = startingPos;
        var steps = 0;
        do
        {
            steps++;
            var nextPipe = GoToNextPipe(board[tempPipe.Item1][tempPipe.Item2], prevPipe, tempPipe);
            prevPipe = tempPipe;
            tempPipe = nextPipe;
        } while (board[tempPipe.Item1][tempPipe.Item2] != 'S');

        Console.WriteLine((steps + 1) / 2);
    }

    public static void Part2() { }

    private static (int, int) GoToFirstPipe((int, int) curPos, char[][] board)
    {
        var left = board.GetValueAt(curPos.Item1, curPos.Item2 - 1);
        var right = board.GetValueAt(curPos.Item1, curPos.Item2 + 1);
        var up = board.GetValueAt(curPos.Item1 - 1, curPos.Item2);
        var down = board.GetValueAt(curPos.Item1 + 1, curPos.Item2);

        if (left is '-' or 'F' or 'L') return (curPos.Item1, curPos.Item2 - 1);
        if (right is '-' or 'J' or '7') return (curPos.Item1, curPos.Item2 + 1);
        if (up is '|' or 'F' or '7') return (curPos.Item1 - 1, curPos.Item2);
        if (down is '|' or 'L' or 'J') return (curPos.Item1 + 1, curPos.Item2);
        return (-1, -1);
    }

    private static (int, int) GoToNextPipe(char currentPipe, (int, int) prevPos, (int, int) curPos)
    {
        return currentPipe switch
        { 
             '-' => prevPos.Item2 < curPos.Item2 ? (curPos.Item1, curPos.Item2 + 1) : (curPos.Item1, curPos.Item2 - 1),
             '|' => prevPos.Item1 < curPos.Item1 ? (curPos.Item1 + 1, curPos.Item2) : (curPos.Item1 - 1, curPos.Item2),
             'F' => prevPos.Item1 != curPos.Item1 ? (curPos.Item1, curPos.Item2 + 1) : (curPos.Item1 + 1, curPos.Item2),
             'L' => prevPos.Item1 != curPos.Item1 ? (curPos.Item1, curPos.Item2 + 1) : (curPos.Item1 - 1, curPos.Item2),
             'J' => prevPos.Item1 != curPos.Item1 ? (curPos.Item1, curPos.Item2 - 1) : (curPos.Item1 - 1, curPos.Item2),
             '7' => prevPos.Item1 != curPos.Item1 ? (curPos.Item1, curPos.Item2 - 1) : (curPos.Item1 + 1, curPos.Item2),
             _ => throw new ArgumentOutOfRangeException(nameof(currentPipe), currentPipe, null)
        };
    }
}

public static class Extensions
{
    public static char? GetValueAt(this char[][] board, int row, int col)
    {
        try
        {
            return board[row][col];
        }
        catch
        {
            return null;
        }
    }
}