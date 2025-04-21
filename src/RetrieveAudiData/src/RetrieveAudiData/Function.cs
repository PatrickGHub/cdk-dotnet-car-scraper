using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Net;
using System.Text.Json;

// Assembly attribute
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RetrieveAudiData;

public class Function
{
    static public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var query = request.QueryStringParameters != null && request.QueryStringParameters.TryGetValue("query", out var queryValue)
            ? queryValue
            : "default";

        var items = new List<Dictionary<string, object>>();

        switch (query)
        {
            case "date":
                var date = request.QueryStringParameters != null && request.QueryStringParameters.TryGetValue("date", out var dateValue)
                ? dateValue
                : null;

                if (date == null)
                {
                    return Lib.HandleResponse.HandleBadRequestResponse("date");
                }

                items = await DynamoDBLib.QueryListingsDate(date);
                break;
            
            case "vin":
                var vin = request.QueryStringParameters != null && request.QueryStringParameters.TryGetValue("vin", out var vinValue)
                ? vinValue
                : null;

                if (vin == null)
                {
                    return Lib.HandleResponse.HandleBadRequestResponse("vin");
                }

                items = await DynamoDBLib.QueryListingsVin(vin);
                break;

            default:
                break;
        }

        return Lib.HandleResponse.HandleGoodResponse(items);
    }
}
