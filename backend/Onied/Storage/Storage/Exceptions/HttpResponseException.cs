using System.Net;

namespace Storage.Exceptions;

public class HttpResponseException : Exception
{
    public HttpResponseException()
    {
    }

    public HttpResponseException(string message)
    {
    }

    public HttpResponseException(string message, HttpStatusCode statusCode)
        : base(message)
    {
        ResponseStatusCode = statusCode;
    }

    public HttpStatusCode ResponseStatusCode { get; set; }
}
