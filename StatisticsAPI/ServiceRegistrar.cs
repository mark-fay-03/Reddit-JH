using PostRetriever.WorkerService.Services;
using Reddit.Data.Contracts;
using RedditSharp.Things;
using StatisticsAPI.Services;

namespace StatisticsAPI;

public static class ServiceRegistrar
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services)
    {
        services.AddScoped(typeof(ILoggerWrapper<>), typeof(LoggerWrapper<>));
        services.AddSingleton<IPostsService, PostsService>();
        services.AddSingleton<IMapper<Post, IRedditPost>, PostMapper>();

        return services;
    }
}