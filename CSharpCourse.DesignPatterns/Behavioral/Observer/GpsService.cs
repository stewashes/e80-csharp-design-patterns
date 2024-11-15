using System.Threading.Channels;

namespace CSharpCourse.DesignPatterns.Behavioral.Observer;

internal readonly record struct Coordinates(
    Guid DeviceId,
    double Latitude,
    double Longitude);
