﻿using Microsoft.Extensions.DependencyInjection.Extensions;
using PostRetriever.WorkerService.Services;
using PostRetriever.WorkerService.Wrappers;
using Reddit.Data.Contracts;
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

        services.TryAddSingleton<IRedditPostProcessor, RedditSharpPostProcessor>();
        services.TryAddSingleton<IObserver<Post>, PostObserver>();

        services.TryAddTransient(typeof(ILoggerWrapper<>), typeof(LoggerWrapper<>));
        services.TryAddSingleton<IDateTimeWrapper, DateTimeWrapper>();
        services.TryAddSingleton<IConsoleWrapper, ConsoleWrapper>();

        services.TryAddSingleton<IMapper<Post, IRedditPost>, PostMapper>();

        return services;
    }

    private static void InitializeRedditSharp(IServiceCollection services, RedditAuthConfig redditAuthConfig)
    {
        services.TryAddSingleton<IWebAgent>(_ =>
            new BotWebAgent(
                redditAuthConfig.UserName,
                redditAuthConfig.Password,
                redditAuthConfig.ClientId,
                redditAuthConfig.ClientSecret,
                redditAuthConfig.RedirectUri));
        services.TryAddSingleton<IRedditWrapper>(provider => new RedditWrapper(provider.GetRequiredService<IWebAgent>(), false));
        var webAgentPool = new RefreshTokenWebAgentPool(redditAuthConfig.ClientId, redditAuthConfig.ClientSecret, redditAuthConfig.RedirectUri)
        {
            DefaultRateLimitMode = RateLimitMode.Burst,
            DefaultUserAgent = redditAuthConfig.UserAgent
        };
        services.TryAddSingleton(webAgentPool);
    }
}