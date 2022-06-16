using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;

namespace CodeStorage.API.Identity;

public static class IdentityServiceConfiguration
{
    public static void AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetValue<string>("Identity:MongoDbConnectionString");
        services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddMongoDbStores<ApplicationUser, ApplicationRole, string>
        (
            connectionString, "identity"
        ).AddDefaultTokenProviders();
    }

}   