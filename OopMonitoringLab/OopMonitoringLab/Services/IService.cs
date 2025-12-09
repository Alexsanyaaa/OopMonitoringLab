using OopMonitoringLab.Models;

namespace OopMonitoringLab.Services;

public interface IService
{
    string Name { get; }
    int BaseLatencyMs { get; }//время обработки 
    double FailureProbability { get; }//Вероятнность сбоя сервиса 

    Response Process(Request request);
}