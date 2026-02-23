namespace Content_App.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string TokenHash { get; set;} = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokeAt { get; set; }
        public bool IsExpired => DateTime.UtcNow.AddHours(7) >= ExpiresAt;
        public bool IsRevoked => RevokeAt != null;
    }
}
