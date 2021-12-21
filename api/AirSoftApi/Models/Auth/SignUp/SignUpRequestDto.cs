using System.ComponentModel.DataAnnotations;

namespace StoreApi.Models.Auth.SignUp;

public class SignUpRequestDto: IValidatableObject
{
    public SignUpRequestDto(string phoneOrEmail, string password, string confirmPassword)
    {
        PhoneOrEmail = phoneOrEmail;
        Password = password;
        ConfirmPassword = confirmPassword;
    }

    [Required]
    public string PhoneOrEmail { get; }

    [Required]
    public string Password { get; }

    [Required]
    public string ConfirmPassword { get; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return new List<ValidationResult>();
    }
}