using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using cpu_process_service.Interfaces;
using cpu_process_service.Models;
using NickStrupat;

namespace cpu_process_service;

public class ComputerProcessing : IComputerProcessing
{
    private List<ComputerProcess> processList;
    private PerformanceCounter totalCpuTime;
    private float totalRamCapacity;
    private float totalCpuCurrentTime;

    const float BYTES_TO_MB = 1048576.1f;

    private int[] ids;
    private long[] ramByteValues;
    private long[] cpuTimeValues;

    public ComputerProcessing()
    {
        ids = new int[0];
        ramByteValues = cpuTimeValues = new long[0];
        processList = new List<ComputerProcess>();
        totalCpuTime = new PerformanceCounter("Process", "% Processor Time", "_Total");
        totalRamCapacity = new ComputerInfo().TotalPhysicalMemory / BYTES_TO_MB;
    }


    //Returns the total reports with the process list. 
    public ComputerData GetComputerData() {
        ComputerData cd = new ComputerData();
        cd.TotalRamUsage = GetTotalRamUsage();
        cd.Processes = GetProcessList();
        cd.TotalCpuUsage = cd.Processes.Sum(item => item.CpuUsage);
        return cd;
    }

    //Calculates the % of the current used ram / total capacity.
    public float GetTotalRamUsage() {
        float totalRamRemaining = new ComputerInfo().AvailablePhysicalMemory / BYTES_TO_MB;
        float totalRamUsed = totalRamCapacity - totalRamRemaining; 
        return (totalRamUsed/totalRamCapacity)*100;
    }

    //Returns an updated list with all current processes and their current data.
    public List<ComputerProcess> GetProcessList()
    {
        UpdateRawValues();
        for (int i = 0; i < ids.Length; i++)
        {
            ComputerProcess? cp = FindProcess(i);
            bool processAlreadyExists = cp is not null;

            if (processAlreadyExists)
            {
                UpdateProcess(cp, i);
            }
            else
            {
                AddProcess(i);
            }
            RemoveProcesses();
        }
        return processList;
    }

    //Returns the process if it found it in the process list by its id using the current index.
    private ComputerProcess? FindProcess(int index)
    {
        int processId = ids[index];
        ComputerProcess? cp = processList.Find(o => o.Id == processId);
        return cp;
    }

    //Updates the values of a process already in the process list.
    private void UpdateProcess(ComputerProcess cp, int index)
    {
        cp.Id = ids[index];
        cp.RamUsage = ramByteValues[index] / BYTES_TO_MB;
        cp.setPastCpuTime(cp.getPresentCpuTime());
        cp.setPresentCpuTime(cpuTimeValues[index]);
        cp.CpuUsage = (cp.getPresentCpuTime() - cp.getPastCpuTime()) / ((totalCpuCurrentTime + 0.01f) * 1000);
    }

    //Adds a new process to the process list. 
    private void AddProcess(int index)
    {
        if (ids[index] != 0)
        {
            Process p = Process.GetProcessById(ids[index]);

            var cp = new ComputerProcess();
            cp.Id = ids[index];
            cp.Group = p.ProcessName;
            cp.GroupDescription = GetProcessDescription(p);
            cp.Name = GetProcessName(p);
            cp.RamUsage = ramByteValues[index] / BYTES_TO_MB;
            cp.setPresentCpuTime(cpuTimeValues[index]);
            processList.Add(cp);
        }
    }

    //Tries to get the process name in the following priority:
    //1º Window Name | 2º Description Name | 3º Process Code Name
    private string GetProcessName(Process process) {
        string name = process.MainWindowTitle;
        if(name.Trim() == "") {
            name = GetProcessDescription(process);
        }
        return name;
    }

    //Tries to get the process description through its file description.
    //It doesn't return satysting results for windows hosted services. 
    private string GetProcessDescription(Process process)
    {
        try
        {
            
            string name = process.MainModule.FileVersionInfo.FileDescription.Trim();
            if (name == "") { name = process.ProcessName; }
            return name;
        }
        catch
        {
            return process.ProcessName;
        }
    }

    //Removes all processes from the list that no longer are present in the process arrays. 
    private void RemoveProcesses()
    {
        processList.RemoveAll(item => !ids.Contains(item.Id));
    }

    //Updates the arrays with the current process performance values.
    private void UpdateRawValues()
    {
        var procs = new PerformanceCounterCategory("Process").ReadCategory();
        totalCpuCurrentTime = totalCpuTime.NextValue();
        ids = GetInstanceValues(ref procs, "Id Process").Select(i => (int)i).ToArray();
        ramByteValues = GetInstanceValues(ref procs, "Working Set - Private");
        cpuTimeValues = GetInstanceValues(ref procs, "% Processor Time");
    }

    //Converts the process collections from System.Diagnostics into simple arrays with just the raw values.
    private long[] GetInstanceValues(ref InstanceDataCollectionCollection processes, string instanceName)
    {
        var instanceCollection = processes[instanceName].Values;
        var instanceArray = new InstanceData[instanceCollection.Count];
        instanceCollection.CopyTo(instanceArray, 0);
        long[] instanceValues = instanceArray.Select(i => i.RawValue).ToArray();
        return instanceValues;
    }

  
}



