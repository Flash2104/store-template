using AirSoft.Service.Contracts.Team;

namespace AirSoftApi.Models.Team.Create;

public class CreateTeamResponseDto
{
    public CreateTeamResponseDto(TeamData? teamData)
    {
        TeamData = teamData;
    }

    public TeamData? TeamData { get; }
}