using CSharpCourse.DesignPatterns.Structural.Decorator;
using System.Reflection;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.DecoratorTests;

public class MultimediaPlayerTests
{
    [Fact]
    public void MultimediaPlayer()
    {
        var player = new MultimediaPlayer();
        player.PlayAudio();
        player.PlayVideo();

        player.Name = "Multimedia Player";

        // Make sure both the audio and video player have the same name
        // through reflection
        var audioPlayer = (AudioPlayer)
            typeof(MultimediaPlayer)
            .GetField("_audioPlayer", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(player)!;
        
        var audioPlayerName = (string)
            typeof(AudioPlayer)
            .GetProperty(nameof(AudioPlayer.Name))!
            .GetValue(audioPlayer)!;

        var videoPlayer = (VideoPlayer)
            typeof(MultimediaPlayer)
            .GetField("_videoPlayer", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(player)!;

        var videoPlayerName = (string)
            typeof(VideoPlayer)
            .GetProperty(nameof(VideoPlayer.Name))!
            .GetValue(videoPlayer)!;

        Assert.Equal(player.Name, audioPlayerName);
        Assert.Equal(player.Name, videoPlayerName);
    }
}
