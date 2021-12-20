using CsvHelper.Configuration.Attributes;

namespace AirSoft.Data.InitialData.RuAddressCities;

public class AddressData
{
    [Name("address")]
    public string CityAddress { get; set; } = null!;

    [Name("country")]
    public string Country { get; set; } = null!;

    [Name("postal_code")]
    public string PostalCode { get; set; } = null!;

    [Name("federal_district")]
    public string? FederalDistrict { get; set; }

    [Name("region_type")]
    public string RegionType { get; set; } = null!;

    [Name("region")]
    public string Region { get; set; } = null!;

    [Name("area_type")]
    public string? AreaType { get; set; }

    [Name("area")]
    public string? Area { get; set; }

    [Name("city")]
    public string? City { get; set; }

    [Name("city_type")]
    public string CityType { get; set; } = null!;

    [Name("timezone")]
    public string TimeZone { get; set; } = null!;
}
