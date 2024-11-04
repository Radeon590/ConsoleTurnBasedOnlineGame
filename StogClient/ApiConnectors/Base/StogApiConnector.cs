namespace StogClient.ApiConnectors.Base;

public abstract class StogApiConnector
{
    private static HttpClient? _httpClient;

    public static HttpClient HttpClient
    {
        get
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
            }
            return _httpClient;
        }
    }
    
    private string? _baseUrl;

    protected string BaseUrl
    {
        get
        {
            if (_baseUrl == null)
            {
                _baseUrl = StogApiConnectorUrls.GetApiConnectorUrl(this.GetType().Name);
            }

            return _baseUrl;
        }
    }
}