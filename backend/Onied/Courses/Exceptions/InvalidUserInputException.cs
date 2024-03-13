namespace Courses.Exceptions;

public class InvalidUserInputException : Exception
{
    public InvalidUserInputException(string message) : base(message) {}
}