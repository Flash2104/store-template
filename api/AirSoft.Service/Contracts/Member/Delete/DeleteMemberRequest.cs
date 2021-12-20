
namespace AirSoft.Service.Contracts.Member.Delete;

public class DeleteMemberRequest
{
    public DeleteMemberRequest(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}