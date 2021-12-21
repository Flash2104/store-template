namespace Store.Service.Contracts.Auth;

public class AuthProfileData
{
    public AuthProfileData(byte[]? avatarIcon)
    {
        AvatarIcon = avatarIcon;
    }

    public byte[]? AvatarIcon { get; }
}