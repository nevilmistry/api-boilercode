using GenricRepository.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GenricRepository.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        return services;
    }
}
