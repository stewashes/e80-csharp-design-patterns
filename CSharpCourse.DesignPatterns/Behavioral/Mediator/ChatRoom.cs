namespace CSharpCourse.DesignPatterns.Behavioral.Mediator;

internal record ChatMessage
{
    public string Sender { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public ChatMessage(string sender, string message)
    {
        Sender = sender;
        Message = message;
    }
}

internal class User
{
    public string Username { get; }
    public ChatRoom? Room { get; set; }
    public List<ChatMessage> ChatLog { get; } = [];

    public User(string username)
    {
        Username = username;
    }

    // The user doesn't add the message to the chat log directly
    // but instead sends it to the room, which then calls ReceiveMessage
    // on the user.
    public void BroadcastMessage(string message)
        => Room?.Broadcast(Username, message);

    public void SendPrivateMessage(string recipient, string message)
        => Room?.SendMessage(Username, recipient, message);

    public void ReceiveMessage(string sender, string message)
    {
        var chatMessage = new ChatMessage(sender, message);

        ChatLog.Add(chatMessage);
    }
}

// This is the mediator
internal class ChatRoom
{
    private readonly List<User> _users = [];

    public void Join(User user)
    {
        var joinMessage = $"{user.Username} joined the chatroom";
        user.Room = this;
        _users.Add(user);
        Broadcast("room", joinMessage);
    }

    public void Broadcast(string sender, string message)
    {
        // Notify everybody that a new message was sent
        foreach (var user in _users)
        {
            user.ReceiveMessage(sender, message);
        }
    }

    public void SendMessage(string sender, string recipient, string message)
    {
        // Notify both participants that a new message was sent
        _users
            .Find(u => u.Username == recipient)?
            .ReceiveMessage(sender, message);

        _users
            .Find(u => u.Username == sender)?
            .ReceiveMessage(sender, message);
    }
}
