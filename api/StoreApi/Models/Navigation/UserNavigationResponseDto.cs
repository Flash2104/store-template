using Store.Service.Contracts.Navigation;

namespace StoreApi.Models.Navigation;

public class UserNavigationResponseDto
{
    public List<UserNavigationData> Navigations { get; }

    public UserNavigationResponseDto(List<UserNavigationData>? navigations)
    {
        Navigations = navigations ?? new List<UserNavigationData>();
    }
}