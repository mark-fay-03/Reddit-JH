namespace PostRetriever.WorkerService.Wrappers;

public interface ILoggerWrapper<T>
{
    void LogError(Exception exception, string? message, params object[]? args);
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
}