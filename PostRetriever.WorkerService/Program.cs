using PostRetriever.WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddInternalServices(context.Configuration);

        services.AddHostedService<PostsProcessorService>();
    })
    .Build();

host.Run();
