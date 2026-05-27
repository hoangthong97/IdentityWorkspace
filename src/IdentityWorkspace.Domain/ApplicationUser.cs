using Microsoft.AspNetCore.Identity;

namespace IdentityWorkspace.Domain;

public class ApplicationUser : IdentityUser
{
    // Anh có thể bổ sung các trường custom cho User ở đây sau này
    // Ví dụ: public string FirstName { get; set; } = string.Empty;
}