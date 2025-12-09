namespace OopMonitoringLab.Models;

public class Response
{
    public bool IsSuccess { get; set; }
    public int LatencyMs { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }

    public Response(bool isSuccess, int latencyMs, string? errorCode = null, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        LatencyMs = latencyMs;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}