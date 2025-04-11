using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Data;
using WebApplication1.Data.Entity;
using WebApplication1.Services.Model;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.Entity.Identity;

namespace WebApplication1.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            AppDbContext context, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _config = config;
        }

        public async Task<AuthResponseModel?> RegisterAsync(RegisterModel register)
        {
            var user = new ApplicationUser
            {
                Email = register.Email,
                UserName = register.Email,
                DisplayName = register.DisplayName
            };

            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                return null;
            }

            if (register.Role == "admin")
            {
                await _userManager.AddToRoleAsync(user, "admin");
            }
            else if (register.Role == "technician")
            {
                await _userManager.AddToRoleAsync(user, "technician");
            }

            return await GenerateAuthResponse(user);
        }

        public async Task<AuthResponseModel?> LoginAsync(LoginModel login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null) return null;

            var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
            return result.Succeeded ? await GenerateAuthResponse(user) : null;
        }

        public async Task<AuthResponseModel?> RefreshTokenAsync(RefreshRequestModel refresh)
        {
            var stored = await _context.RefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token == refresh.RefreshToken &&
                                          !t.IsUsed &&
                                          !t.IsRevoked &&
                                          t.ExpiresAt > DateTime.UtcNow);
            if (stored == null) return null;

            stored.IsUsed = true;

            var user = stored.User!;
            var newAccess = await GenerateJwt(user);
            var newRefresh = GenerateRefresh();

            _context.RefreshTokens.Add(new RefreshTokenEntity
            {
                Token = newRefresh,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            await _context.SaveChangesAsync();

            return new AuthResponseModel
            {
                Email = user.Email!,
                DisplayName = user.DisplayName ?? "",
                Roles = await _userManager.GetRolesAsync(user),
                AccessToken = newAccess,
                RefreshToken = newRefresh
            };
        }

        private async Task<AuthResponseModel> GenerateAuthResponse(ApplicationUser user)
        {
            var token = await GenerateJwt(user);
            var refresh = GenerateRefresh();

            _context.RefreshTokens.Add(new RefreshTokenEntity
            {
                Token = refresh,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            await _context.SaveChangesAsync();

            return new AuthResponseModel
            {
                Email = user.Email!,
                DisplayName = user.DisplayName ?? "",
                Roles = await _userManager.GetRolesAsync(user),
                AccessToken = token,
                RefreshToken = refresh
            };
        }

        private async Task<string> GenerateJwt(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email ?? ""),
                new(ClaimTypes.Name, user.UserName ?? ""),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.UtcNow.AddMinutes(15),
                claims: claims,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefresh()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
