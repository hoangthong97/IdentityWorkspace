using Microsoft.EntityFrameworkCore;

namespace IdentityWorkspace.Infrastructure.Data;

// Sử dụng tính năng Primary Constructor của .NET 8 để inject gọn gàng
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Đây là nơi cấu hình Fluent API cho các bảng sau này (Phase 3)
    }
}