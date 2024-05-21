using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Tets.Controllers;

[ApiController]
 
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

 
    [HttpGet]
    
    [Route("test")]
    public IActionResult GetAll()
    {
        return Ok(new { message = "You are authenticated!" });
    }
    
    [HttpGet]
    [Route("userinfo")]
    public IActionResult GetUserInfo()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("YourSecretKey")),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        try
        {
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var username = jwtToken.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
            // var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            // var roles = jwtToken.Claims.FirstOrDefault(c => c.Type == "realm_access")?.Value;

            var userInfo = new
            {
                Username = username,
                Email = "",
                Roles = ""
            };

            return Ok(userInfo);
        }
        catch (Exception ex)
        {
            return BadRequest("Invalid token");
        }
    }
}