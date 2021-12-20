
namespace AirSoft.Service.Contracts.Team.Delete;

public class DeleteTeamRequest
{
    public DeleteTeamRequest(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}