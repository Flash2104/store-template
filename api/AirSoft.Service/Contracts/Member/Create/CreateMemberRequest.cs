using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Member.Create;

public class CreateMemberRequest
{
    public CreateMemberRequest(Guid userId, string? name = null, string? surname = null, DateTime? birthDate = null, string? city = null)
    {
        UserId = userId;
        Name = name;
        Surname = surname;
        BirthDate = birthDate;
        City = city;
    }

    public Guid UserId { get; }

    public string? Name { get; set; }

    public string? Surname { get; }
    
    public DateTime? BirthDate { get; }

    public string? City { get; }
}