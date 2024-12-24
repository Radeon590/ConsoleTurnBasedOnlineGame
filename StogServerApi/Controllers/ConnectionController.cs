using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StogShared.Authorization;
using StogShared.Entities;
using StogShared.Entities.GameWorld;
using HttpRequestMessage = System.Net.Http.HttpRequestMessage;

namespace StogServerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ConnectionController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly string _launcherApiUrl;
    private readonly ILogger<ConnectionController> _logger;
    private readonly WorldState _worldState;
    
    public ConnectionController(HttpClient httpClient, IConfiguration configuration, ILogger<ConnectionController> logger, WorldState worldState)
    {
        _logger = logger;
        
        _launcherApiUrl = configuration.GetValue<string>("LauncherApiUrl");
        if (string.IsNullOrEmpty(_launcherApiUrl))
        {
            _logger.LogCritical("LauncherApiUrl is missing");
        }

        _httpClient = httpClient;
        _worldState = worldState;
    }

    [HttpGet]
    [Route("Connect")]
    public async Task<IResult> Connect(string launcherJwt)
    {
        _logger.LogInformation("Connecting to launcher API");
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{_launcherApiUrl}/Users/Read");
        request.Headers.Add("Authorization", "Bearer " + launcherJwt);
        var readUserResult = await _httpClient.SendAsync(request);
        if (!readUserResult.IsSuccessStatusCode)
        {
            return Results.Unauthorized();
        }
        
        DbPlayer? dbPlayer = await readUserResult.Content.ReadFromJsonAsync<DbPlayer>();
        if (dbPlayer == null)
        {
            _logger.LogInformation($"Failed to read user from json");
            return Results.StatusCode(400);
        }

        var player = new Player(dbPlayer);
        _worldState.Players.Add(player);
        if (_worldState.CurrentPlayerUsername is null)
        {
            _worldState.CurrentPlayer = player;
        }
        
        var claims = new List<Claim> {new Claim(ClaimTypes.Name, dbPlayer.Username) };
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        var connectionResult = new ServerConnectionResult()
        {
            WorldState = _worldState,
            Jwt = new JwtSecurityTokenHandler().WriteToken(jwt)
        };
        
        return Results.Text(JsonConvert.SerializeObject(connectionResult));
    }
}