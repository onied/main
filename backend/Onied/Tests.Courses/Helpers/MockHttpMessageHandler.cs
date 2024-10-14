using System.Net;
using System.Text;
using System.Text.Json;
using Courses.Dtos;

namespace Tests.Courses.Helpers;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCode;
    private readonly SubscriptionRequestDto _responseContent;

    public MockHttpMessageHandler(HttpStatusCode statusCode, SubscriptionRequestDto responseContent = null!)
    {
        _statusCode = statusCode;
        _responseContent = responseContent;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(_statusCode);

        response.Content = new StringContent(JsonSerializer.Serialize(_responseContent), Encoding.UTF8, "application/json");

        return Task.FromResult(response);
    }
}
