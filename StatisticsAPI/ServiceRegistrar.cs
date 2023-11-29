using Microsoft.Extensions.DependencyInjection.Extensions;
using Reddit.Data.Contracts;
using StatisticsAPI.Mappers;
using StatisticsAPI.Models;
using StatisticsAPI.Services;

namespace StatisticsAPI;

public static class ServiceRegistrar
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services)
    {
        services.TryAddScoped(typeof(ILoggerWrapper<>), typeof(LoggerWrapper<>));
        services.TryAddSingleton<IPostsService, PostsService>();
        services.TryAddSingleton<IUsersService, UsersService>();
        services.TryAddSingleton<IMapper<Post, IRedditPost>, PostMapper>();

        return services;
    }
}