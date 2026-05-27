using IdentityWorkspace.Domain;
using IdentityWorkspace.Infrastructure.Data;
using IdentityWorkspace.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityWorkspace.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnectionString = configuration["ConnectionStrings:PostgreSQLConnection"];

        // 1. Cấu hình kết nối PostgreSQL
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(dbConnectionString, b => b.MigrationsAssembly("IdentityWorkspace.Infrastructure")));

        // 2. Cấu hình ASP.NET Core Identity (Quản lý User)
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        // 3. Cấu hình Duende IdentityServer tích hợp DB Postgres
        services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;

            // Cấu hình Issuer URI nếu cần cố định endpoint
        })
        // Cấu hình lưu trữ Configuration (Clients, Resources, Scopes) vào DB
       .AddConfigurationStore(options =>
       {
           options.ConfigureDbContext = b => b.UseNpgsql(dbConnectionString, sql => sql.MigrationsAssembly("IdentityWorkspace.Infrastructure"));
       })
        // Cấu hình lưu trữ Operational Data (Tokens, Authorization Codes, Refresh Tokens) vào DB
       .AddOperationalStore(options =>
       {
           options.ConfigureDbContext = b => b.UseNpgsql(dbConnectionString, sql => sql.MigrationsAssembly("IdentityWorkspace.Infrastructure"));
           options.EnableTokenCleanup = true;
       })
       .AddAspNetIdentity<ApplicationUser>()
       .AddProfileService<ProfileService>();// Kết nối Duende với User của ASP.NET Identity

        // 4. Cấu hình Redis Distributed Cache (.NET 8)
        var redisConnectionString = configuration["RedisSettings:ConnectionString"];
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = "IdentityCache_";
        });

        return services;
    }
}