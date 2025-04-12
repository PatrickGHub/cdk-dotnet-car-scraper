using Amazon.DynamoDBv2.Model;
using AudiParser.Models;

namespace AudiParser.Mappers;

public static class ListingMapper
{
	public static Dictionary<string, AttributeValue> ToAttributeMap(Listing listing)
	{
		return new Dictionary<string, AttributeValue>
		{
			{ "audiCode", new AttributeValue { S = listing.AudiCode } },
			{ "availableFromDate", new AttributeValue { N = listing.AvailableFromDate.ToString() } },
			{ "bodyType", MapCodeWithDescription(listing.BodyType) },
			{ "carId", new AttributeValue { S = listing.CarId } },
			{ "dateOffer", new AttributeValue { N = listing.DateOffer.ToString() } },
			{ "dealer", MapDealer(listing.Dealer) },
			{ "driveType", MapCodeWithDescription(listing.DriveType) },
			{ "extColor", MapCodeWithDescription(listing.ExtColor) },
			{ "fuel", MapCodeWithDescription(listing.Fuel) },
			{ "gearType", MapCodeWithDescription(listing.GearType) },
			{ "lastChange", new AttributeValue { N = listing.LastChange.ToString() } },
			{ "mbvHandbook", MapCodeWithDescription(listing.MbvHandbook) },
			{ "modelYear", new AttributeValue { N = listing.ModelYear.ToString() } },
			{ "symbolicCarline", MapCodeWithDescription(listing.SymbolicCarline) },
			{ "symbolicCarlineGroup", MapCodeWithDescription(listing.SymbolicCarlineGroup) },
			{ "trimline", MapCodeWithDescription(listing.Trimline) },
			{ "typedPrices", new AttributeValue
				{
					L = listing.TypedPrices
						.Select(price => new AttributeValue
						{
							M = new Dictionary<string, AttributeValue>
							{
								{ "amount", new AttributeValue { N = price.Amount.ToString() } }
							}
						}).ToList()
				}
			},
			{ "used", MapUsed(listing.Used) },
			{ "vin", new AttributeValue { S = listing.Vin } },
			{ "weblink", new AttributeValue { S = listing.Weblink } }
		};
	}

	private static AttributeValue MapCodeWithDescription(CodeWithDescription code)
	{
		return new AttributeValue
		{
			M = new Dictionary<string, AttributeValue>
			{
				{ "code", new AttributeValue { S = code.Code } },
				{ "description", new AttributeValue { S = code.Description ?? "" } }
			}
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

	private static AttributeValue MapUsed(Used used)
	{
		return new AttributeValue
		{
			M = new Dictionary<string, AttributeValue>
			{
				{ "mileage", new AttributeValue { N = used.Mileage.ToString() } },
				{ "mileageUnit", new AttributeValue { S = used.MileageUnit } }
			}
		};
	}
}