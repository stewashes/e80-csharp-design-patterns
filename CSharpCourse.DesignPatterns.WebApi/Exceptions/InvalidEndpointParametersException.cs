namespace CSharpCourse.DesignPatterns.WebApi.Exceptions;

public class InvalidEndpointParametersException : Exception
{
    public InvalidEndpointParametersException(string message) : base(message)
    {

    }
}
