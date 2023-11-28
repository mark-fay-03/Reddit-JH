using Microsoft.AspNetCore.Authentication.OAuth;
using PostRetriever.WorkerService.Services;
using PostRetriever.WorkerService.Wrappers;
using RedditSharp;
using RedditSharp.Things;

namespace PostRetriever.WorkerService;

public static class ServicesRegistrar
{
    public static IServiceCollection AddInternalServices(this IServiceCollection services, IConfiguration contextConfiguration)
    {
        var configurationSection = contextConfiguration.GetSection(nameof(RedditAuthConfig));
        services.Configure<RedditAuthConfig>(configurationSection);
        var redditAuthConfig = configurationSection.Get<RedditAuthConfig>()!;

        InitializeRedditSharp(services, redditAuthConfig);

        services.AddSingleton<IRedditPostProcessor, RedditSharpPostProcessor>();
        services.AddSingleton<IObserver<Post>, PostObserver>();

        return services;
    }

    private static void InitializeRedditSharp(IServiceCollection services, RedditAuthConfig redditAuthConfig)
    {
        services.AddSingleton<IWebAgent>(_ =>
            new BotWebAgent(
                redditAuthConfig.UserName,
                redditAuthConfig.Password,
                redditAuthConfig.ClientId,
                redditAuthConfig.ClientSecret,
                redditAuthConfig.RedirectUri));
        services.AddSingleton<IRedditWrapper>(provider => new RedditWrapper(provider.GetRequiredService<IWebAgent>(), false));
        var webAgentPool = new RefreshTokenWebAgentPool(redditAuthConfig.ClientId, redditAuthConfig.ClientSecret, redditAuthConfig.RedirectUri)
        {
            DefaultRateLimitMode = RateLimitMode.Burst,
            DefaultUserAgent = redditAuthConfig.UserAgent
        };
        services.AddSingleton(webAgentPool);
    }
}