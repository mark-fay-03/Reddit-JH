using RedditSharp;
using RedditSharp.Things;

namespace PostRetriever.WorkerService.Wrappers;

public interface IListingStreamWrapper<T>
{
    IDisposable Subscribe(IObserver<T> observer);
    Task Enumerate(CancellationToken cancellationToken);
}

public class ListingStreamWrapper<T> : IListingStreamWrapper<T> where T : Thing
{
    private readonly ListingStream<T> _listingStream;

    public ListingStreamWrapper(ListingStream<T> listingStream)
    {
        _listingStream = listingStream;
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        return _listingStream.Subscribe(observer);
    }

    public Task Enumerate(CancellationToken cancellationToken)
    {
        return _listingStream.Enumerate(cancellationToken);
    }
}