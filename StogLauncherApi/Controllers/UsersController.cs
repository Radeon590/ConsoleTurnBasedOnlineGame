using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StogLauncherApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private ApplicationContext _applicationContext;
    private ILogger _logger;

    public UsersController(ApplicationContext applicationContext, ILogger<UsersController> logger)
    {
        _applicationContext = applicationContext;
        _logger = logger;
    }

    [HttpGet]
    [Route("Read")]
    [Authorize]
    public async Task<IResult> Read()
    {
        var userNameByClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
        if (userNameByClaim == null)
        {
            _logger.LogInformation("Cant find usernameByClaim");
            return Results.NotFound("Cant find username by claim");
        }
        
        var user = await _applicationContext.Users.SingleAsync(
            u => u.Username == userNameByClaim.Value);
        if (user == null)
        {
            _logger.LogInformation("Cant find user with username {Value}", userNameByClaim.Value);
            return Results.NotFound("Cant find user");
        }

        return Results.Json(user);
    }
}