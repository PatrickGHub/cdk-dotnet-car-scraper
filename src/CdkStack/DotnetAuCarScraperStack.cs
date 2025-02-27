using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace DotnetAuCarScraper
{
    public class DotnetAuCarScraperStack : Stack
    {
        internal DotnetAuCarScraperStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
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

            var audiScraperLambda = new Function(this, "AudiScraperFunction", new FunctionProps
            {
                FunctionName = "audi-scraper",
                Runtime = Runtime.DOTNET_8,
                Handler = "AudiScraper::AudiScraper.Function::FunctionHandler",
                Code = Code.FromAsset("./src/AudiScraper/src/AudiScraper", new Amazon.CDK.AWS.S3.Assets.AssetOptions
                {
                    Bundling = buildOption
                }),
                MemorySize = 128,
                Timeout = Duration.Seconds(30)
            });
        }
    }
}
