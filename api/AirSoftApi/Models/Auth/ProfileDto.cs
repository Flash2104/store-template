namespace StoreApi.Models.Auth;

public class ProfileDto
{
    public ProfileDto(byte[]? avatarIcon)
    {
        AvatarIcon = avatarIcon;
    }

    public byte[]? AvatarIcon { get; }
}