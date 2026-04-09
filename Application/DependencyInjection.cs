using FluentValidation;
using GenricRepository.Application.Handlers.Roles;
using GenricRepository.Application.Handlers.Users;
using GenricRepository.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GenricRepository.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserQueryHandler, UserQueryHandler>();
        services.AddScoped<IUserCommandHandler, UserCommandHandler>();
        services.AddScoped<IRoleQueryHandler, RoleQueryHandler>();
        services.AddScoped<IRoleCommandHandler, RoleCommandHandler>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddValidatorsFromAssemblyContaining<UserService>();
        return services;
    }
}
