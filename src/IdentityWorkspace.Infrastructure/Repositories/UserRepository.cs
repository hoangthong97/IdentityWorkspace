using IdentityWorkspace.Application.Interfaces;
using IdentityWorkspace.Domain;
using Microsoft.AspNetCore.Identity;

namespace IdentityWorkspace.Infrastructure.Repositories;

// Sử dụng Primary Constructor của .NET 8 để inject UserManager trực tiếp trên khai báo class
public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
{
    public async Task<ApplicationUser?> GetByIdAsync(string userId)
    {
        return await userManager.FindByIdAsync(userId);
    }

    public async Task<ApplicationUser?> GetByUsernameAsync(string username)
    {
        return await userManager.FindByNameAsync(username);
    }

    public async Task<bool> UpdateUserAsync(ApplicationUser user)
    {
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}