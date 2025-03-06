using Amazon.DynamoDBv2;
using AWS.Lambda.Powertools.Logging;

public class DynamoDBLib
{
    static public async Task<long> BatchWriteItems(AmazonDynamoDBClient client, string listings)
    {
        Logger.LogInformation("Writing batch of items to DynamoDB table");
        var dynamoDbContext = new DynamoDBContext(client);

        var listingsBatch = dynamoDbContext.CreateBatchWrite<Listing>();
        listingsBatch.AddPutItems(listings);

        await listingsBatch.ExecuteAsync();

        return listings.Count;
    }
}