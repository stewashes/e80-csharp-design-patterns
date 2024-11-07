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

// The client is dumb and only sends messages to the server.
// Its state is updated by the server.
internal class GameClient
{
    private readonly GameServer _server;
    public char[,] LocalBoard { get; private set; } = new char[3, 3];
    public char PlayerId { get; private set; }
    public char CurrentPlayer { get; private set; }
    public char Winner { get; private set; }

    public GameClient(GameServer server)
    {
        _server = server;
        PlayerId = server.Register(this);
    }

    public void MakeMove(int row, int col)
    {
        if (CurrentPlayer == PlayerId)
        {
            _server.SendMove(this, row, col);
        }
    }

    public void UpdateGameState(char[,] board, char currentPlayer)
    {
        LocalBoard = (char[,])board.Clone();
        CurrentPlayer = currentPlayer;
    }

    public void GameOver(char winner)
    {
        Winner = winner;
    }
}

internal class GameServer
{
    private readonly TicTacToe _game = new();
    private readonly List<GameClient> _clients = [];
    private char _currentPlayer = 'X';
    private bool _gameOver;

    public GameServer()
    {
        _gameOver = false;
    }

    // Returns the player id
    public char Register(GameClient client)
    {
        _clients.Add(client);
        NotifyGameState(_game.GetBoard(), _currentPlayer);

        return _clients.Count switch
        {
            1 => 'X',
            2 => 'O',
            _ => throw new InvalidOperationException("Too many players")
        };
    }

    public void SendMove(GameClient client, int row, int col)
    {
        if (_gameOver)
        {
            return;
        }

        if (_game.MakeMove(row, col, _currentPlayer))
        {
            if (_game.CheckWin(_currentPlayer))
            {
                _gameOver = true;
                NotifyGameOver(_currentPlayer);
                return;
            }

            if (_game.IsBoardFull())
            {
                _gameOver = true;
                NotifyGameOver(TicTacToe.Draw);
                return;
            }

            _currentPlayer = _currentPlayer == 'X' ? 'O' : 'X';
            NotifyGameState(_game.GetBoard(), _currentPlayer);
        }
    }

    public void NotifyGameState(char[,] board, char currentPlayer)
    {
        foreach (var client in _clients)
        {
            client.UpdateGameState(board, currentPlayer);
        }
    }

    public void NotifyGameOver(char winner)
    {
        foreach (var client in _clients)
        {
            client.GameOver(winner);
        }
    }
}
