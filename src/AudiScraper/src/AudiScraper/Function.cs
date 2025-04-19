using Amazon.Lambda.Core;
using AWS.Lambda.Powertools.Logging;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AudiScraper;

public class Function
{
    private const int PageSize = 200;

    public async Task<string> FunctionHandler(ILambdaContext context)
    {
        Logger.LogInformation("Lambda invoked");

        string today = DateTime.Today.ToString(@"yyyy-MM-dd");
        string svd = $"svd=svd-${today}t15_00_00_501-13";
        string size = $"&size={PageSize}";
        string preset = "&preset=geo%3A-27.4660994_153.023588_5000_km_4000";

        List<JsonElement> allListings = [];
        int from = 0;
        int totalCount = 1;

        while (from < totalCount)
        {
            Uri endpoint = new($"https://scs.audi.de/api/v2/search/filter/auuc/en?{svd}{size}{preset}&from={from}");
            Logger.LogInformation($"Calling endpoint: {endpoint}");

            try
            {
                var (responseBody, error) = await CallEndpointLib.CallEndpoint(endpoint);
                if (error != null) return error.Message;

                using JsonDocument doc = JsonDocument.Parse(responseBody);
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("totalCount", out var totalCountElement))
                {
                    totalCount = totalCountElement.GetInt32();
                }

                if (root.TryGetProperty("vehicleBasic", out var vehicleBasic))
                {
                    foreach (var car in vehicleBasic.EnumerateArray())
                    {
                        allListings.Add(JsonDocument.Parse(car.GetRawText()).RootElement.Clone());
                    }
                }

                from += PageSize;
            }

            catch (Exception error)
            {
                Logger.LogError(error);
                return "Error occurred during recursive fetch";
            }
        }

        // Optional: store or return the whole array
        string allCarsJson = JsonSerializer.Serialize(allListings);
        await S3Lib.PutS3Object(allCarsJson);

        return "All vehicles fetched and stored.";
    }
}
