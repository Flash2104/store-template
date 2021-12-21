using Store.Service.Contracts.Models;

namespace Store.Service.Contracts;

public interface ICorrelationService
{
    Guid? GetUserId();
    void SetUserId(Guid? userId);
}