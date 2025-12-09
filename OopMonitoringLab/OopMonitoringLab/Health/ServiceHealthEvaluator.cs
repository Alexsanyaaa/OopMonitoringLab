using OopMonitoringLab.Metrics;

namespace OopMonitoringLab.Health;

public class ServiceHealthEvaluator
{
    public double MaxHealthyErrorRate { get; set; } = 0.05;
    public double MaxDegradedErrorRate { get; set; } = 0.20;

    public int MaxHealthyLatencyMs { get; set; } = 150;
    public int MaxDegradedLatencyMs { get; set; } = 400;

    public ServiceHealth Evaluate(ServiceMetrics metrics)
    {
        bool errorRateHealthy = metrics.ErrorRate <= MaxHealthyErrorRate;
        bool errorRateDegraded = metrics.ErrorRate <= MaxDegradedErrorRate;

        bool latencyHealthy = metrics.AverageLatencyMs <= MaxHealthyLatencyMs;
        bool latencyDegraded = metrics.AverageLatencyMs <= MaxDegradedLatencyMs;

        if (errorRateHealthy && latencyHealthy)
            return ServiceHealth.Healthy;
        else if (errorRateDegraded && latencyDegraded)
            return ServiceHealth.Degraded;
        else
            return ServiceHealth.Unhealthy;
    }
}