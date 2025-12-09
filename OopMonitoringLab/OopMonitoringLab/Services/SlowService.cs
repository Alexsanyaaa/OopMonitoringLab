using System.Threading;
using OopMonitoringLab.Models;

namespace OopMonitoringLab.Services;

public class SlowService : ServiceBase
{
    public SlowService()
        : base("SlowService", baseLatencyMs: 300, failureProbability: 0.05)
    {
    }

    public override Response Process(Request request)
    {
        int jitter = (int)(BaseLatencyMs * 0.3);
        int latency = BaseLatencyMs + Random.Next(-jitter, jitter + 1);
        latency = Math.Max(1, latency);

        //Thread.Sleep(latency);

        bool isSuccess = Random.NextDouble() >= FailureProbability;
        Response response = isSuccess
            ? new Response(true, latency)
            : new Response(false, latency, "ERR_SLOW_FAIL", "Simulated slow service failure");

        Log(request, response);
        return response;
    }
}