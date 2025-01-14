using System.Net;

namespace Storage.Exceptions;

public class ForbidException(string message) : HttpResponseException(message, HttpStatusCode.Forbidden);
