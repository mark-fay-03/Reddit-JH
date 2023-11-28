namespace PostRetriever.WorkerService.Wrappers;

public interface IConsoleWrapper
{
    void WriteLine(string message);
}
public class ConsoleWrapper : IConsoleWrapper
{
    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
}