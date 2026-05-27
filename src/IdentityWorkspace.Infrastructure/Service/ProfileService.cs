using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityWorkspace.Domain;
using Microsoft.AspNetCore.Identity;

namespace IdentityWorkspace.Infrastructure.Services;

// Sử dụng Primary Constructor của .NET 8 để inject UserManager gọn gàng
public class ProfileService(UserManager<ApplicationUser> userManager) : IProfileService
{
    // Hàm này quyết định những Claims nào sẽ được đưa vào bên trong JWT (Access Token)
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        // 1. Lấy UserId từ context hiện tại
        var sub = context.Subject.GetSubjectId();

        // 2. Tìm User trong Database Postgres
        var user = await userManager.FindByIdAsync(sub);
        if (user != null)
        {
            // 3. Khởi tạo danh sách Claims chuẩn cho User
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim("phone_number", user.PhoneNumber ?? string.Empty),
                
                // Anh có thể bổ sung bất kỳ Custom Claim nào ở đây
                new Claim("developer_tenure", "6_years_experience"),
                new Claim("project_target", "master_identity_server")
            };

            // 4. Nếu anh có phân quyền (Roles), lấy roles của user và add vào claim luôn
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 5. Gắn danh sách claims này vào context để Duende đóng gói thành JWT
            context.IssuedClaims.AddRange(claims);
        }
    }

    // Hàm này dùng để check xem tài khoản User có còn hoạt động (Active) hay không
    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await userManager.FindByIdAsync(sub);

        // Trả về true nếu user tồn tại và không bị khóa tài khoản
        context.IsActive = user != null;
    }
}