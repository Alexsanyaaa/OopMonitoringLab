using System;
using OopMonitoringLab.Models;

namespace OopMonitoringLab.Services;

public abstract class ServiceBase : IService
{
    public string Name { get; protected set; }
    public int BaseLatencyMs { get; protected set; }
    public double FailureProbability { get; protected set; }

    protected static readonly Random Random = new();//Для генерации сбоев, задержки

    protected ServiceBase(string name, int baseLatencyMs, double failureProbability)
    {
        Name = name;
        BaseLatencyMs = baseLatencyMs;
        FailureProbability = failureProbability;
    }

    public abstract Response Process(Request request);//Логика, которая реализуется наследниками 

    protected virtual void Log(Request request, Response response)
    {
        string status = response.IsSuccess ? "SUCCESS" : "FAILED";
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] " + $"{Name.PadRight(11)} | " + $"{status.PadRight(7)} | " 
            + $"Payload: {request.PayloadSize,3}B | " + $"Latency: {response.LatencyMs,3}ms" 
            + (response.ErrorCode != null ? $" | Error: {response.ErrorCode}" : ""));
    }
}