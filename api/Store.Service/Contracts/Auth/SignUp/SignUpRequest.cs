namespace Store.Service.Contracts.Auth.SignUp;

public class SignUpRequest
{
    public SignUpRequest(string phoneOrEmail, string password, string confirmPassword)
    {
        PhoneOrEmail = phoneOrEmail;
        Password = password;
        ConfirmPassword = confirmPassword;
    }

    public string PhoneOrEmail { get; }

    public string Password { get; }

    public string ConfirmPassword { get; }
}