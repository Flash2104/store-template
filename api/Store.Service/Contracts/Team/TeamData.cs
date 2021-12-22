using Store.Service.Contracts.Models;

namespace Store.Service.Contracts.Team;

public class TeamData
{
    public TeamData(Guid id, string? title, string? city, DateTime? foundationDate, byte[]? avatar, List<MemberViewData>? members, List<ReferenceData<Guid>>? teamRoles)
    {
        Id = id;
        Title = title;
        Avatar = avatar;
        Members = members;
        TeamRoles = teamRoles;
        City = city;
        FoundationDate = foundationDate;
    }

    public Guid Id { get; }

    public string? Title { get; set; }

    public string? City { get; }

    public DateTime? FoundationDate { get; }

    public byte[]? Avatar { get; }

    public List<ReferenceData<Guid>>? TeamRoles { get; }

    public List<MemberViewData>? Members { get; }
}

public class MemberViewData
{
    public MemberViewData(
        Guid id,
        string? name,
        string? surname,
        string? city,
        string? about,
        byte[]? avatar,
        bool? isLeader,
        List<ReferenceData<Guid>>? roles
        )
    {
        Id = id;
        Name = name;
        Surname = surname;
        Avatar = avatar;
        IsLeader = isLeader;
        Roles = roles;
        City = city;
        About = about;
    }

    public Guid Id { get; }

    public string? Name { get; set; }

    public string? Surname { get; }

    public string? City { get; }

    public string? About { get; }

    public byte[]? Avatar { get; }

    public bool? IsLeader { get; }

    public List<ReferenceData<Guid>>? Roles { get; }
}