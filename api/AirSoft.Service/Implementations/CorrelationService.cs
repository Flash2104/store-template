using Store.Service.Contracts;

namespace Store.Service.Implementations;

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