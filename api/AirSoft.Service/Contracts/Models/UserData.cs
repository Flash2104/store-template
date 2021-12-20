using AirSoft.Data.Entity;

namespace AirSoft.Service.Contracts.Models;

public class UserData
{
    public UserData(Guid id, string? email, string? phone, UserStatus? status, List<ReferenceData<int>>? userRoles)
    {
        Id = id;
        Email = email;
        Phone = phone;
        Status = status;
        UserRoles = userRoles;
    }

    public Guid Id { get; }
    
    public string? Email { get; }

    public string? Phone { get; }

    public UserStatus? Status { get; }

    public List<ReferenceData<int>>? UserRoles { get; }
}