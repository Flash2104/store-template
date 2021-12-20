namespace AirSoft.Service.Contracts.Navigation;

public interface INavigationService
{
    Task<UserNavigationDataResponse> GetUserNavigations();
}