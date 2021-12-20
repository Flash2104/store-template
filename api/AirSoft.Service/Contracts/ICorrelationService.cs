using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts;

public interface ICorrelationService
{
    Guid? GetUserId();
    void SetUserId(Guid? userId);
}