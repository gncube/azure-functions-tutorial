using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace IsolatedSerilogFunctionApp;

public class SimpleFunction
{
    private readonly ILogger _logger;

    public SimpleFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<SimpleFunction>();
    }

    [Function("SimpleFunction")]
    public HttpResponseData Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        response.WriteString("Welcome to Azure Functions!");

        return response;
    }
}
