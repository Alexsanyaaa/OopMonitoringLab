using OopMonitoringLab.Health;
using OopMonitoringLab.Metrics;
using OopMonitoringLab.Models;
using OopMonitoringLab.Services;
using System.Text;

namespace OopMonitoringLab;

public class Program
{
    private static readonly Random Random = new();
    private const string MetricsFilePath = "metrics.json";

    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        IService fastService = new FastService();
        IService slowService = new SlowService();

        var metricsCollector = new InMemoryMetricsCollector();
        metricsCollector.RegisterService(fastService);
        metricsCollector.RegisterService(slowService);

        var healthEvaluator = new ServiceHealthEvaluator();

        var requests = GenerateRequests(100);

        for (int i = 0; i < requests.Count; i++)
        {
            var request = requests[i];
            var service = request.ServiceName == fastService.Name ? fastService : slowService;

            var response = service.Process(request);
            metricsCollector.Record(request, response);
        }

        Console.WriteLine("Экспорт метрик в файл...");
        MetricsPersistence.SaveToFile(metricsCollector.GetCurrentMetrics(), MetricsFilePath);
        Console.WriteLine($"Метрики сохранены в {MetricsFilePath}");

        Console.WriteLine("Загрузка метрик из файла и отображение:");
        try
        {
            var loadedSnapshots = MetricsPersistence.LoadFromFile(MetricsFilePath);
            foreach (var snapshot in loadedSnapshots)
            {
                var health = new ServiceHealthEvaluator().Evaluate(CreateMetricsFromSnapshot(snapshot));
                Console.WriteLine(
                    $"Сервис: {snapshot.ServiceName} \nЗапросов: {snapshot.TotalRequests}" +
                    $"\nУспехов: {snapshot.SuccessfulRequests} \nОшибок: {snapshot.FailedRequests}" +
                    $"\nСред. задержка: {snapshot.AverageLatencyMs:F1} мс \nМакс.: {snapshot.MaxLatencyMs} мс" +
                    $"\nОшибок: {snapshot.ErrorRate:P1}\nСостояние: {health}"
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке метрик: {ex.Message}");
        }
    }

    private static List<Request> GenerateRequests(int count)
    {
        var requests = new List<Request>();
        string[] services = { "FastService", "SlowService" };

        for (int i = 0; i < count; i++)
        {
            //int fastServiceCheance = Random.Next(1, 100);

            string serviceName = services[Random.Next(services.Length)];
            //string serviceName = fastServiceCheance > 20 ? (fastServiceCheance < 90 ? "FastService" : "SlowService") : "SlowService";
            int payload = Random.Next(10, 1000);
            requests.Add(new Request(serviceName, payload, null));
        }
        return requests;
    }

    private static ServiceMetrics CreateMetricsFromSnapshot(ServiceMetricsSnapshot snapshot)
    {
        var metrics = new ServiceMetrics(snapshot.ServiceName);
        var fakeResponse = new Response(true, (int)snapshot.AverageLatencyMs);
        for (int i = 0; i < snapshot.TotalRequests; i++)
        {
            if (i < snapshot.SuccessfulRequests)
                metrics.Update(new Response(true, (int)snapshot.AverageLatencyMs));
            else
                metrics.Update(new Response(false, (int)snapshot.AverageLatencyMs, "ERR", "Loaded"));
        }
        return metrics;
    }
}