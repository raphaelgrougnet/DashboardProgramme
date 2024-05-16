using Application.Interfaces.Services;

using Domain.Constants.User;
using Domain.Entities;
using Domain.Entities.Identity;

using EntityFrameworkCore.SqlServer.NodaTime.Extensions;

using Infrastructure.Services;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Persistence;
using Persistence.Interceptors;

using ScottBrady91.AspNetCore.Identity;

using Tests.Common.Builders;

namespace Tests.Common.Fixtures;

public class TestFixture : IDisposable
{
    private readonly Random _random;
    public readonly DashboardProgrammeDbContext DbContext;
    public readonly RoleManager<Role> RoleManager;
    public readonly UserManager<User> UserManager;

    private Guid _anyRoleId;

    private UserBuilder _userBuilder;
    public MemberBuilder MemberBuilder;

    public TestFixture()
    {
        _userBuilder = new UserBuilder();
        MemberBuilder = new MemberBuilder();

        _random = new Random();

        // Create test database and apply migrations
        IConfigurationRoot? configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile("appsettings.local.json", true)
            .Build();

        ServiceCollection serviceCollection = [];

        serviceCollection.AddMediatR(typeof(DashboardProgrammeDbContext).Assembly);

        serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        serviceCollection.AddSingleton<IHttpContextUserService, HttpContextUserService>();
        serviceCollection.AddScoped<EntitySaveChangesInterceptor>();
        serviceCollection.AddScoped<AuditableAndSoftDeletableEntitySaveChangesInterceptor>();
        serviceCollection.AddScoped<AuditableEntitySaveChangesInterceptor>();
        serviceCollection.AddScoped<UserSaveChangesInterceptor>();

        serviceCollection.AddDbContext<DashboardProgrammeDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")!,
                optionsBuilder => optionsBuilder
                    .UseNodaTime()
                    .EnableRetryOnFailure()
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)
                    .MigrationsAssembly(typeof(DashboardProgrammeDbContext).Assembly.FullName));
        });

        DbContext = serviceCollection.BuildServiceProvider().GetService<DashboardProgrammeDbContext>()!;
        if (DbContext == null)
        {
            throw new Exception("DbContext cannot be null.");
        }

        DbContext.Database.EnsureDeleted();
        try
        {
            DbContext.Database.Migrate();
        }
        catch (SqlException ex) when (ex.Number == 1801)
        {
            // ne pas lancer d'exception si la bd est déjà migrée
        }

        serviceCollection.AddDataProtection();

        serviceCollection.AddIdentityCore<User>(options =>
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

        serviceCollection.AddScoped<IPasswordHasher<User>, Argon2PasswordHasher<User>>();
        serviceCollection.Configure<Argon2PasswordHasherOptions>(options =>
        {
            options.Strength = Argon2HashStrength.Interactive;
        });

        // Set Identity related managers
        UserManager = serviceCollection.BuildServiceProvider().GetService<UserManager<User>>()!;
        RoleManager = serviceCollection.BuildServiceProvider().GetService<RoleManager<Role>>()!;

        SeedRoles();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
        UserManager.Dispose();
        RoleManager.Dispose();
    }

    private Role BuildRole(string roleName)
    {
        return new Role { Name = roleName, NormalizedName = roleName.ToUpperInvariant() };
    }

    public Role FindRoleWithName(string roleName)
    {
        return DbContext.Roles.First(x => x.Name == roleName);
    }

    public int GenerateCrmId()
    {
        return _random.Next(200000000, 300000000);
    }

    public string GenerateEmail()
    {
        return $"john.doe.{GenerateCrmId()}@gmail.com";
    }

    public async Task<Member> GivenMember(string? email = null)
    {
        Member member = MemberBuilder.WithEmail(email ?? GenerateEmail()).Build();
        DbContext.Members.Add(member);
        await DbContext.SaveChangesAsync();
        return member;
    }

    public async Task<User> GivenUserInDatabase()
    {
        User user = _userBuilder.WithEmail(GenerateEmail()).AsNotDeleted().Build();
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        if (DbContext.UserRoles.Any(x => x.UserId == user.Id && x.RoleId == _anyRoleId))
        {
            return user;
        }

        DbContext.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = _anyRoleId });
        await DbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User> GivenUserInUserManager()
    {
        User user = _userBuilder.WithEmail(GenerateEmail()).AsNotDeleted().Build();
        await UserManager.CreateAsync(user);
        await UserManager.AddPasswordAsync(user, "Qwerty123!");

        if (DbContext.UserRoles.Any(x => x.UserId == user.Id && x.RoleId == _anyRoleId))
        {
            return user;
        }

        DbContext.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = _anyRoleId });
        await DbContext.SaveChangesAsync();
        return user;
    }

    public void ResetBuilders()
    {
        _userBuilder = new UserBuilder();
        MemberBuilder = new MemberBuilder();
    }

    private void SeedRoles()
    {
        Role role = BuildRole(Roles.ADMINISTRATOR);
        DbContext.Roles.Add(role);
        DbContext.SaveChanges();
        _anyRoleId = role.Id;
    }
}