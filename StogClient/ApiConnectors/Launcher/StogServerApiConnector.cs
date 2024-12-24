using System.Net.Http.Json;
using System.Numerics;
using Newtonsoft.Json;
using StogClient.ApiConnectors.Base;
using StogShared.Entities;
using StogShared.Entities.GameWorld;

namespace StogClient.WebApi;

public class StogServerApiConnector : StogApiConnector
{
    protected override string BaseUrl => "http://localhost:5266";

    public static async Task<ServerConnectionResult> ConnectToGameServer(string connectionString, string jwt)
    {
        Console.WriteLine($"Connecting to Game Server {connectionString}/Connection/Connect?jwt={jwt}");
        var httpResult = await HttpClient.GetAsync($"{connectionString}/Connection/Connect?launcherJwt={jwt}");
        if (!httpResult.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Unable to connect to server: {httpResult.StatusCode}");
        }

        var resultString = await httpResult.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ServerConnectionResult>(resultString);
        if (result == null)
        {
            throw new Exception($"Unable to deserialize ServerConnectionResult: {httpResult.StatusCode}");
        }

        return result;
    }

    public static async Task<WorldState> GetWorldState(string connectionString, string jwt)
    {
        var readWorldStateResult = await SendAuthorizedRequest(HttpMethod.Get,
            $"{connectionString}/WorldState/Read",
            jwt);
        if (!readWorldStateResult.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Unable to read world state from server {connectionString}: {readWorldStateResult.StatusCode}");
        }

        var resultContent = await readWorldStateResult.Content.ReadAsStringAsync();
        WorldState? result = JsonConvert.DeserializeObject<WorldState>(resultContent);
        if (result == null)
        {
            throw new Exception(
                $"Unable to deserialize WorldState from server {connectionString}: {readWorldStateResult.StatusCode}");
        }

        return result;
    }

    public static async Task<string> Attack(GameServerData serverData, 
        string sourcePlayerUsername, 
        string targetPlayerUsername, 
        int damage)
    {
        string requestString = $"{serverData.ServerConnectionString}/Actions/Attack?sourcePlayerUsername={sourcePlayerUsername}&targetPlayerUsername={targetPlayerUsername}&damage={damage}";
        return await SendActionRequest(requestString, serverData);
    }
    
    public static async Task<string> Move(GameServerData serverData, 
        string username, 
        Vector2 vector)
    {
        string requestString = $"{serverData.ServerConnectionString}/Actions/Move?username={username}&x={vector.X}&y={vector.Y}";
        return await SendActionRequest(requestString, serverData);
    }
    
    public static async Task<string> Heal(GameServerData serverData, 
        string targetPlayerUsername)
    {
        string requestString = $"{serverData.ServerConnectionString}/Actions/Heal?targetPlayerUsername={targetPlayerUsername}";
        return await SendActionRequest(requestString, serverData);
    }

    private static async Task<string> SendActionRequest(string requestString, GameServerData serverData, HttpContent? content = null)
    {
        var result = await SendAuthorizedRequest(HttpMethod.Patch, requestString, serverData.Jwt, content);
        return await result.Content.ReadAsStringAsync();
    }

    private static async Task<HttpResponseMessage> SendAuthorizedRequest(HttpMethod method, string requestString,
        string jwt, HttpContent? content = null)
    {
        using var request = new HttpRequestMessage(method, requestString);
        request.Headers.Add("Authorization", "Bearer " + jwt);
        if (content != null)
            request.Content = content; 
        return await HttpClient.SendAsync(request);
    }
}