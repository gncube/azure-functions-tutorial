using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace InprocessFunctionApp;

public class VonageEvent
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public VonageEvent(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        _configuration = configuration;
        _logger = loggerFactory.CreateLogger<VonageEvent>();
    }

    [FunctionName("VonageEvent")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "event")] HttpRequest req)
    {
        _logger.LogInformation($"---> {nameof(VonageEvent)} function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        string status = data?.status;

        _logger.LogWarning(status);

        if (data.status != null)
        {
            switch ((string)data.status)
            {
                case "ringing":
                    RecordSteps($"UUID: {data.conversation_uuid} - ringing.", _logger);
                    break;
                case "answered":
                    RecordSteps($"UUID: {data.conversation_uuid} - was answered.", _logger);
                    break;
                case "machine":
                    RecordSteps($"UUID: {data.conversation_uuid} - answering machine.", _logger);
                    break;
                case "complete":
                    RecordSteps($"UUID: {data.conversation_uuid} - complete.", _logger);
                    break;
            }
        }

        if (data.dtmf != null)
        {
            switch ((string)data.dtmf)
            {
                case "1":
                    RecordSteps($"UUID: {data.conversation_uuid} - confirmed receipt.", _logger);
                    break;
                default:
                    RecordSteps($"UUID: {data.conversation_uuid} - other button pressed ({data.dtmf}).", _logger);
                    break;
            }
        }

        string responseMessage = string.IsNullOrEmpty(status)
            ? $"unsuccessfully, add a valid JSON payload in the request body for a complete response. {requestBody}"
            : $"successfully: {requestBody}";


        return new OkObjectResult($"Processed a call event {responseMessage}");
    }

    private static void RecordSteps(string message, ILogger log)
    {
        log.LogInformation(message);
    }
}
