using Application.Interfaces.FileStorage;
using Application.Interfaces.Services;

using Domain.Entities.Identity;
using Domain.Repositories;

using Infrastructure.ExternalApis.Azure;
using Infrastructure.ExternalApis.Azure.Consumers;
using Infrastructure.ExternalApis.Azure.Http;
using Infrastructure.Mailing;
using Infrastructure.Repositories.Books;
using Infrastructure.Repositories.Correlations;
using Infrastructure.Repositories.CoursAssistes;
using Infrastructure.Repositories.CoursNs;
using Infrastructure.Repositories.CoursSecondaireReussis;
using Infrastructure.Repositories.Etudiants;
using Infrastructure.Repositories.GrilleProgrammes;
using Infrastructure.Repositories.MemberProgrammes;
using Infrastructure.Repositories.Members;
using Infrastructure.Repositories.Programmes;
using Infrastructure.Repositories.SessionAssistees;
using Infrastructure.Repositories.SessionEtudes;
using Infrastructure.Repositories.Users;
using Infrastructure.Services;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Persistence;

using ScottBrady91.AspNetCore.Identity;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        ConfigureInfrastructureServices(services);
        ConfigureFormsServices(services);

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        MailingInitializer.Configure(services, configuration);

        ConfigureAuthentication(services);

        return services;
    }

    private static void ConfigureAuthentication(IServiceCollection services)
    {
        services.AddIdentityCore<User>(options =>
            {
                options.Stores.MaxLengthForKeys = 128;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequiredLength = 10;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredUniqueChars = 6;
            })
            .AddRoles<Role>()
            .AddRoleManager<RoleManager<Role>>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<DashboardProgrammeDbContext>()
            .AddSignInManager<SignInManager<User>>();

        // Add and configure Argon2 password hasher
        services.AddScoped<IPasswordHasher<User>, Argon2PasswordHasher<User>>();
        services.Configure<Argon2PasswordHasherOptions>(options =>
        {
            options.Strength = Argon2HashStrength.Interactive;
        });

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options => { options.LoginPath = "/authentication/login"; })
            .AddCookie(IdentityConstants.TwoFactorRememberMeScheme, o =>
            {
                o.Cookie.Name = IdentityConstants.TwoFactorRememberMeScheme;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddCookie(IdentityConstants.TwoFactorUserIdScheme, o =>
            {
                o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddCookie(IdentityConstants.ApplicationScheme, o =>
            {
                o.Cookie.Name = IdentityConstants.ApplicationScheme;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });
    }

    private static void ConfigureFormsServices(IServiceCollection services)
    {
        services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
        });

        services.Configure<FormOptions>(x =>
        {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
            x.MultipartHeadersLengthLimit = int.MaxValue;
        });
    }

    private static void ConfigureInfrastructureServices(IServiceCollection services)
    {
        services.AddSingleton<IHttpContextUserService, HttpContextUserService>();

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IProgrammeRepository, ProgrammeRepository>();
        services.AddScoped<ICoursRepository, CoursRepository>();
        services.AddScoped<ICoursAssisteRepository, CoursAssisteRepository>();

        services.AddScoped<ISessionEtudeRepository, SessionEtudeRepository>();
        services.AddScoped<ISessionAssisteeRepository, SessionAssisteeRepository>();

        services.AddScoped<IEtudiantRepository, EtudiantRepository>();
        services.AddScoped<ICoursSecondaireReussiRepository, CoursSecondaireReussiRepository>();

        services.AddScoped<IGrilleProgrammeRepository, GrilleProgrammeRepository>();
        services.AddScoped<IMemberProgrammeRepository, MemberProgrammeRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IDataCorrelationRepository, DataCorrelationRepository>();

        services.AddScoped<IFileStorageApiConsumer, AzureBlobApiConsumer>();
        services.AddScoped<IAzureApiHttpClient, AzureApiHttpClient>();
        services.AddScoped<IAzureBlobWrapper, AzureBlobWrapper>();
    }
}