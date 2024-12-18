using System.Net;

namespace Storage.Exceptions;

public class BadRequestException(string message) : HttpResponseException(message, HttpStatusCode.BadRequest);
