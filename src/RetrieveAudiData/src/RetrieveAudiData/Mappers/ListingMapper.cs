using Amazon.DynamoDBv2.Model;
using RetrieveAudiData.Models;

namespace RetrieveAudiData.Mappers;

public static class ListingMapper
{
	public static Dictionary<string, AttributeValue> ToAttributeMap(Listing listing)
	{
		return new Dictionary<string, AttributeValue>
		{
			{ "audiCode", new AttributeValue { S = listing.AudiCode } },
			{ "availableFromDate", new AttributeValue { N = listing.AvailableFromDate.ToString() } },
			{ "bodyType", new AttributeValue { S = listing.BodyType.Description ?? "" } },
			{ "carId", new AttributeValue { S = listing.CarId } },
			{ "dateOffer", new AttributeValue { N = listing.DateOffer.ToString() } },
			{ "dealer", MapDealer(listing.Dealer) },
			{ "driveType", new AttributeValue { S = listing.DriveType.Description ?? "" } },
			{ "extColor", new AttributeValue { S = listing.ExtColor.Description ?? "" } },
			{ "fuel", new AttributeValue { S = listing.Fuel.Description ?? "" } },
			{ "gearType", new AttributeValue { S = listing.GearType.Description ?? "" } },
			{ "kilometres", new AttributeValue { N = listing.Used.Mileage.ToString() ?? "0" } },
			{ "lastChange", new AttributeValue { N = listing.LastChange.ToString() } },
			{ "mbvHandbook", new AttributeValue { S = string.IsNullOrWhiteSpace(listing.MbvHandbook.Description) ? "unknown" : listing.MbvHandbook.Description } },
			{ "modelYear", new AttributeValue { N = listing.ModelYear.ToString() } },
			{ "price", new AttributeValue { N = listing.TypedPrices[0].Amount.ToString() ?? "0" } },
			{ "symbolicCarline", new AttributeValue { S = listing.SymbolicCarline.Description ?? "" } },
			{ "symbolicCarlineGroup", new AttributeValue { S = listing.SymbolicCarlineGroup.Description ?? "" } },
			{ "trimline", new AttributeValue { S = listing.Trimline.Description ?? "" } },
			{ "vin", new AttributeValue { S = listing.Vin } },
			{ "weblink", new AttributeValue { S = listing.Weblink } }
		};
	}

	private static AttributeValue MapDealer(Dealer dealer)
	{
		return new AttributeValue
		{
			M = new Dictionary<string, AttributeValue>
			{
				{ "city", new AttributeValue { S = dealer.City } },
				{ "name", new AttributeValue { S = dealer.Name } },
				{ "street", new AttributeValue { S = dealer.Street } },
				{ "zipCode", new AttributeValue { S = dealer.ZipCode } }
			}
		};
	}
}