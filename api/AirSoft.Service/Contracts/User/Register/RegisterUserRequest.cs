namespace AirSoft.Service.Contracts.User.Register;

public class RegisterUserRequest
{
    public RegisterUserRequest(string phoneOrEmail, string password, string confirmPassword)
    {
        PhoneOrEmail = phoneOrEmail;
        Password = password;
        ConfirmPassword = confirmPassword;
    }

    public string PhoneOrEmail { get; }

    public string Password { get; }

    public string ConfirmPassword { get; }
}