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

    protected abstract string BaseUrl { get; }
}