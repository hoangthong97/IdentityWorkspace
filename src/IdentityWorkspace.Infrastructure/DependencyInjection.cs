using IdentityWorkspace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityWorkspace.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Lấy Connection String và cấu hình PostgreSQL với EF Core
        var dbConnectionString = configuration.GetConnectionString("PostgreSQLConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(dbConnectionString, b => b.MigrationsAssembly("IdentityWorkspace.IdentityServer")));

        // 2. Lấy cấu hình và đăng ký Redis Distributed Cache tiêu chuẩn của .NET 8
        var redisConnectionString = configuration["RedisSettings:ConnectionString"];
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = "IdentityCache_"; // Đặt tiền tố để tránh đụng hàng key với dự án khác trên cùng 1 Redis server
        });

        return services;
    }
}