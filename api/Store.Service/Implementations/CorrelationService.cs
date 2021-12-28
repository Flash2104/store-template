using Store.Service.Contracts;

namespace Store.Service.Implementations;

public class CorrelationService: ICorrelationService
{
    private int? _userId = null;

    public int? GetUserId()
    {
        return _userId;
    }

    public void SetUserId(int? userId)
    {
        _userId = userId;
    }
}