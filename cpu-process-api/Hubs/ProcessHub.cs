using Microsoft.AspNetCore.SignalR;
using cpu_process_api.Interfaces;
using cpu_process_api.Models;

namespace cpu_process_api.Hubs;

public class ProcessHub : Hub<IProcessInterface>
{
    public async Task SendData(ComputerData message)
    {
        await Clients.All.ReceiveData(message);
    }
}
