using Serilog;

namespace Free_Internet;

public class LoggerManager
{
    public LoggerManager()
    {
        Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Fatal,
                    outputTemplate:
                    "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level}] [{SourceContext}] [{Message}{NewLine}" +
                    "{Exception}{NewLine}-----------------------------------------------------]{NewLine}")
                .CreateLogger();
        
        _logger = Log.Logger;
    }
    
    private readonly ILogger _logger;

    public void LogDebug(string message) => _logger.Debug(message);

    public void LogInfo(string message) => _logger.Information(message);

    public void LogWarn(string message) => _logger.Warning(message);

    public void LogError(string message) => _logger.Error(message);

    public void LogError(Exception exception, string message) => _logger.Error(exception, message);

    public void LogFatal(string message) => _logger.Fatal(message);

    public void LogFatal(Exception exception, string message) => _logger.Fatal(exception, message);

    public void LogDebug(string message, params object[] args) => _logger.Debug(message, args);

    public void LogInfo(string message, params object[] args) => _logger.Information(message, args);

    public void LogWarn(string message, params object[] args) => _logger.Warning(message, args);

    public void LogError(string message, params object[] args) => _logger.Error(message, args);

    public void LogError(Exception exception, string message, params object[] args) =>
        _logger.Error(exception, message, args);

    public void LogFatal(string message, params object[] args) => _logger.Fatal(message, args);

    public void LogFatal(Exception exception, string message, params object[] args) => 
        _logger.Fatal(exception, message, args);
}

