using IdentityWorkspace.Domain;

namespace IdentityWorkspace.Application.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByIdAsync(string userId);
    Task<ApplicationUser?> GetByUsernameAsync(string username);
    Task<bool> UpdateUserAsync(ApplicationUser user);
}