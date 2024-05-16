using Application.Interfaces;

using EntityFrameworkCore.SqlServer.NodaTime.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Persistence.Interceptors;

namespace Persistence;

public static class ConfigureServices
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        ConfigureInfrastructureServices(services);

        ConfigureDbContext(services, configuration);

        return services;
    }

    private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DashboardProgrammeDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")!,
                optionsBuilder => optionsBuilder
                    .UseNodaTime()
                    .EnableRetryOnFailure()
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)
                    .MigrationsAssembly(typeof(DashboardProgrammeDbContext).Assembly.FullName));
        });

        services.AddScoped<IDashboardProgrammeDbContext>(provider =>
            provider.GetRequiredService<DashboardProgrammeDbContext>());
        services.AddScoped<DashboardProgrammeDbContextInitializer>();
    }

    private static void ConfigureInfrastructureServices(IServiceCollection services)
    {
        services.AddScoped<AuditableAndSoftDeletableEntitySaveChangesInterceptor>();
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<EntitySaveChangesInterceptor>();
        services.AddScoped<UserSaveChangesInterceptor>();
    }

    public static async Task InitializeAndSeedDatabase(this IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        DashboardProgrammeDbContextInitializer initializer =
            scope.ServiceProvider.GetRequiredService<DashboardProgrammeDbContextInitializer>();
        await initializer.InitialiseAsync();
        await initializer.SeedAsync();
    }
}