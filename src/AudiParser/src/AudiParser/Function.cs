using Amazon.Lambda.Core;
using AWS.Lambda.Powertools.Logging;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AudiParser;

public class Function
{
    public async Task<string> FunctionHandler(ILambdaContext context)
    {
        Logger.LogInformation("Lambda invoked");

        var (s3Object, s3Error) = await S3Lib.GetS3Object("2025-02-28.json");
        return null;
    }
}
