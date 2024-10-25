using System.Net;

namespace Support.Exceptions;


public class HttpResponseException : Exception
{
    public HttpStatusCode ResponseStatusCode { get; set; }

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
}
