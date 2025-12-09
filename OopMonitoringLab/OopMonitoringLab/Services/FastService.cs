using System.Threading;
using OopMonitoringLab.Models;

namespace OopMonitoringLab.Services;

public class FastService : ServiceBase
{
    public FastService()
        : base("FastService", baseLatencyMs: 20, failureProbability: 0.05)
    {
    }

    public override Response Process(Request request)
    {
        int jitter = (int)(BaseLatencyMs * 0.2);
        int latency = BaseLatencyMs + Random.Next(-jitter, jitter + 1);
        latency = Math.Max(1, latency);

        //Thread.Sleep(latency);

        bool isSuccess = Random.NextDouble() >= FailureProbability;//5% на фейл
        Response response = isSuccess
            ? new Response(true, latency)
            : new Response(false, latency, "ERR_FAST_FAIL", "Simulated fast service failure");

        Log(request, response);
        return response;
    }
}