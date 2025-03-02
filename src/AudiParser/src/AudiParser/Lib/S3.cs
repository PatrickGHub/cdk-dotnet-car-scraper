using Amazon.S3;
using Amazon.S3.Model;
using AWS.Lambda.Powertools.Logging;

public class S3Lib
{
    static public async Task<(string? result, AmazonS3Exception? error)> GetS3Object(string objectKey)
    {
        Logger.LogInformation($"Getting S3 object: {objectKey}");

        IAmazonS3 client = new AmazonS3Client();

        try
        {
            var s3Request = new GetObjectRequest
            {
                BucketName = "audi-scraper-output-bucket",
                Key = objectKey
            };

            using GetObjectResponse response = await client.GetObjectAsync(s3Request);
            using Stream responseStream = response.ResponseStream;
            using StreamReader reader = new StreamReader(responseStream);
            {
                return (reader.ReadToEnd(), null);
            }
        }
        catch (AmazonS3Exception error)
        {
            Logger.LogError($"Failed to get S3 object: {objectKey}");
            return (null, error);
        }
    }
}