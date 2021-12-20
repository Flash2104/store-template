using System.ComponentModel.DataAnnotations;
using AirSoft.Service.Contracts.Models;

namespace AirSoftApi.Models.TeamModify.UpdateMainInfo;

public class UpdateTeamMainInfoRequestDto: IValidatableObject
{
    public UpdateTeamMainInfoRequestDto(Guid id, string title, int? cityId, DateTime? foundationDate)
    {
        Id = id;
        Title = title;
        CityId = cityId;
        FoundationDate = foundationDate;
    }

    [Required]
    public Guid Id { get; }

    [Required]
    public string Title { get; set; }

    public int? CityId { get; }

    public DateTime? FoundationDate { get; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }
}