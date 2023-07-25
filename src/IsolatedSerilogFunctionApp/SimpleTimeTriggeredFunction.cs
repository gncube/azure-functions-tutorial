using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace IsolatedSerilogFunctionApp;

public class SimpleTimeTriggeredFunction
{
    private readonly ILogger _logger;

    public SimpleTimeTriggeredFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<SimpleTimeTriggeredFunction>();
    }


    [Function("SimpleTimeTriggeredFunction")]
    public void Run([TimerTrigger("*/5 * * * * *")] MyInfo myTimer)
    {
        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        if (myTimer.IsPastDue)
        {
            _logger.LogInformation("Timer is running late!");
        }

        if (myTimer.ScheduleStatus != null)
        {
            _logger.LogWarning($"Last timer schedule was: {myTimer.ScheduleStatus.Last}");

            _logger.LogError($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            _logger.LogInformation($"Last timer schedule was updated at: {myTimer.ScheduleStatus.LastUpdated}");
        }

        _logger.LogInformation($"---> LogInformation function completed at: {DateTime.Now}");
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
