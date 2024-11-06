namespace CSharpCourse.DesignPatterns.Behavioral.Mediator;

// Server-authoritative multiplayer game

internal class TicTacToe
{
    private readonly char[,] _board = new char[3, 3];
    public const char EmptyCell = ' ';
    public const char Draw = 'D';

    public TicTacToe()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                _board[i, j] = EmptyCell;
    }

    public char[,] GetBoard() => (char[,])_board.Clone();

    public bool MakeMove(int row, int col, char player)
    {
        if (row < 0 || row > 2 || col < 0 || col > 2 || _board[row, col] != EmptyCell)
            return false;

        _board[row, col] = player;
        return true;
    }

    public bool CheckWin(char player)
    {
        // Check rows
        for (int i = 0; i < 3; i++)
            if (_board[i, 0] == player && _board[i, 1] == player && _board[i, 2] == player)
                return true;

        // Check columns
        for (int i = 0; i < 3; i++)
            if (_board[0, i] == player && _board[1, i] == player && _board[2, i] == player)
                return true;

        // Check diagonals
        if (_board[0, 0] == player && _board[1, 1] == player && _board[2, 2] == player)
            return true;
        if (_board[0, 2] == player && _board[1, 1] == player && _board[2, 0] == player)
            return true;

        return false;
    }

    public bool IsBoardFull()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (_board[i, j] == EmptyCell)
                    return false;
        return true;
    }
}
