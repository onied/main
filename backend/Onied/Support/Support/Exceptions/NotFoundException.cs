using System.Net;

namespace Support.Exceptions;

public class NotFoundException(string message) : HttpResponseException(message, HttpStatusCode.NotFound);
