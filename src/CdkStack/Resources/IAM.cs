using System.Collections.Generic;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.DynamoDB;
using Constructs;

namespace DotnetAuCarScraper.Resources
{
    public static class IAMResources
    {
        public static Role CreateAudiLambdaExecutionRole(Construct scope, TableV2 dynamoDbTable, Bucket outputBucket)
        {
            return new Role(scope, "audiLambdaExecutionRole", new RoleProps
            {
                AssumedBy = new ServicePrincipal("lambda.amazonaws.com"),
                RoleName = "audi-lambda-execution-role",
                InlinePolicies = new Dictionary<string, PolicyDocument>
                {
                    {
                        "CloudWatchLogsPolicy",
                        new PolicyDocument(new PolicyDocumentProps
                        {
                            Statements = [
                                new PolicyStatement(new PolicyStatementProps
                                {
                                    Actions = ["logs:CreateLogGroup", "logs:CreateLogStream", "logs:PutLogEvents"],
                                    Resources = ["*"]
                                })
                            ]
                        })
                    },
                    {
                        "DynamoDBPolicy",
                        new PolicyDocument(new PolicyDocumentProps
                        {
                            Statements = [
                                new PolicyStatement(new PolicyStatementProps
                                {
                                    Actions = [
                                        "dynamodb:BatchWriteItem",
                                        "dynamodb:DescribeTable",
                                        "dynamodb:PutItem",
                                        "dynamodb:Query",
                                        "dynamodb:UpdateItem",
                                    ],
                                    Resources = [
                                        dynamoDbTable.TableArn,
                                        $"{dynamoDbTable.TableArn}/*"
                                    ]
                                })
                            ]
                        })
                    },
                    {
                        "S3ObjectPolicy",
                        new PolicyDocument(new PolicyDocumentProps
                        {
                            Statements = [
                                new PolicyStatement(new PolicyStatementProps
                                {
                                    Actions = ["s3:GetObject", "s3:PutObject"],
                                    Resources = [$"{outputBucket.BucketArn}/*"]
                                })
                            ]
                        })
                    },
                }
            });
        }
    }
}
