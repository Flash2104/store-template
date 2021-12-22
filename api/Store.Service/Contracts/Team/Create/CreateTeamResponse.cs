
namespace Store.Service.Contracts.Team.Create;

public class CreateTeamResponse
{
    public CreateTeamResponse(TeamData? teamData)
    {
        TeamData = teamData;
    }
    
    public TeamData? TeamData { get; }
}