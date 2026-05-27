using Duende.IdentityServer.Models;

namespace IdentityWorkspace.Infrastructure;

public static class Config
{
    // 1. Định nghĩa các thông tin Identity (OIDC) để trả về ID Token
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    // 2. Định nghĩa API Scopes (Quyền hạn truy cập vào Resource API ở Phase 5)
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("api.read", "Quyền đọc dữ liệu Web API"),
            new ApiScope("api.write", "Quyền ghi dữ liệu Web API")
        };

    // 3. Định nghĩa Client (Chính là ứng dụng Web API Client của chúng ta sau này)
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "interactive.client",
                ClientName = "Interactive Web App",
                
                // Sử dụng Authorization Code Flow kết hợp PKCE chuẩn bảo mật .NET 8
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                
                // Mật khẩu kết nối giữa Client và IdentityServer (được hash bảo mật)
                ClientSecrets = { new Secret("secret-key-6-nam-kinh-nghiem".Sha256()) },

                // Nơi nhận Token sau khi đăng nhập thành công (sau này test sẽ dùng)
                RedirectUris = { "https://localhost:7001/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:7001/signout-callback-oidc" },

                // Các quyền mà Client này được phép yêu cầu cấp phát
                AllowedScopes = { "openid", "profile", "api.read", "api.write" },
                
                // Cấu hình cho phép dùng Refresh Token
                AllowOfflineAccess = true,
                AccessTokenLifetime = 900, // 15 phút (Tính bằng giây)
                AbsoluteRefreshTokenLifetime = 2592000 // 30 ngày
            }
        };
}