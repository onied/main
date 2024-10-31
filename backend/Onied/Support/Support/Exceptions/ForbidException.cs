using System.Net;

namespace Support.Exceptions;

public class ForbidException(string message) : HttpResponseException(message, HttpStatusCode.Forbidden);
