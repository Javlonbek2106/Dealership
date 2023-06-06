using Domain.Common;

namespace Domain.Entities.IdentityEntities
{
    public class RefreshTokens : BaseAuditableEntity
    {
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
