
namespace AirSoft.Service.Contracts.Auth.SignIn;

public class SignInRequest
{
    public SignInRequest(string phoneOrEmail, string password)
    {
        PhoneOrEmail = phoneOrEmail;
        Password = password;
    }
    
    public string PhoneOrEmail { get; }

    public string Password { get; }
}