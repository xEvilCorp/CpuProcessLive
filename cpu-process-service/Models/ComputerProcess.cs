namespace cpu_process_service.Models;

public class ComputerProcess {
    public int Id {get; set;}
    public float RamUsage {get; set;}
    public float CpuUsage {get; set;}
    public string? Name {get; set;}
    public string? Group {get; set;} 
    public string? GroupDescription {get; set;} 

    private long _pastCpuTime;
    private long _presentCpuTime;

    public long getPastCpuTime() { return _pastCpuTime; }
    public void setPastCpuTime(long value) =>  _pastCpuTime = value;

    public long getPresentCpuTime() { return _presentCpuTime; }
    public void setPresentCpuTime(long value) => _presentCpuTime = value;

}