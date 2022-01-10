using System.Diagnostics;
using cpu_process_service.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace cpu_process_service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private IComputerProcessing _computerProcessing;
    private ILiveProcessApi _liveProcessApi;

    public Worker(ILogger<Worker> logger, IComputerProcessing computerProcessing, ILiveProcessApi liveProcessApi)
    {
        _computerProcessing= computerProcessing;
        _logger = logger;
        _liveProcessApi = liveProcessApi;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try {
                //var proc = _computerData.GetProcessList();
                //Console.WriteLine($"Id: {proc.Id} | Name: {proc.Name} | Ram(MB): {proc.RamUsage}");

                _liveProcessApi.SendData(_computerProcessing.GetComputerData());
            } catch (Exception e) {
                Console.WriteLine("ERROR: " + e.Message);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }

  
}
