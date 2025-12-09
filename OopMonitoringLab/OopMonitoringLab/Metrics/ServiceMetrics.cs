using OopMonitoringLab.Models;

namespace OopMonitoringLab.Metrics;

public class ServiceMetrics
{
    public string ServiceName { get; }
    public int TotalRequests { get; private set; }
    public int SuccessfulRequests { get; private set; }
    public int FailedRequests { get; private set; }

    private double _totalLatency;
    public double AverageLatencyMs { get; private set; }
    public int MaxLatencyMs { get; private set; }

    public ServiceMetrics(string serviceName)
    {
        ServiceName = serviceName;
    }

    public double ErrorRate =>
        TotalRequests == 0 ? 0.0 : (double)FailedRequests / TotalRequests;

    public void Update(Response response)
    {
        TotalRequests++;
        if (response.IsSuccess)
            SuccessfulRequests++;
        else
            FailedRequests++;

        _totalLatency += response.LatencyMs;
        AverageLatencyMs = _totalLatency / TotalRequests;
        if (response.LatencyMs > MaxLatencyMs)
            MaxLatencyMs = response.LatencyMs;
    }
}