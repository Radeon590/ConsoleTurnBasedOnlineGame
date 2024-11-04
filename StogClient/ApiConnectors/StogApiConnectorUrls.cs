namespace StogClient.ApiConnectors;

public class StogApiConnectorUrls
{
    public static string GetApiConnectorUrl(string connectorName)
    {
        switch (connectorName)
        {
            case "StogLauncherApiConnector":
                return "http://localhost:5153";
            default:
                throw new ArgumentException("Invalid stog connector name");
        }
    }
}