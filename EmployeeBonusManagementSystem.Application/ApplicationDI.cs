using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EmployeeBonusManagementSystem.Application;

public static class ApplicationDI
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}
