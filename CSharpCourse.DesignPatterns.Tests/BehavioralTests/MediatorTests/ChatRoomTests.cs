using CSharpCourse.DesignPatterns.Behavioral.Mediator;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.MediatorTests;

public class ChatRoomTests
{
    [Fact]
    public void ChatRoom()
    {
        var chatRoom = new ChatRoom();

        var alice = new User("Alice");
        var bob = new User("Bob");
        var charlie = new User("Charlie");

        // The join message is broadcasted to the room (Alice)
        chatRoom.Join(alice);

        Assert.Single(alice.ChatLog);

        // The join message is broadcasted to the room (Alice, Bob)
        chatRoom.Join(bob);

        Assert.Equal(2, alice.ChatLog.Count);
        Assert.Single(bob.ChatLog);

        // Private messages are not broadcasted to the room
        // Users don't hold a reference to other users,
        // they just know their usernames
        alice.SendPrivateMessage(bob.Username, "Hello, Bob!");
        bob.SendPrivateMessage(alice.Username, "Hi, Alice!");

        Assert.Equal(4, alice.ChatLog.Count);
        Assert.Equal(3, bob.ChatLog.Count);

        alice.BroadcastMessage("Hello, everyone!");

        Assert.Equal(5, alice.ChatLog.Count);
        Assert.Equal(4, bob.ChatLog.Count);

        // The join message is broadcasted to the room (Alice, Bob, Charlie)
        chatRoom.Join(charlie);

        Assert.Equal(6, alice.ChatLog.Count);
        Assert.Equal(5, bob.ChatLog.Count);
        Assert.Single(charlie.ChatLog);

        charlie.BroadcastMessage("Hi, everyone!");

        Assert.Equal(7, alice.ChatLog.Count);
        Assert.Equal(6, bob.ChatLog.Count);
        Assert.Equal(2, charlie.ChatLog.Count);
    }
}
