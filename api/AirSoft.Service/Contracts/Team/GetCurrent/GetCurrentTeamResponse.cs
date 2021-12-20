
namespace AirSoft.Service.Contracts.Team.GetCurrent;

public class GetCurrentTeamResponse
{
    public GetCurrentTeamResponse(TeamData? teamData)
    {
        TeamData = teamData;
    }
    
    public TeamData? TeamData { get; }
} 