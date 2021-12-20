using AirSoft.Service.Contracts;

namespace AirSoft.Service.Implementations;

public class CorrelationService: ICorrelationService
{
    private Guid? _userId = null;

    public Guid? GetUserId()
    {
        return _userId;
    }

    public void SetUserId(Guid? userId)
    {
        _userId = userId;
    }
}