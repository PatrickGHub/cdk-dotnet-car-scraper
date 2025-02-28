using Amazon.Lambda.Core;
using AWS.Lambda.Powertools.Logging;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AudiScraper;

public class Function
{
    public async Task<string> FunctionHandler(ILambdaContext context)
    {
        Logger.LogInformation("Lambda invoked");

        string[] models = [
            "a3sb",
            "a3limo",
            "s3sb",
            "s3limo",
            "rs3sb",
            "rs3limo",
            "a4limo",
            "s4limo",
            "rs4avant",
            "rs5sb",
            "rs5coupe",
            "rs6avant",
            "a7sb",
            "s7sb",
            "rs7sb",
            "rsq8",
            "ttcoupe",
            "ttscoupe",
            "ttrscoupe",
            "r8rwd",
            "r8spyderrwd",
            "r8performance",
            "r8spyderperformance"
        ];

        string modelsString = string.Join("%2Ccarline.", models);
        string today = DateTime.Today.ToString(@"yyyy-MM-dd");
        string svd = $"svd=svd-${today}t15_00_00_501-13";
        string size = "&size=200";
        string preset = "&preset=geo%3A-27.4660994_153.023588_5000_km_4000";
        string modelsFilter = $"$filter={modelsString}";

        Uri endpoint = new($"https://scs.audi.de/api/v2/search/filter/auuc/en?{svd}{size}{preset}{modelsFilter}");
        Logger.LogInformation($"Created API endpoint: {endpoint}");

        try
        {
            var (responseBody, error) = await CallEndpointLib.CallEndpoint(endpoint);
            if (error != null) return error.Message;

            Logger.LogInformation($"Response received: {responseBody}");
        }

        catch
        {
            return "Error occurred";
        }

        return "Invocation complete";
    }
}
