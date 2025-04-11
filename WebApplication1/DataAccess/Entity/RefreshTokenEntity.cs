using WebApplication1.Data.Entity.Identity;

namespace WebApplication1.Data.Entity
{
    public class RefreshTokenEntity
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public bool IsUsed { get; set; } = false;
        public bool IsRevoked { get; set; } = false;
        public DateTime ExpiresAt { get; set; }

        public ApplicationUser? User { get; set; }
    }
}
