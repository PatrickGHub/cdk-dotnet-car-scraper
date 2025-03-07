using System.Text.Json.Serialization;

namespace ListingLib
{
  public class Listing
  {
    [JsonPropertyName("availableFromDate")]
    public required int AvailableFromDate { get; set; }

    [JsonPropertyName("carId")]
    public required string CarId { get; set; }

    [JsonPropertyName("date")]
    public required string Date { get; set; }

    [JsonPropertyName("dealerName")]
    public required string DealerName { get; set; }

    [JsonPropertyName("dealerPostCode")]
    public required string DealerPostCode { get; set; }

    [JsonPropertyName("fuel")]
    public required string Fuel { get; set; }

    [JsonPropertyName("gearbox")]
    public required string Gearbox { get; set; }

    [JsonPropertyName("lastUpdated")]
    public required long LastUpdated { get; set; }

    [JsonPropertyName("mileage")]
    public required double Mileage { get; set; }

    [JsonPropertyName("model")]
    public required string Model { get; set; }

    [JsonPropertyName("price")]
    public required decimal Price { get; set; }

    [JsonPropertyName("vin")]
    public required string Vin { get; set; }

    [JsonPropertyName("year")]
    public required int Year { get; set; }
  }
}