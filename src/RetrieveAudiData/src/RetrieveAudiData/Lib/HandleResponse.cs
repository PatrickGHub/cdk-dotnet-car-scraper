using Amazon.Lambda.APIGatewayEvents;
using System.Net;
using System.Text.Json;

namespace RetrieveAudiData.Lib;

public class HandleResponse
{
	static public APIGatewayProxyResponse HandleBadRequestResponse(string input)
	{
		return new APIGatewayProxyResponse
		{
			StatusCode = (int)HttpStatusCode.BadRequest,
			Body = JsonSerializer.Serialize(new { message = $"{input} query parameter is missing" }),
			Headers = new Dictionary<string, string>
			{
				{ "Content-Type", "application/json" },
				{ "Access-Control-Allow-Origin", "*" }
			}
		};
	}

	static public APIGatewayProxyResponse HandleGoodResponse(List<Dictionary<string, object>> input)
	{
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