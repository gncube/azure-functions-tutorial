using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace InprocessFunctionApp;

public class MyFunction
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MyFunction> _logger;

    public MyFunction(IConfiguration configuration, ILogger<MyFunction> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [FunctionName("MyFunction")]
    public void Run([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        // Access configuration values
        var mySetting = _configuration["MySetting"];
        var connectionString = _configuration.GetConnectionString("Database");

        if (mySetting != null)
        {
            _logger.LogWarning(mySetting.ToString());
        }

        if (connectionString != null)
        {
            connectionString = connectionString.Trim();
            _logger.LogError(connectionString);
        }
    }
}
