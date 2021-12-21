namespace Store.Service.Contracts.Navigation;

public interface INavigationService
{
    Task<UserNavigationDataResponse> GetUserNavigations();
}