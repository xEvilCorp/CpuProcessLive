using cpu_process_service;
using cpu_process_service.Api;
using cpu_process_service.Interfaces;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<ILiveProcessApi, LiveProcessApi>();
        services.AddSingleton<IComputerProcessing, ComputerProcessing>();
    })
    .Build();

await host.RunAsync();
