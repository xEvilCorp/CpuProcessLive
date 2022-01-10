using System.Diagnostics;
using cpu_process_service.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace cpu_process_service.Api;


public class LiveProcessApi : ILiveProcessApi
{
    private HubConnection _connection;
    private readonly IConfiguration _configuration;

    public LiveProcessApi(IConfiguration configuration)
    {
        _configuration = configuration;
        Connect();
    }

    public HubConnectionState ConnectionState() {
        return _connection.State;
    }

    private async void Connect()
    {
        try
        {
            string url = _configuration.GetSection("ApiURL").Get<string>();
            _connection = new HubConnectionBuilder().WithUrl(url).Build();
            await _connection.StartAsync();

            if (_connection.State == HubConnectionState.Connected)
                Console.WriteLine("Connection to the Api established.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Connection to the Api failed.");
            Console.WriteLine(e.Message);
        }

    }

    public async void SendData(object data)
    {
        if (_connection.State == HubConnectionState.Connected)
        {
            try
            {
                await _connection.InvokeAsync("SendData", data);
                Console.WriteLine($"Updated data sent at {DateTime.Now}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
        else
        {
            Console.WriteLine("There is an issue with the connection.");
            Console.WriteLine("Connection State: " + _connection.State);
        }
    }
}