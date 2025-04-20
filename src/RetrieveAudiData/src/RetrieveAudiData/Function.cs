using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Net;
using System.Text.Json;

// Assembly attribute
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RetrieveAudiData;

public class Function
{
    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var sortBy = request.QueryStringParameters != null && request.QueryStringParameters.TryGetValue("sortBy", out var sortByValue)
            ? sortByValue
            : "default";

        var date = request.QueryStringParameters != null && request.QueryStringParameters.TryGetValue("date", out var dateValue)
            ? dateValue
            : "default";

        var items = await DynamoDBLib.QueryTodayListings(date);

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = JsonSerializer.Serialize(items),
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Access-Control-Allow-Origin", "*" }
            }
        };
    }
}
