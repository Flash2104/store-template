namespace AirSoftApi.Models.Auth;

public class UserDto
{
    public UserDto(Guid id, string? email, string? phone)
    {
        Id = id;
        Email = email;
        Phone = phone;
    }

    public Guid Id { get; }

    public string? Email { get; }

    public string? Phone { get; }
}