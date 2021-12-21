using Store.Service.Contracts.Models;

namespace Store.Service.Contracts.Team.Get;

public class GetByIdTeamRequest
{
    public GetByIdTeamRequest(string id)
    {
        Id = id;
    }
    
    public string Id { get; }
}