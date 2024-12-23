using System.Net.Http.Json;
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
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{connectionString}/WorldState/Read");
        request.Headers.Add("Authorization", "Bearer " + jwt);
        var readUserResult = await HttpClient.SendAsync(request);
        if (!readUserResult.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Unable to read world state from server {connectionString}: {readUserResult.StatusCode}");
        }

        var result = await readUserResult.Content.ReadFromJsonAsync<WorldState>();
        if (result == null)
        {
            throw new Exception($"Unable to deserialize WorldState from server {connectionString}: {readUserResult.StatusCode}");
        }
        
        return result;
    }
}