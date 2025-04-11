using System.Text.Json;
using System.Text.Json.Serialization;

namespace ListingLib
{
    public class Listing
    {
        [JsonPropertyName("audiCode")]
        public string AudiCode { get; set; }

        [JsonPropertyName("availableFromDate")]
        public long AvailableFromDate { get; set; }

        [JsonPropertyName("bodyType")]
        public CodeWithDescription BodyType { get; set; }

        [JsonPropertyName("carId")]
        public string CarId { get; set; }

        [JsonPropertyName("dateOffer")]
        public long DateOffer { get; set; }

        [JsonPropertyName("dealer")]
        public Dealer Dealer { get; set; }

        [JsonPropertyName("driveType")]
        public CodeWithDescription DriveType { get; set; }

        [JsonPropertyName("extColor")]
        public CodeWithDescription ExtColor { get; set; }

        [JsonPropertyName("fuel")]
        public CodeWithDescription Fuel { get; set; }

        [JsonPropertyName("gearType")]
        public CodeWithDescription GearType { get; set; }

        [JsonPropertyName("lastChange")]
        public long LastChange { get; set; }

        [JsonPropertyName("mbvHandbook")]
        public CodeWithDescription MbvHandbook { get; set; }

        [JsonPropertyName("modelYear")]
        public int ModelYear { get; set; }

        [JsonPropertyName("symbolicCarline")]
        public CodeWithDescription SymbolicCarline { get; set; }

        [JsonPropertyName("symbolicCarlineGroup")]
        public CodeWithDescription SymbolicCarlineGroup { get; set; }

        [JsonPropertyName("trimline")]
        public CodeWithDescription Trimline { get; set; }

        [JsonPropertyName("typedPrices")]
        public List<TypedPrice> TypedPrices { get; set; }

        [JsonPropertyName("used")]
        public Used Used { get; set; }

        [JsonPropertyName("vin")]
        public string Vin { get; set; }

        [JsonPropertyName("weblink")]
        public string Weblink { get; set; }
    }

    public class CodeWithDescription
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class Dealer
    {
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("zipCode")]
        public string ZipCode { get; set; }
    }

    public class TypedPrice
    {
        [JsonPropertyName("amount")]
        public double Amount { get; set; }
    }

    public class Used
    {
        [JsonPropertyName("mileage")]
        public double Mileage { get; set; }

        [JsonPropertyName("mileageUnit")]
        public string MileageUnit { get; set; }
    }

    public class VehicleData
    {
        [JsonPropertyName("vehicleBasic")]
        public List<Listing> VehicleBasic { get; set; }
    }
}