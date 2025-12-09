using System.Text.Json;
using System.Text.Json.Serialization;

namespace OopMonitoringLab.Metrics;

public static class MetricsPersistence
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault//подавляет значения по умолчанию(например 0 для инта)
    };

    public static void SaveToFile(IReadOnlyCollection<ServiceMetrics> metrics, string filePath)
    {
        var snapshots = metrics.Select(m => new ServiceMetricsSnapshot
        {
            ServiceName = m.ServiceName,
            TotalRequests = m.TotalRequests,
            SuccessfulRequests = m.SuccessfulRequests,
            FailedRequests = m.FailedRequests,
            AverageLatencyMs = m.AverageLatencyMs,
            MaxLatencyMs = m.MaxLatencyMs,
            ErrorRate = m.ErrorRate
        }).ToList();

        string json = JsonSerializer.Serialize(snapshots, Options);
        File.WriteAllText(filePath, json);
    }

    public static List<ServiceMetricsSnapshot> LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл метрик не найден: {filePath}");

        string json = File.ReadAllText(filePath);
        var snapshots = JsonSerializer.Deserialize<List<ServiceMetricsSnapshot>>(json, Options);
        return snapshots ?? new List<ServiceMetricsSnapshot>();
    }
}