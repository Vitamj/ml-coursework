using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Sessions;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, string systemPassword)
    {
        services.AddScoped<ISessionService>(provider =>
            new SessionService(
                provider.GetRequiredService<Abstractions.Persistence.IPersistenceContext>(),
                systemPassword));

        services.AddScoped<IAccountService, AccountService>();

        return services;
    }
}