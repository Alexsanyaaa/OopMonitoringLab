namespace OopMonitoringLab.Metrics;

public class ServiceMetricsSnapshot//чистый DTO-класс
{
    public string ServiceName { get; set; } = string.Empty;
    public int TotalRequests { get; set; }
    public int SuccessfulRequests { get; set; }
    public int FailedRequests { get; set; }
    public double AverageLatencyMs { get; set; }
    public int MaxLatencyMs { get; set; }
    public double ErrorRate { get; set; }
}