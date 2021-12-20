using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AirSoftApi.Models.Auth.SignIn;

public class SignInRequestDto: IValidatableObject
{
    public SignInRequestDto(string phoneOrEmail, string password)
    {
        PhoneOrEmail = phoneOrEmail;
        Password = password;
    }
    
    [Required]
    public string PhoneOrEmail { get; }

    [Required]
    public string Password { get; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return new List<ValidationResult>();
    }
}