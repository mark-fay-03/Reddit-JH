using Microsoft.Extensions.DependencyInjection;

namespace Reddit.Data.Contracts;

public static class DataContractsRegistrar
{
    public static IServiceCollection AddDataContractDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IRedditPosts, RedditPosts>();
        return services;
    }
}