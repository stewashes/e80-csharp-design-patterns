namespace CSharpCourse.DesignPatterns.Structural.Decorator;

/*
 * Some languages like C# and Java do not support multiple inheritance.
 * You can implement multiple interfaces but not inherit from multiple base types.
 * We can get around this by using the decorator pattern.
 */

internal interface IAudioPlayer
{
    void PlayAudio();
    string Name { get; set; }
}

internal class AudioPlayer : IAudioPlayer
{
    public void PlayAudio() => Console.WriteLine("Playing audio track...");
    public string Name { get; set; } = string.Empty;
}

internal interface IVideoPlayer
{
    void PlayVideo();
    string Name { get; set; }
}

internal class VideoPlayer : IVideoPlayer
{
    public void PlayVideo() => Console.WriteLine("Playing video content...");
    public string Name { get; set; } = string.Empty;
}

// Implement the two interfaces
internal class MultimediaPlayer : IAudioPlayer, IVideoPlayer
{
    // Keep a reference to the objects we are trying to "inherit" from
    private readonly AudioPlayer _audioPlayer = new();
    private readonly VideoPlayer _videoPlayer = new();
    private string _name = string.Empty;

    // Call the methods on the correct base instance
    public void PlayAudio() => _audioPlayer.PlayAudio();
    public void PlayVideo() => _videoPlayer.PlayVideo();

    // If both interfaces implement the same thing, re-implement it
    // once to satisfy both and keep everything synchronized.
    public string Name
    {
        get => _name;
        set
        {
            _audioPlayer.Name = value;
            _videoPlayer.Name = value;
            _name = value;
        }
    }
}