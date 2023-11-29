using PostRetriever.WorkerService.Services;
using Reddit.Data.Contracts;
using RedditSharp.Things;

namespace StatisticsAPI;

public static class ServiceRegistrar
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IPostsService, PostsService>();
        services.AddSingleton<IMapper<Post, IRedditPost>, PostMapper>();

        return services;
    }
}