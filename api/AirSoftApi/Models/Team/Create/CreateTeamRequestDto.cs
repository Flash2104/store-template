using System.ComponentModel.DataAnnotations;

namespace AirSoftApi.Models.Team.Create;

public class CreateTeamRequestDto
{
    public CreateTeamRequestDto(string title, int? cityId, DateTime? foundationDate, byte[]? avatar)
    {
        Title = title;
        CityId = cityId;
        FoundationDate = foundationDate;
        Avatar = avatar;
    }
    
    [Required]
    public string Title { get; set; }

    public int? CityId { get; }

    public DateTime? FoundationDate { get; }

    public byte[]? Avatar { get; }
}