using Amazon.S3;
using Amazon.S3.Model;
using AWS.Lambda.Powertools.Logging;
using System.Text;

public class S3Lib
{
    static public async Task<AmazonS3Exception?> PutS3Object(string data)
    {
        Logger.LogInformation("Putting S3 object");

        IAmazonS3 client = new AmazonS3Client();
        string date = DateTime.Today.ToString(@"yyyy-MM-dd");
        string objectName = $"{date}.json";

        try
        {
            var s3Request = new PutObjectRequest
            {
                BucketName = "audi-scraper-output-bucket",
                Key = objectName,
                InputStream = new MemoryStream(Encoding.UTF8.GetBytes(data))
            };

            await client.PutObjectAsync(s3Request);

            Logger.LogInformation($"Finished putting S3 object: {objectName}");
            return null;
        }

        catch (AmazonS3Exception error)
        {
            Logger.LogError($"Failed to upload {objectName}: '{error.Message}'");
            return error;
        }
    }
}