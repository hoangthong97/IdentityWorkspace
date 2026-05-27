using IdentityWorkspace.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityWorkspace.Infrastructure.Data;

// Kế thừa từ IdentityDbContext và truyền ApplicationUser vào
public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Tất cả cấu hình mặc định của các bảng Identity sẽ được khởi tạo ở đây
    }
}