using System.Text.Json.Serialization;

namespace ListingLib
{
  public class Listing
  {
    [JsonPropertyName("availableFromDate")]
    public int AvailableFromDate { get; set; }

    [JsonPropertyName("carId")]
    public string CarId { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("dealerName")]
    public string DealerName { get; set; }

    [JsonPropertyName("dealerPostCode")]
    public string DealerPostCode { get; set; }

    [JsonPropertyName("fuel")]
    public string Fuel { get; set; }

    [JsonPropertyName("gearbox")]
    public string Gearbox { get; set; }

    [JsonPropertyName("lastUpdated")]
    public long LastUpdated { get; set; }

    [JsonPropertyName("mileage")]
    public double Mileage { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("vin")]
    public string Vin { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }
  }
}