using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Globalization;
using System.Reflection;
using Store.Data.InitialData.RuAddressCities;

namespace Store.Data.Entity;

public class DbRuCity
{
    public int Id { get; set; }

    public string CityAddress { get; set; } = null!;

    public string? Country { get; set; }

    public string CountryIsoCode { get; set; } = null!;

    public string? PostalCode { get; set; }

    public string? FederalDistrict { get; set; }

    public string RegionType { get; set; } = null!;

    public string? Region { get; set; }

    public string? AreaType { get; set; }

    public string? Area { get; set; }

    public string City { get; set; } = null!;

    public string? CityType { get; set; }

    public string? TimeZone { get; set; }

}

internal sealed class DbRuCityMapping
{
    public void Map(EntityTypeBuilder<DbRuCity> builder)
    {
        builder.ToTable("RuCities");

        builder.HasKey(x => new { x.Id });
        builder.Property(f => f.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.City).IsRequired().HasMaxLength(50);
        builder.Property(x => x.CityAddress).IsRequired().HasMaxLength(255);
        string? root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        using var reader = new StreamReader(root + "\\InitialData\\RuAddressCities\\city.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<AddressData>().Select((x, index) => new DbRuCity()
        {
            Id = index + 1,
            Area = x.Area,
            AreaType = x.AreaType,
            City = !string.IsNullOrWhiteSpace(x.City)
                ? x.City
                : (x.RegionType == "г" ? x.Region : (x.CityAddress.Split("г ")[1]))
            ,
            CityAddress = x.CityAddress,
            CityType = x.CityType,
            Country = x.Country,
            CountryIsoCode = "RUS",
            FederalDistrict = x.FederalDistrict,
            PostalCode = x.PostalCode,
            Region = x.Region,
            RegionType = x.RegionType,
            TimeZone = x.TimeZone
        }).ToList();
        builder.HasData(records);
    }
}