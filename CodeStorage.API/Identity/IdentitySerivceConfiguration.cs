using AspNetCore.Identity.MongoDbCore.Models;

namespace CodeStorage.API.Identity;

public static class IdentityServiceConfiguration
{
    public static void AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddMongoDbStores<ApplicationUser, ApplicationRole, string>
        (
            "mongodb://root:example@localhost:27017", "identity"
        );
    }

}   