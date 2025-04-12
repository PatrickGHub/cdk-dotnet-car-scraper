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
        var bucketEventObject = bucketEvent.Records[0].S3.Object.Key;

        var (s3Object, s3Error) = await S3Lib.GetS3Object(bucketEventObject);

        if (string.IsNullOrEmpty(s3Object))
        {
            Logger.LogError($"Failed to retrieve S3 object: {s3Error}");
            return;
        }

        VehicleData? vehicleData = JsonSerializer.Deserialize<VehicleData>(
            s3Object,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            }
        );

        if (vehicleData == null)
        {
            Logger.LogError("Failed to deserialize vehicle data");
            return;
        }

        List<Listing> listings = vehicleData.VehicleBasic;

        await DynamoDBLib.BatchAndWriteItems(listings);
    }
}
