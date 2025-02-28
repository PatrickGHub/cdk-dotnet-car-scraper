using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
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

            var audiScraperLambdaExecutionRole = new Role(this, "audiScraperLambdaExecutionRole", new RoleProps
            {
                AssumedBy = new ServicePrincipal("lambda.amazonaws.com"),
                RoleName = "audi-scraper-execution-role",
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
                                    Actions = ["s3:PutObject"],
                                    Resources = [audiScraperLambdaOutputBucket.BucketArn]
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
                Role = audiScraperLambdaExecutionRole,
                Timeout = Duration.Seconds(30)
            });
        }
    }
}
