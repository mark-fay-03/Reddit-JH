using RedditSharp.Things;

namespace PostRetriever.WorkerService.Services;

public class PostObserver : IObserver<Post>
{
    private List<Post> _data = new();

    public void OnCompleted()
    {
        _data.Clear();
    }

    public void OnError(Exception error)
    {
        Console.WriteLine(error.ToString());
    }

    public void OnNext(Post value)
    {
        _data.Add(value);
    }
}