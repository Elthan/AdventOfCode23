namespace AdventOfCode;

internal static class Extensions
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