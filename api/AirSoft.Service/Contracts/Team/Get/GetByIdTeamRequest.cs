using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Team.Get;

public class GetByIdTeamRequest
{
    public GetByIdTeamRequest(string id)
    {
        Id = id;
    }
    
    public string Id { get; }
}