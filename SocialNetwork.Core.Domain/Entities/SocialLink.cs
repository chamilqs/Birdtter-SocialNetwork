using SocialNetwork.Core.Domain.Common;

namespace SocialNetwork.Core.Domain.Entities
{
    public class SocialLink : AuditableBaseEntity
    {
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Instagram { get; set; }
        public string? LinkedIn { get; set; }
        public string? YouTube { get; set; }
        public string? GitHub { get; set; }
        public string? Website { get; set; }

        // Navigation property
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
