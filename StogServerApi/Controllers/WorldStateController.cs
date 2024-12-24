using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StogShared.Entities.GameWorld;

namespace StogServerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WorldStateController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly WorldState _worldState;
    
    public WorldStateController(ILogger<WorldStateController> logger, WorldState worldState)
    {
        _logger = logger;
        _worldState = worldState;
    }

    [HttpGet]
    [Route("Read")]
    [Authorize]
    public async Task<IResult> Read()
    {
        return Results.Text(JsonConvert.SerializeObject(_worldState), "application/json");
    }
}