namespace AirSoft.Service.Contracts.References.Cities;

public class GetCityReferencesResponse
{
    public List<RegionReferenceData> Regions { get; }

    public GetCityReferencesResponse(List<RegionReferenceData> regions)
    {
        Regions = regions;
    }
}

public class RegionReferenceData
{
    public int Id { get; set; }

    public string Title { get; set; }

    public List<CityReferenceData> Cities { get; }

    public RegionReferenceData(int id, string title, List<CityReferenceData> cities)
    {
        Id = id;
        Title = title;
        Cities = cities;
    }
}

public class CityReferenceData
{
    public int Id { get; }

    public string CityAddress { get; }

    public string? FederalDistrict { get; }

    public string City { get; }

    public CityReferenceData(int id, string cityAddress, string? federalDistrict, string city)
    {
        CityAddress = cityAddress;
        FederalDistrict = federalDistrict;
        City = city;
        Id = id;
    }
}