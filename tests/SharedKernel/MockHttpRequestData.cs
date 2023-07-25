using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Security.Claims;

namespace SharedKernel;
public class MockHttpRequestData : HttpRequestData
{
    public MockHttpRequestData(FunctionContext functionContext, Uri url, Stream body) : base(functionContext)
    {
        Url = url;
        Body = body ?? new MemoryStream();
    }

    public override HttpResponseData CreateResponse()
    {
        return new MockHttpResponseData(FunctionContext);
    }

    public override Stream Body { get; } = new MemoryStream();

    public override HttpHeadersCollection Headers { get; } = new HttpHeadersCollection();

    public override IReadOnlyCollection<IHttpCookie> Cookies { get; }

    public override Uri Url { get; }

    public override IEnumerable<ClaimsIdentity> Identities { get; }

    public override string Method { get; }
}
