using cpu_process_api.Models;

namespace cpu_process_api.Interfaces;

public interface IProcessInterface
{
    Task ReceiveData(ComputerData message);
}