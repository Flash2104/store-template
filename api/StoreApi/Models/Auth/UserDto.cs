namespace StoreApi.Models.Auth;

public class UserDto
{
    public UserDto(int id, string? email, string? phone)
    {
        Id = id;
        Email = email;
        Phone = phone;
    }

    public int Id { get; }

    public string? Email { get; }

    public string? Phone { get; }
}