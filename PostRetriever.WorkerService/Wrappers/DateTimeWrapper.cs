namespace PostRetriever.WorkerService.Wrappers;

public interface IDateTimeWrapper
{
    DateTime UtcNow { get; }
}
public class DateTimeWrapper : IDateTimeWrapper
{
    public DateTime UtcNow => DateTime.UtcNow;
}