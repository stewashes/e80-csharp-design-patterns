using CSharpCourse.DesignPatterns.Behavioral.Mediator;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.MediatorTests;

public class TicTacToeTests
{
    [Fact]
    public void GamePlaythrough()
    {
        var server = new GameServer();
        var client1 = new GameClient(server);
        var client2 = new GameClient(server);

        Assert.Equal('X', client1.PlayerId);
        Assert.Equal('O', client2.PlayerId);

        // Player X makes first move
        client1.MakeMove(0, 0);
        Assert.Equal('X', client1.LocalBoard[0, 0]);
        Assert.Equal('O', client1.CurrentPlayer);

        // Player O makes move
        client2.MakeMove(1, 1);
        Assert.Equal('O', client2.LocalBoard[1, 1]);
        Assert.Equal('X', client2.CurrentPlayer);

        // Player X makes move
        client1.MakeMove(0, 1);
        Assert.Equal('X', client1.LocalBoard[0, 1]);
        Assert.Equal('O', client1.CurrentPlayer);

        // Player O makes move
        client2.MakeMove(2, 2);
        Assert.Equal('O', client2.LocalBoard[2, 2]);
        Assert.Equal('X', client2.CurrentPlayer);

        // Player X makes winning move
        client1.MakeMove(0, 2);
        Assert.Equal('X', client1.Winner);
        Assert.Equal('X', client2.Winner);
    }

    [Fact]
    public void InvalidMove()
    {
        var server = new GameServer();
        var client1 = new GameClient(server);
        var client2 = new GameClient(server);

        client1.MakeMove(0, 0);  // Valid move
        client2.MakeMove(0, 0);  // Invalid move - same position

        Assert.Equal('X', client1.LocalBoard[0, 0]);
        Assert.Equal('O', client1.CurrentPlayer);  // Turn shouldn't change after invalid move
    }

    [Fact]
    public void Draw()
    {
        var server = new GameServer();
        var client1 = new GameClient(server);
        var client2 = new GameClient(server);

        // Fill board without winning
        client1.MakeMove(0, 0); // X
        client2.MakeMove(0, 1); // O
        client1.MakeMove(0, 2); // X
        client2.MakeMove(1, 0); // O
        client1.MakeMove(1, 1); // X
        client2.MakeMove(2, 0); // O
        client1.MakeMove(1, 2); // X
        client2.MakeMove(2, 2); // O
        client1.MakeMove(2, 1); // X

        Assert.Equal(TicTacToe.Draw, client1.Winner);
        Assert.Equal(TicTacToe.Draw, client2.Winner);
    }
}
