using AirSoft.Service.Contracts.Navigation;

namespace AirSoftApi.Models.Navigation;

public class UserNavigationResponseDto
{
    public List<UserNavigationData> Navigations { get; }

    public UserNavigationResponseDto(List<UserNavigationData>? navigations)
    {
        Navigations = navigations ?? new List<UserNavigationData>();
    }
}