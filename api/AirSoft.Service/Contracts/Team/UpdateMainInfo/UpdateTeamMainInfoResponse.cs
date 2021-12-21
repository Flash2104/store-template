
namespace Store.Service.Contracts.Team.UpdateMainInfo;

public class UpdateTeamMainInfoResponse
{
    public UpdateTeamMainInfoResponse(TeamData? teamData)
    {
        TeamData = teamData;
    }
    
    public TeamData? TeamData { get; }
}