using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityWorkspace.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityWorkspace.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        // Thực thi Migration tự động nếu chưa chạy dưới DB
        using var scope = serviceProvider.CreateScope();
        var providers = scope.ServiceProvider;

        var appDbContext = providers.GetRequiredService<AppDbContext>();
        var configDbContext = providers.GetRequiredService<ConfigurationDbContext>();
        var operationalDbContext = providers.GetRequiredService<PersistedGrantDbContext>();

        await appDbContext.Database.MigrateAsync();
        await configDbContext.Database.MigrateAsync();
        await operationalDbContext.Database.MigrateAsync();

        // 1. Seed Dữ liệu cấu hình Duende (Clients, Scopes, Resources)
        if (!await configDbContext.Clients.AnyAsync())
        {
            foreach (var client in Config.Clients)
            {
                await configDbContext.Clients.AddAsync(client.ToEntity());
            }
            await configDbContext.SaveChangesAsync();
        }

        if (!await configDbContext.IdentityResources.AnyAsync())
        {
            foreach (var resource in Config.IdentityResources)
            {
                await configDbContext.IdentityResources.AddAsync(resource.ToEntity());
            }
            await configDbContext.SaveChangesAsync();
        }

        if (!await configDbContext.ApiScopes.AnyAsync())
        {
            foreach (var scopeItem in Config.ApiScopes)
            {
                await configDbContext.ApiScopes.AddAsync(scopeItem.ToEntity());
            }
            await configDbContext.SaveChangesAsync();
        }

        // 2. Seed Dữ liệu User mẫu bằng UserManager
        var userManager = providers.GetRequiredService<UserManager<ApplicationUser>>();
        if (!await appDbContext.Users.AnyAsync())
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "thongchu97",
                Email = "hoangthong97.th@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0123456789"
            };

            var result = await userManager.CreateAsync(defaultUser, "Admin@123");
            if (result.Succeeded)
            {
                // Anh có thể add thêm claims mặc định cho user tại đây nếu muốn
            }
        }
    }
}