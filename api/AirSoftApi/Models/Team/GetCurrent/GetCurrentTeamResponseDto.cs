using AirSoft.Service.Contracts.Team;

namespace AirSoftApi.Models.Team.GetCurrent;

public class GetCurrentTeamResponseDto
{
    public GetCurrentTeamResponseDto(TeamData? teamData)
    {
        TeamData = teamData;
    }

    public TeamData? TeamData { get; }
}