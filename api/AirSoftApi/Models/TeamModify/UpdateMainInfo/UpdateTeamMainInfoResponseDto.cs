using System.ComponentModel.DataAnnotations;
using AirSoft.Service.Contracts.Team;

namespace AirSoftApi.Models.TeamModify.UpdateMainInfo;

public class UpdateTeamMainInfoResponseDto
{
    public UpdateTeamMainInfoResponseDto(TeamData? teamData)
    {
        TeamData = teamData;
    }

    public TeamData? TeamData { get; }
}