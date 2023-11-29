using PostRetriever.WorkerService;
using Reddit.Data.Contracts;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddInternalServices(context.Configuration);
        services.AddDataContractDependencies();
        
        services.AddLogging();
        services.AddHostedService<PostsProcessorService>();
    })
    .Build();

host.Run();
