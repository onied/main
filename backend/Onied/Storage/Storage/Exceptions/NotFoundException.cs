using System.Net;

namespace Storage.Exceptions;

public class NotFoundException(string message) : HttpResponseException(message, HttpStatusCode.NotFound);
