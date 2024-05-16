using System.Reflection;

using Application.Interfaces.Services.Books;
using Application.Interfaces.Services.Correlations;
using Application.Interfaces.Services.Gestionnaire;
using Application.Interfaces.Services.Members;
using Application.Interfaces.Services.Notifications;
using Application.Interfaces.Services.Programmes;
using Application.Interfaces.Services.SessionAssistees;
using Application.Interfaces.Services.SessionEtudes;
using Application.Interfaces.Services.Users;
using Application.Services.Books;
using Application.Services.Correlations;
using Application.Services.Gestionnaire;
using Application.Services.Members;
using Application.Services.Notifications;
using Application.Services.Programmes;
using Application.Services.SessionAssistees;
using Application.Services.SessionEtudes;
using Application.Services.Users;
using Application.Settings;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Slugify;

namespace Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.Configure<ApplicationSettings>(configuration.GetSection("Application"));

        services.AddScoped<ISlugHelper, SlugHelper>();

        services.AddScoped<IBookCreationService, BookCreationService>();
        services.AddScoped<IBookUpdateService, BookUpdateService>();

        services.AddScoped<IProgrammeCreationService, ProgrammeCreationService>();
        services.AddScoped<IProgrammeUpdateService, ProgrammeUpdateService>();

        services.AddScoped<IMemberCreationService, MemberCreationService>();
        services.AddScoped<IMemberUpdateService, MemberUpdateService>();


        services.AddScoped<ISessionEtudeCreationService, SessionEtudeCreationService>();


        services.AddScoped<IDataCorrelationService, DataCorrelationService>();

        services.AddScoped<INotificationService, EmailNotificationService>();

        services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        services.AddScoped<IAuthenticatedMemberService, AuthenticatedMemberService>();
        services.AddScoped<IImportDataService, ImportDataService>();

        services.AddScoped<ISpeQueryService, SpeQueryService>();

        services.AddScoped<IPortraitEtudiantQueryService, PortraitEtudiantQueryService>();
        
        services.AddScoped<IMemberDeletionService, MemberDeletionService>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}