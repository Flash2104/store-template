using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Member.Get;

public class GetByIdMemberRequest
{
    public GetByIdMemberRequest(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }
}