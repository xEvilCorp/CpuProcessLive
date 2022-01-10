namespace cpu_process_api.Models;

public class ComputerProcess {
    public int Id {get; set;}
    public float RamUsage {get; set;}
    public float CpuUsage {get; set;}
    public string? Name {get; set;}
    public string? Group {get; set;} 
    public string? GroupDescription {get; set;} 
}