using OopMonitoringLab.Models;
using OopMonitoringLab.Services;

namespace OopMonitoringLab.Metrics;

public class InMemoryMetricsCollector : IMetricsCollector
{
    private readonly Dictionary<string, ServiceMetrics> _metricsByService =
        new Dictionary<string, ServiceMetrics>();

    public void RegisterService(IService service)
    {
        if (!_metricsByService.ContainsKey(service.Name))
        {
            _metricsByService[service.Name] = new ServiceMetrics(service.Name);
        }
    }

    public void Record(Request request, Response response)
    {
        if (_metricsByService.TryGetValue(request.ServiceName, out var metrics))
        {
            metrics.Update(response);
        }
    }

    public IReadOnlyCollection<ServiceMetrics> GetCurrentMetrics()
    {
        return _metricsByService.Values;
    }
}