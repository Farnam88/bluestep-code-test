using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace apitests.ExternalServiceTests.Stubs;

public sealed class StubHttpMessageHandler : HttpMessageHandler
{
    public required Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> Responder { get; init; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var result = Responder(request, ct);
        return Task.FromResult(result);
    }
        
}