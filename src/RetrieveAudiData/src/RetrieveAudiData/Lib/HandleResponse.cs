using Amazon.Lambda.APIGatewayEvents;
using AWS.Lambda.Powertools.Logging;
using System.Net;
using System.Text.Json;

namespace RetrieveAudiData.Lib;

public class HandleResponse
{
	static public APIGatewayProxyResponse HandleBadRequestResponse(string message)
	{
		Logger.LogInformation("Handling bad request response");
		return new APIGatewayProxyResponse
		{
			StatusCode = (int)HttpStatusCode.BadRequest,
			Body = JsonSerializer.Serialize(new { message }),
			Headers = new Dictionary<string, string>
			{
				{ "Content-Type", "application/json" },
				{ "Access-Control-Allow-Origin", "*" }
			}
		};
	}

	static public APIGatewayProxyResponse HandleGoodResponse(List<Dictionary<string, object>> input)
	{
		Logger.LogInformation("Handling good response");
		return new APIGatewayProxyResponse
		{
			StatusCode = (int)HttpStatusCode.OK,
			Body = JsonSerializer.Serialize(input),
			Headers = new Dictionary<string, string>
			{
				{ "Content-Type", "application/json" },
				{ "Access-Control-Allow-Origin", "*" }
			}
		};
	}
}