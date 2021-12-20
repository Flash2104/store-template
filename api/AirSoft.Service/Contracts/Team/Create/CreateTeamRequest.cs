using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Team.Create;

public class CreateTeamRequest
{
    public CreateTeamRequest(string title, int? cityId, DateTime? foundationDate, byte[]? avatar)
    {
        Title = title;
        CityId = cityId;
        FoundationDate = foundationDate;
        Avatar = avatar;
    }

    public string Title { get; set; }

    public int? CityId { get; }

    public DateTime? FoundationDate { get; }

    public byte[]? Avatar { get; }
}