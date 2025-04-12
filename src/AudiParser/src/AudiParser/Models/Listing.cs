using System.Text.Json;
using System.Text.Json.Serialization;

namespace AudiParser.Models;

public class Listing
{
    [JsonPropertyName("audiCode")]
    public required string AudiCode { get; set; }

    [JsonPropertyName("availableFromDate")]
    public required long AvailableFromDate { get; set; }

    [JsonPropertyName("bodyType")]
    public required CodeWithDescription BodyType { get; set; }

    [JsonPropertyName("carId")]
    public required string CarId { get; set; }

    [JsonPropertyName("dateOffer")]
    public required long DateOffer { get; set; }

    [JsonPropertyName("dealer")]
    public required Dealer Dealer { get; set; }

    [JsonPropertyName("driveType")]
    public required CodeWithDescription DriveType { get; set; }

    [JsonPropertyName("extColor")]
    public required CodeWithDescription ExtColor { get; set; }

    [JsonPropertyName("fuel")]
    public required CodeWithDescription Fuel { get; set; }

    [JsonPropertyName("gearType")]
    public required CodeWithDescription GearType { get; set; }

    [JsonPropertyName("lastChange")]
    public required long LastChange { get; set; }

    [JsonPropertyName("mbvHandbook")]
    public required CodeWithDescription MbvHandbook { get; set; }

    [JsonPropertyName("modelYear")]
    public required int ModelYear { get; set; }

    [JsonPropertyName("symbolicCarline")]
    public required CodeWithDescription SymbolicCarline { get; set; }

    [JsonPropertyName("symbolicCarlineGroup")]
    public required CodeWithDescription SymbolicCarlineGroup { get; set; }

    [JsonPropertyName("trimline")]
    public required CodeWithDescription Trimline { get; set; }

    [JsonPropertyName("typedPrices")]
    public required List<TypedPrice> TypedPrices { get; set; }

    [JsonPropertyName("used")]
    public required Used Used { get; set; }

    [JsonPropertyName("vin")]
    public required string Vin { get; set; }

    [JsonPropertyName("weblink")]
    public required string Weblink { get; set; }
}

public class CodeWithDescription
{
    [JsonPropertyName("code")]
    public required string Code { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

public class Dealer
{
    [JsonPropertyName("city")]
    public required string City { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("street")]
    public required string Street { get; set; }

    [JsonPropertyName("zipCode")]
    public required string ZipCode { get; set; }
}

public class TypedPrice
{
    [JsonPropertyName("amount")]
    public required double Amount { get; set; }
}

public class Used
{
    [JsonPropertyName("mileage")]
    public required double Mileage { get; set; }

    [JsonPropertyName("mileageUnit")]
    public required string MileageUnit { get; set; }
}

public class VehicleData
{
    [JsonPropertyName("vehicleBasic")]
    public required List<Listing> VehicleBasic { get; set; }
}

