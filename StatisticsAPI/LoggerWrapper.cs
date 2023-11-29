namespace StatisticsAPI;

public interface ILoggerWrapper<T>
{
    void LogError(Exception exception, string? message, params object[]? args);
    void LogInfo(string message, params object[]? args);
}

public class LoggerWrapper<T>: ILoggerWrapper<T>
{
    private readonly ILogger<T> _logger;

    public LoggerWrapper(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogError(Exception exception, string? message, params object[]? args)
    {
        _logger.LogError(exception, message, args);
    }

    public void LogInfo(string message, params object[]? args)
    {
        _logger.LogInformation(message, args);
    }
}