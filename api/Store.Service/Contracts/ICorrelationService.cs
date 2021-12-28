using Store.Service.Contracts.Models;

namespace Store.Service.Contracts;

public interface ICorrelationService
{
    int? GetUserId();
    void SetUserId(int? userId);
}