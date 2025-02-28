using AWS.Lambda.Powertools.Logging;

public class CallEndpointLib
{
  static public async Task<(string? result, Exception? error)> CallEndpoint(Uri endpoint)
  {
    try
    {
      HttpClient client = new();
      HttpResponseMessage response = await client.GetAsync(endpoint);
      response.EnsureSuccessStatusCode();
      return (await response.Content.ReadAsStringAsync(), null);
    }

    catch (HttpRequestException e)
    {
      Logger.LogError($"Request error: {e.Message}");
      return (null, e);
    }

    catch (Exception e)
    {
      Logger.LogError($"Unexpected error: {e.Message}");
      return (null, e);
    }
  }
}
