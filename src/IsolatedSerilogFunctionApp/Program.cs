using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;

var host = new HostBuilder()
    .ConfigureLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddJsonConsole();
    })
    .UseSerilog((hostingContext, loggerConfiguration) =>
    {
        loggerConfiguration
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Worker", LogEventLevel.Warning)
            .MinimumLevel.Override("Host", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Error)
            .MinimumLevel.Override("Function", LogEventLevel.Error)
            .MinimumLevel.Override("Azure.Storage.Blobs", LogEventLevel.Error)
            .MinimumLevel.Override("Azure.Core", LogEventLevel.Error)
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}",
                theme: AnsiConsoleTheme.Sixteen)

            .WriteTo.Debug()
            .WriteTo.ApplicationInsights(TelemetryConfiguration.CreateDefault(), TelemetryConverter.Traces)
            .WriteTo.File(new JsonFormatter(renderMessage: true),
                $"logs\\{nameof(IsolatedSerilogFunctionApp)}-.json",
                LogEventLevel.Information,
                rollingInterval: RollingInterval.Day);
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
