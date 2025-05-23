using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using AWS.Lambda.Powertools.Logging;
using AudiParser.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AudiParser;

public class Function
{
    public static async Task FunctionHandler(S3Event bucketEvent, ILambdaContext context)
    {
        Logger.LogInformation(bucketEvent);
        Logger.LogInformation("Lambda invoked");
        string s3ObjectKey = bucketEvent.Records[0].S3.Object.Key;
        string dateOfS3Object = s3ObjectKey.Split(".json")[0];

        var (s3Object, s3Error) = await S3Lib.GetS3Object(s3ObjectKey);

        if (string.IsNullOrEmpty(s3Object))
        {
            Logger.LogError($"Failed to retrieve S3 object: {s3Error}");
            return;
        }

        List<Listing>? listings = JsonSerializer.Deserialize<List<Listing>>(
            s3Object,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            }
        );

        if (listings == null)
        {
            Logger.LogError("Failed to deserialize vehicle data");
            return;
        }

        await DynamoDBLib.BatchAndWriteItems(listings, dateOfS3Object);
    }
}
