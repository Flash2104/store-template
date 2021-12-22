using Microsoft.Extensions.Logging;
using Store.Service.Contracts;
using Store.Service.Contracts.References;
using Store.Service.Contracts.References.Cities;
using static System.Char;

namespace Store.Service.Implementations.References;

public class ReferenceService : IReferenceService
{
    private readonly ILogger<ReferenceService> _logger;
    private readonly ICorrelationService _correlationService;
    private readonly IDataService _dataService;

    public ReferenceService(ILogger<ReferenceService> logger, ICorrelationService correlationService, IDataService dataService)
    {
        _logger = logger;
        _correlationService = correlationService;
        _dataService = dataService;
    }

    public async Task<GetCityReferencesResponse> GetCities()
    {
        var dbCities = await _dataService.Cities.ListAsync();
        return new GetCityReferencesResponse(dbCities
            .GroupBy(x => new { x.Region, x.RegionType })
            .Select((x, index) => new RegionReferenceData(
                index + 1,
                IsUpper(x.Key.RegionType, 0) ? $"{x.Key.RegionType} {x.Key.Region}" : $"{x.Key.Region} {x.Key.RegionType}",
                x.Select(y => new CityReferenceData(y.Id, y.CityAddress, y.FederalDistrict, y.City)).ToList()
            )).ToList());
    }
}