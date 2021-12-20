using AirSoft.Service.Contracts.References.Cities;

namespace AirSoftApi.Models.References.Cities;

public class GetCityReferencesResponseDto
{
    public List<RegionReferenceData> Regions { get; }

    public GetCityReferencesResponseDto(List<RegionReferenceData> regions)
    {
        Regions = regions;
    }
}