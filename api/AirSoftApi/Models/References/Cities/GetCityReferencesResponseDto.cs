using Store.Service.Contracts.References.Cities;

namespace StoreApi.Models.References.Cities;

public class GetCityReferencesResponseDto
{
    public List<RegionReferenceData> Regions { get; }

    public GetCityReferencesResponseDto(List<RegionReferenceData> regions)
    {
        Regions = regions;
    }
}