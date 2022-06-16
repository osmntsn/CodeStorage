
using CodeStorage.Domain.Code;
using CodeStorage.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CodeStorage.Infrastructure;

public static class InfrastructureDependencyConfiguration
{
    public static void AddInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ICodeRepository,CodeRepository>();
    }
}