using CodeStorage.API.Identity;
using Elastic.Clients.Elasticsearch.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeStorage.API.Controllers;


public class UsersController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<ApplicationUser> Create([FromBody] CreateUserRequest user)
    {
        var applicationUser = new ApplicationUser { UserName = user.Email, Email = user.Email };
        var result = await _userManager.CreateAsync(applicationUser, user.Password);
        return applicationUser;
    }
}