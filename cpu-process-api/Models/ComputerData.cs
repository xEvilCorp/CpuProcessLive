namespace cpu_process_api.Models;

public class ComputerData {
    public float TotalRamUsage {get; set;}
    public float TotalCpuUsage {get; set;}
    public List<ComputerProcess> Processes {get; set;}
}