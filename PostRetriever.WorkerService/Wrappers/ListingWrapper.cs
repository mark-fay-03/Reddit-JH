using RedditSharp;
using RedditSharp.Things;

namespace PostRetriever.WorkerService.Wrappers;

public interface IListingWrapper<T> where T : Thing
{
    IListingStreamWrapper<T> Stream();
}

public class ListingWrapper<T> : IListingWrapper<T> where T : Thing
{
    private readonly Listing<T> _listing;

    public ListingWrapper(Listing<T> listing)
    {
        _listing = listing;
    }

    public IListingStreamWrapper<T> Stream()
    {
        return new ListingStreamWrapper<T>(_listing.Stream());
    }
}