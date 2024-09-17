using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManager.Contexts;
using UserManager.Entities;
using UserManager.Middlewares;
using UserManager.Services.Contracts;
using UserManager.Services.Implementations;

namespace UserManager.Extensions;

public static class ServiceExtensions
{
    public static void AddContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
        
        serviceCollection.AddDbContext<PostgresDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }
    
    public static void AddIdentity(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddIdentity<User, Role>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<PostgresDbContext>()
            .AddDefaultTokenProviders();
        
        serviceCollection.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
            });
    }

    public static void AddMiddlewares(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<CheckUserLockoutMiddleware>();
    }
    
    public static void AddMapper(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(typeof(Program).Assembly);
    }
    
    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserService, UserService>();
    }
}