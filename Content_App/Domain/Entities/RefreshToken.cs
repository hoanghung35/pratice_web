namespace Content_App.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }

        public string TokenHash { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt != null;
    }
}
