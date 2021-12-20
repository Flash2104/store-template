using AirSoft.Service.Contracts.Models;

namespace AirSoftApi.Models.Member;

public class MemberDataDto
{
    public MemberDataDto(
        Guid id, 
        string? name, 
        string? surname, 
        DateTime? birthDate, 
        string? city, 
        string? email, 
        string? phone, 
        byte[]? avatar, 
        byte[]? avatarIcon, 
        ReferenceData<Guid>? team, 
        List<ReferenceData<Guid>>? roles
        )
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        Phone = phone;
        AvatarIcon = avatarIcon;
        AvatarData = avatar != null ? Convert.ToBase64String(avatar) : null;
        Team = team;
        Roles = roles;
        BirthDate = birthDate;
        City = city;
    }

    public Guid Id { get; }

    public string? Name { get; set; }

    public string? Surname { get; }

    public DateTime? BirthDate { get; }

    public string? City { get; }

    public string? Email { get; }

    public string? Phone { get; }

    public byte[]? AvatarIcon { get; }

    public List<ReferenceData<Guid>>? Roles { get; }

    public string? AvatarData { get; }

    public ReferenceData<Guid>? Team { get; }
}