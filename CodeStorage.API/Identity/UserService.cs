using System.Security.Claims;
using CodeStorage.Domain.User;

namespace CodeStorage.API.Identity;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserId()
    {
        var result = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
        if (result == null)
            throw new Exception("User not logged in");

        return result;
    }
}