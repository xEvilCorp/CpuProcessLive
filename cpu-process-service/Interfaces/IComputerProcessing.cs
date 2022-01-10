using cpu_process_service.Models;

namespace cpu_process_service.Interfaces;

public interface IComputerProcessing
{
    List<ComputerProcess> GetProcessList();
    ComputerData GetComputerData();

}