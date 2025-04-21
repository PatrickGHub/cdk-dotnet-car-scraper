using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using RetrieveAudiData.Mappers;
using RetrieveAudiData.Models;
using System.Text.Json;

public class DynamoDBLib
{
    public static async Task<List<Dictionary<string, object>>> QueryListingsDate(string date)
    {
        var client = new AmazonDynamoDBClient();

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
    public static async Task<List<Dictionary<string, object>>> QueryListingsVin(string vin)
    {
        var client = new AmazonDynamoDBClient();

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
