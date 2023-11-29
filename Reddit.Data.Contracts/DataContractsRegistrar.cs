using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Reddit.Data.Contracts;

public static class DataContractsRegistrar
{
    public static IServiceCollection AddDataContractDependencies(this IServiceCollection services)
    {
        services.TryAddSingleton<IRedditPosts, RedditPosts>();
        return services;
    }
}