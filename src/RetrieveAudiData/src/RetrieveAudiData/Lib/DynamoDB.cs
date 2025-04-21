using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using AWS.Lambda.Powertools.Logging;

public class DynamoDBLib
{
    private static AmazonDynamoDBClient GetClient()
    {
        AmazonDynamoDBClient? client = null;

        if (client == null)
        {
            client = new AmazonDynamoDBClient();
        }

        return client;
    }

    public static async Task<List<Dictionary<string, object>>> QueryListingsDate(string date)
    {
        Logger.LogInformation($"Querying listings by date: {date}");
        var client = GetClient();

        var table = Table.LoadTable(client, "audi-listings");
        var filter = new QueryFilter("date", QueryOperator.Equal, date);

        var config = new QueryOperationConfig
        {
            Filter = filter,
            IndexName = "audi-listings-by-date"
        };

        var result = new List<Dictionary<string, object>>();
        var search = table.Query(config);

        do
        {
            var docs = await search.GetNextSetAsync();

            foreach (var doc in docs)
            {
                var attrMap = doc.ToAttributeMap();

                var flat = attrMap.ToDictionary(
                    kvp => kvp.Key,
                    kvp => ConvertAttributeValue(kvp.Value)
                );

                result.Add(flat);
            }
        }
        while (!search.IsDone);

        return result;
    }
 
    public static async Task<List<Dictionary<string, object>>> QueryListingsModel(string model)
    {
        Logger.LogInformation($"Querying listings by model: {model}");
        var client = GetClient();

        var table = Table.LoadTable(client, "audi-listings");
        var filter = new QueryFilter("symbolicCarline", QueryOperator.Equal, model);

        var config = new QueryOperationConfig
        {
            Filter = filter,
            IndexName = "audi-listings-by-model"
        };

        var result = new List<Dictionary<string, object>>();
        var search = table.Query(config);

        do
        {
            var docs = await search.GetNextSetAsync();

            foreach (var doc in docs)
            {
                var attrMap = doc.ToAttributeMap();

                var flat = attrMap.ToDictionary(
                    kvp => kvp.Key,
                    kvp => ConvertAttributeValue(kvp.Value)
                );

                result.Add(flat);
            }
        }
        while (!search.IsDone);

        return result;
    }
 
    public static async Task<List<Dictionary<string, object>>> QueryListingsVin(string vin)
    {
        Logger.LogInformation($"Querying listings by vin: {vin}");
        var client = GetClient();

        var table = Table.LoadTable(client, "audi-listings");
        var filter = new QueryFilter("vin", QueryOperator.Equal, vin);

        var config = new QueryOperationConfig
        {
            Filter = filter,
        };

        var result = new List<Dictionary<string, object>>();
        var search = table.Query(config);

        do
        {
            var docs = await search.GetNextSetAsync();

            foreach (var doc in docs)
            {
                var attrMap = doc.ToAttributeMap();

                var flat = attrMap.ToDictionary(
                    kvp => kvp.Key,
                    kvp => ConvertAttributeValue(kvp.Value)
                );

                result.Add(flat);
            }
        }
        while (!search.IsDone);

        return result;
    }

    private static object ConvertAttributeValue(AttributeValue attr)
    {
        if (attr.S != null) return attr.S;
        if (attr.N != null && double.TryParse(attr.N, out var num)) return num;
        if (attr.IsBOOLSet) return attr.BOOL;
        if (attr.M != null) return attr.M.ToDictionary(kvp => kvp.Key, kvp => ConvertAttributeValue(kvp.Value));
        if (attr.L != null) return attr.L.Select(ConvertAttributeValue).ToList();
        return null!;
    }
}
