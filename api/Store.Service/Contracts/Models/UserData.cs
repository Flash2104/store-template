using Store.Data.Entity;

namespace Store.Service.Contracts.Models;

public class UserData
{
    public UserData(int id, string? email, string? phone, UserStatus? status, List<ReferenceData<int>>? userRoles)
    {
        Id = id;
        Email = email;
        Phone = phone;
        Status = status;
        UserRoles = userRoles;
    }

    public int Id { get; }
    
    public string? Email { get; }

    public string? Phone { get; }

    public UserStatus? Status { get; }

    public List<ReferenceData<int>>? UserRoles { get; }
}