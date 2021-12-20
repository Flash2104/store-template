namespace AirSoft.Service.Contracts.References.Cities;

public class GetCityReferencesRequest
{
    public string CountryIsoCode { get; }

    public GetCityReferencesRequest(string countryIsoCode)
    {
        CountryIsoCode = countryIsoCode;
    }
}