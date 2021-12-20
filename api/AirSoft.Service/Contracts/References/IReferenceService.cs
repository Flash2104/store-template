using AirSoft.Service.Contracts.References.Cities;

namespace AirSoft.Service.Contracts.References;

public interface IReferenceService
{
    Task<GetCityReferencesResponse> GetCities();
}