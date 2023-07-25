using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;

var host = new HostBuilder()
    .ConfigureLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddSerilog();
    })
    .UseSerilog(logger: new LoggerConfiguration()
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .MinimumLevel.Override("Worker", LogEventLevel.Warning)
        .MinimumLevel.Override("Host", LogEventLevel.Warning)
        .MinimumLevel.Override("Function", LogEventLevel.Warning)
        .MinimumLevel.Override("Azure.Storage.Blobs", LogEventLevel.Error)
        .MinimumLevel.Override("Azure.Core", LogEventLevel.Error)
        .Enrich.FromLogContext()
        .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}",
                theme: AnsiConsoleTheme.Literate,
                levelSwitch: new LoggingLevelSwitch(LogEventLevel.Information))

        .WriteTo.Debug()

        .WriteTo.File(new JsonFormatter(renderMessage: true),
                $"logs\\{nameof(IsolatedFunctionApp)}-.json",
                LogEventLevel.Information,
                rollingInterval: RollingInterval.Day)

        .CreateLogger())
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
