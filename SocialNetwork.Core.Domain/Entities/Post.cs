using SocialNetwork.Core.Domain.Common;

namespace SocialNetwork.Core.Domain.Entities
{
    public class Post : AuditableBaseEntity
    {
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? MediaVideo { get; set; }
        public string UserId { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
