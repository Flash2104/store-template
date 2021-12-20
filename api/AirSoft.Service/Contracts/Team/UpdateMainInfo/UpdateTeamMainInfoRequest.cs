using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Team.UpdateMainInfo;

public class UpdateTeamMainInfoRequest
{
    public UpdateTeamMainInfoRequest(Guid id, string title, int? cityId, DateTime? foundationDate)
    {
        Id = id;
        Title = title;
        CityId = cityId;
        FoundationDate = foundationDate;
    }

    public Guid Id { get; }

    public string Title { get; set; }

    public int? CityId { get; }

    public DateTime? FoundationDate { get; }

}