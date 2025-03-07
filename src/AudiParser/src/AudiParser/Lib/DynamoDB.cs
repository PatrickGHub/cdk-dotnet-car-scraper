using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AWS.Lambda.Powertools.Logging;

public class DynamoDBLib
{
    static public async Task<(int? countOfStoredItems, Exception? error)> BatchWriteItems(AmazonDynamoDBClient client, List<Listings> listings)
    {
        try
        {
            Logger.LogInformation("Writing batch of items to DynamoDB table");
            var dynamoDbContext = new DynamoDBContext(client);

            var listingsBatch = dynamoDbContext.CreateBatchWrite<Listing>();
            listingsBatch.AddPutItems(listings);

            await listingsBatch.ExecuteAsync();

            return (listings.Count, null);
        }
        catch (Exception ex)
        {
            Logger.LogError("Failed to batch write items to DynamoDB");
            return (null, ex);
        }
    }
}