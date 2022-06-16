
using CodeStorage.Domain.Code;
using CodeStorage.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;

namespace CodeStorage.Application;

public static class ApplicationDependencyConfiguration
{
    public static void AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());
        serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
    }
}