using Application;
using Application.Extensions;

using FastEndpoints;
using FastEndpoints.Swagger;

using Infrastructure;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Localization;

using Persistence;

using Serilog;

using Web.Extensions;
using Web.Features.Common;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddPersistenceServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration);

builder.Configuration.AddJsonFile("appsettings.local.json", true);
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(x =>
    {
        x.ExcludeNonFastEndpoints = true;
        x.ShortSchemaNames = true;
    });

builder.Services
    .AddControllersWithViews()
    .AddViewLocalization(opts => { opts.ResourcesPath = "Resources"; });

builder.Services.AddFlashes().AddMvc();

// Logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .ReadFrom.Configuration(builder.Configuration)
    .Filter.ByExcluding(x =>
    {
        if (x.Exception?.GetType().Name == null)
        {
            return false;
        }

        List<string> handledErrors = builder.Configuration.GetSection("HandledErrors").Get<List<string>>() ?? [];
        return handledErrors.Contains(x.Exception.GetType().Name);
    })
    .CreateLogger();
builder.Logging.AddSerilog(Log.Logger);

builder.Services.AddCors(options =>
{
    options.AddPolicy("corsDomains",
        policy =>
        {
            policy.WithOrigins(builder.Configuration.GetSection("CorsDomains")
                    .GetChildren()
                    .Select(c => c.Value)
                    .ToArray()!)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

WebApplication app = builder.Build();
await app.Services.InitializeAndSeedDatabase();

string[] supportedCultures = ["en-CA", "fr-CA"];
app.UseRequestLocalization(options =>
{
    // the order of QueryStringRequestCultureProvider and CookieRequestCultureProvider is switched,
    // so the RequestLocalizationMiddleware looks for the cultures from the cookies first, then query string.
    IRequestCultureProvider questStringCultureProvider = options.RequestCultureProviders[0];
    options.RequestCultureProviders.RemoveAt(0);
    options.RequestCultureProviders.Insert(1, questStringCultureProvider);
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

app.UseExceptionHandler(c => c.Run(async context =>
{
    IExceptionHandlerPathFeature? exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>();
    if (exceptionHandler?.Error == null)
    {
        return;
    }

    SucceededOrNotResponse responseBody = new(false, exceptionHandler.Error.ErrorObject());
    switch (exceptionHandler.Error)
    {
        case AggregateException exception:
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            responseBody =
                new SucceededOrNotResponse(false, exception.InnerExceptions.Select(x => x.ErrorObject()).ToList());
            break;
        case ValidationFailureException exception:
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            responseBody = new SucceededOrNotResponse(false, exception.ErrorObjects());
            break;
    }

    await context.Response.WriteAsJsonAsync(responseBody);
}));

app.UseStaticFiles();
app.UseRouting();
app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = "api";
});

app.UseSwaggerGen();

app.MapDefaultControllerRoute();

app.Run();