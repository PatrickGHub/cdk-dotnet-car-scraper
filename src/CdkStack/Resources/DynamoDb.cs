using Amazon.CDK.AWS.DynamoDB;
using Constructs;

namespace DotnetAuCarScraper.Resources
{
    public static class DynamoDbResources
    {
        public static TableV2 CreateAudiListingsTable(Construct scope)
        {
            return new TableV2(scope, "audi-listings", new TablePropsV2
            {
                PartitionKey = new Attribute { Name = "vin", Type = AttributeType.STRING },
                SortKey = new Attribute { Name = "date", Type = AttributeType.STRING },
                TableName = "audi-listings"
            });
        }
    }
}
