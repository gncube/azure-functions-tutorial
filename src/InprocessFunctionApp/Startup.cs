using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;

[assembly: FunctionsStartup(typeof(InprocessFunctionApp.Startup))]

namespace InprocessFunctionApp;
public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        // Register ILogger<T> as a service
        builder.Services.AddLogging();

        // Add logging providers
        builder.Services.AddSingleton<ILoggerProvider>(sp =>
        {
            var hostConfig = sp.GetRequiredService<IConfiguration>();
            var logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Worker", LogEventLevel.Warning)
            .MinimumLevel.Override("Host", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Error)
            .MinimumLevel.Override("Function", LogEventLevel.Error)
            .MinimumLevel.Override("Azure.Storage.Blobs", LogEventLevel.Error)
            .MinimumLevel.Override("Azure.Core", LogEventLevel.Error)
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}",
                theme: AnsiConsoleTheme.Literate,
                levelSwitch: new LoggingLevelSwitch(LogEventLevel.Information))
            .WriteTo.Debug()
            .WriteTo.File(new JsonFormatter(renderMessage: true),
                $"logs\\{nameof(InprocessFunctionApp)}-.json",
                LogEventLevel.Information,
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

            return new SerilogLoggerProvider(logger, dispose: true);
        });

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog();
        });

        // Get the execution context
        var context = builder.GetContext();

        // Build the configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(context.ApplicationRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Register the configuration as a singleton service
        builder.Services.AddSingleton<IConfiguration>(configuration);
    }
}
