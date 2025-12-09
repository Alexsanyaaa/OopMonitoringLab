using OopMonitoringLab.Models;
using OopMonitoringLab.Services;

namespace OopMonitoringLab.Metrics;

public interface IMetricsCollector
{
    void RegisterService(IService service);//регистрация для сбора метрик
    void Record(Request request, Response response);//сохранение результатов
    IReadOnlyCollection<ServiceMetrics> GetCurrentMetrics();//Вернуть сводку
}