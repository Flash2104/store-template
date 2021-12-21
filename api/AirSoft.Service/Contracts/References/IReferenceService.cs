using Store.Service.Contracts.References.Cities;

namespace Store.Service.Contracts.References;

public interface IReferenceService
{
    Task<GetCityReferencesResponse> GetCities();
}