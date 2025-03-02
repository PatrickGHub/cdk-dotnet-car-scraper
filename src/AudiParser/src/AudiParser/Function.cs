using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using AWS.Lambda.Powertools.Logging;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AudiParser;

public class Function
{
    public async Task<string> FunctionHandler(S3Event bucketEvent, ILambdaContext context)
    {
        Logger.LogInformation(bucketEvent);
        Logger.LogInformation("Lambda invoked");

        var bucketEventObject = bucketEvent.Records[0].S3.Object.Key;

        var (s3Object, s3Error) = await S3Lib.GetS3Object(bucketEventObject);
        return null;
    }
}
