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
    public void Run([TimerTrigger("0 */5 * * * * ")] TimerInfo myTimer)
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

        _logger.LogInformation($"{_configuration} function completed at: {DateTime.Now}");

        //_logger.LogInformation($"---> LogInformation function completed at: {DateTime.Now}");
        _logger.LogDebug($"---> LogDebug function completed at: {DateTime.Now}");
        _logger.LogError($"---> LogError function completed at: {DateTime.Now}");
        _logger.LogCritical($"---> LogCritical function completed at: {DateTime.Now}");
        _logger.LogWarning($"---> LogWarning function completed at: {DateTime.Now}");
        _logger.LogTrace($"---> LogTrace function completed at: {DateTime.Now}");
    }
}
