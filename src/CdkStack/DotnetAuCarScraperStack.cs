using System.Collections.Generic;
using System.Data;
using Amazon.CDK;
using Amazon.CDK.AWS.ApplicationAutoScaling;
using Amazon.CDK.AWS.Config;
using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.Events.Targets;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.S3.Notifications;
using Constructs;

namespace DotnetAuCarScraper
{
    public class DotnetAuCarScraperStack : Stack
    {
        internal DotnetAuCarScraperStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var audiScraperLambdaOutputBucket = new Bucket(this, "AudiScraperLambdaOutputBucket", new BucketProps
            {
                BlockPublicAccess = BlockPublicAccess.BLOCK_ALL,
                BucketName = "audi-scraper-output-bucket",
                Versioned = true,
            }
            );

            var buildOption = new BundlingOptions()
            {
                Image = Runtime.DOTNET_8.BundlingImage,
                User = "root",
                OutputType = BundlingOutput.ARCHIVED,
                Command = [
                    "/bin/sh",
                    "-c",
                    " dotnet tool install -g Amazon.Lambda.Tools"+
                    " && dotnet build"+
                    " && dotnet lambda package --output-package /asset-output/function.zip"
                ]
            };

            var audiLambdaExecutionRole = new Role(this, "audiLambdaExecutionRole", new RoleProps
            {
                AssumedBy = new ServicePrincipal("lambda.amazonaws.com"),
                RoleName = "audi-lambda-execution-role",
                InlinePolicies = new Dictionary<string, PolicyDocument>
                {
                    {
                        "S3PutObjectPolicy",
                        new PolicyDocument(new PolicyDocumentProps
                        {
                            Statements =
                            [
                                new PolicyStatement(new PolicyStatementProps
                                {
                                    Actions = ["s3:GetObject", "s3:PutObject"],
                                    Resources = [$"{audiScraperLambdaOutputBucket.BucketArn}/*"]
                                })
                            ]
                        })
                    },
                    {
                        "CloudWatchLogsPolicy",
                        new PolicyDocument(new PolicyDocumentProps
                        {
                            Statements =
                            [
                                new PolicyStatement(new PolicyStatementProps
                                {
                                    Actions = ["logs:CreateLogGroup", "logs:CreateLogStream", "logs:PutLogEvents"],
                                    Resources = ["*"]
                                })
                            ]
                        })
                    }
                }
            });

            var audiScraperLambda = new Function(this, "AudiScraperFunction", new FunctionProps
            {
                FunctionName = "audi-scraper",

                Code = Code.FromAsset("./src/AudiScraper/src/AudiScraper", new Amazon.CDK.AWS.S3.Assets.AssetOptions
                {
                    Bundling = buildOption
                }),
                Handler = "AudiScraper::AudiScraper.Function::FunctionHandler",
                MemorySize = 128,
                Runtime = Runtime.DOTNET_8,
                Role = audiLambdaExecutionRole,
                Timeout = Duration.Seconds(30)
            });

            var audiParserLambda = new Function(this, "AudiParserFunction", new FunctionProps
            {
                FunctionName = "audi-parser",

                Code = Code.FromAsset("./src/AudiParser/src/AudiParser", new Amazon.CDK.AWS.S3.Assets.AssetOptions
                {
                    Bundling = buildOption
                }),
                Handler = "AudiParser::AudiParser.Function::FunctionHandler",
                MemorySize = 128,
                Runtime = Runtime.DOTNET_8,
                Role = audiLambdaExecutionRole,
                Timeout = Duration.Seconds(30)
            });

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
        }
    }
}
