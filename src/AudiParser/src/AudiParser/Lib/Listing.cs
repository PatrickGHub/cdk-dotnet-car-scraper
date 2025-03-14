using System.Text.Json.Serialization;

namespace ListingLib
{
  public class Listing
  {
    public required long AvailableFromDate { get; set; }

    public required string CarId { get; set; }

    public required string Date { get; set; }

    public required string DealerName { get; set; }

    public required string DealerPostCode { get; set; }

    public required string Fuel { get; set; }

    public required string Gearbox { get; set; }

    public required long LastUpdated { get; set; }

    public required double Mileage { get; set; }

    public required string Model { get; set; }

    public required decimal Price { get; set; }

    public required string Vin { get; set; }

    public required int Year { get; set; }
  }
}