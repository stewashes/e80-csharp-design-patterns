using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
