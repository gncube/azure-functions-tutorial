using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace IsolatedFunctionApp;

public class SimpleTimeFunction
{
    private readonly ILogger _logger;

    public SimpleTimeFunction(ILogger<SimpleTimeFunction> logger)
    {
        _logger = logger;
    }

    [Function("SimpleTimeFunction")]
    public void Run([TimerTrigger("0 */1 * * * *")] MyInfo myTimer)
    {
        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        _logger.LogDebug($"---> LogDebug function completed at: {DateTime.Now}");
        _logger.LogError($"---> LogError function completed at: {DateTime.Now}");
        _logger.LogCritical($"---> LogCritical function completed at: {DateTime.Now}");
        _logger.LogWarning($"---> LogWarning function completed at: {DateTime.Now}");
        _logger.LogTrace($"---> LogTrace function completed at: {DateTime.Now}");
    }
}

public class MyInfo
{
    public MyScheduleStatus ScheduleStatus { get; set; }

    public bool IsPastDue { get; set; }
}

public class MyScheduleStatus
{
    public DateTime Last { get; set; }

    public DateTime Next { get; set; }

    public DateTime LastUpdated { get; set; }
}
