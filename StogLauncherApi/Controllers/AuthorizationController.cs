using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StogShared.Authorization;
using StogShared.Entities;

namespace StogLauncherApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorizationController : ControllerBase
{
    private ApplicationContext _applicationContext;
    private ILogger _logger;

    public AuthorizationController(ApplicationContext applicationContext, ILogger<AuthorizationController> logger)
    {
        _applicationContext = applicationContext;
        _logger = logger;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IResult> RegisterUser(string username, string password)
    {
        if (_applicationContext.Users.Any(u => u.Username == username))
        {
            return Results.BadRequest("Username is already taken");
        }
        else
        {
            _applicationContext.Users.Add(new DbPlayer(username, password));
            await _applicationContext.SaveChangesAsync();
            return await Authorize(username, password);
        }
    }
    
    [HttpGet]
    [Route("Authorize")]
    public async Task<IResult> Authorize(string username, string password)
    {
        _logger.LogInformation("Authorizing user {Username}", username);
        try
        {
            var user = await _applicationContext.Users.SingleAsync(
                u => u.Username == username && u.Password == password);
            var claims = new List<Claim> {new Claim(ClaimTypes.Name, username) };
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
            return Results.Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
        }
        catch (InvalidOperationException e)
        {
            _logger.LogInformation("player {Username} {Password} wont be authorized because of exception: {EMessage}", username, password, e.Message);
            return Results.NotFound("Username or password is invalid");
        }
    }
    
    [HttpGet]
    [Route("ValidateJwt")]
    [Authorize]
    public IResult ValidateJwt()
    {
        return Results.Ok();
    }
}