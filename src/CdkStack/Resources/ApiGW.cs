using Amazon.CDK.AWS.APIGateway;
using Constructs;

namespace DotnetAuCarScraper.Resources
{
	public static class ApiGwResources
	{
		public static RestApi CreateApiGateway(Construct scope)
		{
			var api = new RestApi(scope, "listings-api", new RestApiProps
			{
				DefaultCorsPreflightOptions = new CorsOptions
				{
					AllowOrigins = Cors.ALL_ORIGINS,
					AllowMethods = Cors.ALL_METHODS
				},
				RestApiName = "listings-api"
			});

			var apiKey = new ApiKey(scope, "listings-api-key", new ApiKeyProps
			{
				ApiKeyName = "listings-api-key",
				Enabled = true
			});

			var deployment = new Deployment(scope, $"listings-api-deployment-{System.DateTime.Now.Ticks}", new DeploymentProps
			{
					Api = api
			});

			var stage = new Amazon.CDK.AWS.APIGateway.Stage(scope, "nonprod-stage", new Amazon.CDK.AWS.APIGateway.StageProps
			{
					Deployment = deployment,
					StageName = "nonprod"
			});

			api.DeploymentStage = stage;

			var usagePlan = new UsagePlan(scope, "listings-api-usage-plan", new UsagePlanProps
			{
				Name = "listings-api-usage-plan",
				ApiStages = new[]
				{
					new UsagePlanPerApiStage
					{
						Api = api,
						Stage = api.DeploymentStage
					}
				},
				Throttle = new ThrottleSettings
				{
					RateLimit = 50,
					BurstLimit = 20
				},
				Quota = new QuotaSettings
				{
					Limit = 1000,
					Period = Period.WEEK
				}
			});

			usagePlan.AddApiKey(apiKey);

			return api;
		}
	}
}