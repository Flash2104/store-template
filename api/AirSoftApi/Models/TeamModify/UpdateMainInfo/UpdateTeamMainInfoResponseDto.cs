using System.ComponentModel.DataAnnotations;
using Store.Service.Contracts.Team;

namespace StoreApi.Models.TeamModify.UpdateMainInfo;

public class UpdateTeamMainInfoResponseDto
{
    public UpdateTeamMainInfoResponseDto(TeamData? teamData)
    {
        TeamData = teamData;
    }

    public TeamData? TeamData { get; }
}