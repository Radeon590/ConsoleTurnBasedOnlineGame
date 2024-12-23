using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StogShared.Entities;

namespace StogServerApi;

public class LauncherConnector : IHostedService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly string _launcherApiUrl;
    private readonly GameServerData _gameServerData;

    public LauncherConnector(HttpClient client, ILogger<LauncherConnector> logger,
        IOptions<GameServerData> gameServerData, IConfiguration configuration)
    {
        _httpClient = client;
        _logger = logger;
        
        // TODO: bind obj from configuration
        var section = configuration.GetSection("GameServerData");
        _gameServerData = new GameServerData()
        {
            ServerName = section.GetValue<string>("ServerName"),
            ServerConnectionString = section.GetValue<string>("ServerConnectionString"),
        };
            
        _launcherApiUrl = configuration.GetValue<string>("LauncherApiUrl");
        if (string.IsNullOrEmpty(_launcherApiUrl))
        {
            _logger.LogInformation("LauncherApiUrl is missing");
        }
    }

    private async Task Connect()
    {
        _logger.LogInformation($"Connecting to Launcher {_launcherApiUrl} with GameServerData {JsonConvert.SerializeObject(_gameServerData)}");
        var result = await _httpClient.PostAsJsonAsync($"{_launcherApiUrl}/GameServers/Connect", _gameServerData);
        if (!result.IsSuccessStatusCode)
        {
            _logger.LogInformation("Failed to connect to Launcher api: {ResultStatusCode}", result.StatusCode);
        }
        else
        {
            _logger.LogInformation("Successfully connected to Launcher api: {ResultStatusCode}", result.StatusCode);
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Connect();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        
    }
}