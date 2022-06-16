using System.Security.Claims;
using CodeStorage.API.Identity;
using Elastic.Clients.Elasticsearch.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace CodeStorage.API.Controllers;


public class UsersController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public UsersController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ApplicationUser> Create([FromBody] CreateUserRequest user)
    {
        var applicationUser = new ApplicationUser { UserName = user.Email, Email = user.Email };
        var result = await _userManager.CreateAsync(applicationUser, user.Password);
        return applicationUser;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromHeader] string authorization)
    {
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(authorization.Replace("Basic ", "", true, new CultureInfo("en-GB"))));
        var model = new LoginModel { Username = decoded.Split(':')[0], Password = decoded.Split(':')[1] };
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            authClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            var token = GetToken(authClaims);

            return Ok(new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(token),
                token_type = "bearer",
                expires_in = TimeSpan.FromHours(3).TotalSeconds,
            });
        }
        return Unauthorized();
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }
}