using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace DotnetAuCarScraper.Resources
{
	public static class LambdaFunctions
	{
		private static readonly BundlingOptions buildOptions = new()
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

		public static Function CreateAudiScraper(Construct scope, IRole role)
		{
			return new Function(scope, "AudiScraperFunction", new FunctionProps
			{
				FunctionName = "audi-scraper",
				Handler = "AudiScraper::AudiScraper.Function::FunctionHandler",
				Code = Code.FromAsset("./src/AudiScraper/src/AudiScraper", new Amazon.CDK.AWS.S3.Assets.AssetOptions { Bundling = buildOptions }),
				Runtime = Runtime.DOTNET_8,
				Role = role,
				MemorySize = 512,
				Timeout = Duration.Seconds(30)
			});
		}

		public static Function CreateAudiParser(Construct scope, IRole role)
		{
			return new Function(scope, "AudiParserFunction", new FunctionProps
			{
				FunctionName = "audi-parser",
				Handler = "AudiParser::AudiParser.Function::FunctionHandler",
				Code = Code.FromAsset("./src/AudiParser/src/AudiParser", new Amazon.CDK.AWS.S3.Assets.AssetOptions { Bundling = buildOptions }),
				Runtime = Runtime.DOTNET_8,
				Role = role,
				MemorySize = 512,
				Timeout = Duration.Seconds(30)
			});
		}
	}
}
