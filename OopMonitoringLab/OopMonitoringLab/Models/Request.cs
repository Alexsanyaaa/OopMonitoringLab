namespace OopMonitoringLab.Models;

public class Request
{
    public string ServiceName { get; } = string.Empty;
    public int PayloadSize { get; }
    public int? DeadlineMs { get; }

    public Request(string serviceName, int payloadSize, int? deadlineMs = null)
    {
        ServiceName = serviceName;
        PayloadSize = payloadSize;
        DeadlineMs = deadlineMs;
    }
}