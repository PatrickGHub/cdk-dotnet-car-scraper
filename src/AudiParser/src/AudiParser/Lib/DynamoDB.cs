using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using AWS.Lambda.Powertools.Logging;
using AudiParser.Mappers;
using AudiParser.Models;

public class DynamoDBLib
{
    static public async Task BatchAndWriteItems(List<Listing> listings, string dateScraped)
    {
        var client = new AmazonDynamoDBClient();
        var batchSize = 25;
        int startSlice = 0;

        try
        {
            while (startSlice < listings.Count)
            {
                var sliceOfListings = listings.GetRange(startSlice, Math.Min(batchSize, listings.Count - startSlice));

                var writeTasks = sliceOfListings
                    .Select(async listing =>
                    {
                        var item = ListingMapper.ToAttributeMap(listing);
                        item["date"] = new AttributeValue { S = dateScraped };

                        var writeRequest = new WriteRequest
                        {
                            PutRequest = new PutRequest
                            {
                                Item = item
                            }
                        };

                        var batchWriteRequest = new BatchWriteItemRequest
                        {
                            RequestItems = new Dictionary<string, List<WriteRequest>>()
                            {
                                {
                                    "audi-listings",
                                    new List<WriteRequest> { writeRequest }
                                }
                            }
                        };

                        await client.BatchWriteItemAsync(batchWriteRequest);
                    })
                    .ToList();

                await Task.WhenAll(writeTasks);
                startSlice += batchSize;
            }

        }
        catch (Exception ex)
        {
            Logger.LogError("Failed to write items to table");
            Logger.LogError(ex.ToString());
            throw;
        }
    }
}
