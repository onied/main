using System.Net;

namespace Support.Exceptions;

public class BadRequestException(string message) : HttpResponseException(message, HttpStatusCode.BadRequest);
