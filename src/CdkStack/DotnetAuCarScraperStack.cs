using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Events.Targets;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.S3.Notifications;
using Constructs;
using DotnetAuCarScraper.Resources;

namespace DotnetAuCarScraper
{
    public class DotnetAuCarScraperStack : Stack
    {
        internal DotnetAuCarScraperStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var audiDynamoDbTable = DynamoDbResources.CreateAudiListingsTable(this);
            audiDynamoDbTable.AddGlobalSecondaryIndex(DynamoDbResources.ByDateIndex());
            audiDynamoDbTable.AddGlobalSecondaryIndex(DynamoDbResources.ByModelIndex());

            var audiScraperLambdaOutputBucket = new Bucket(this, "AudiScraperLambdaOutputBucket", new BucketProps
            {
                BlockPublicAccess = BlockPublicAccess.BLOCK_ALL,
                BucketName = "audi-scraper-output-bucket",
                Versioned = true,
            }
            );

            var audiLambdaExecutionRole = IAMResources.CreateAudiLambdaExecutionRole(this, audiDynamoDbTable, audiScraperLambdaOutputBucket);

            var audiScraperLambda = LambdaFunctions.CreateAudiScraper(this, audiLambdaExecutionRole);
            var audiParserLambda = LambdaFunctions.CreateAudiParser(this, audiLambdaExecutionRole);
            var retrieveAudiDataLambda = LambdaFunctions.CreateRetrieveAudiData(this, audiLambdaExecutionRole);

            var audiScraperLambdaSchedule = new Amazon.CDK.AWS.Events.Rule(this, "Audi scraper lambda schedule", new Amazon.CDK.AWS.Events.RuleProps
            {
                Schedule = Amazon.CDK.AWS.Events.Schedule.Cron(new Amazon.CDK.AWS.Events.CronOptions
                {
                    Hour = "12",
                    Minute = "0"
                })
            });

            audiScraperLambdaSchedule.AddTarget(new LambdaFunction(audiScraperLambda));

            audiScraperLambdaOutputBucket.AddEventNotification(EventType.OBJECT_CREATED_PUT, new LambdaDestination(audiParserLambda));

            // var apiGw = ApiGwResources.CreateApiGateway(this);
            // var listings = apiGw.Root.AddResource("listings");
		    // listings.AddResource("audi").AddMethod("GET", new LambdaIntegration(retrieveAudiDataLambda), new MethodOptions
            // {
            //     ApiKeyRequired = true
            // });
        }
    }
}
