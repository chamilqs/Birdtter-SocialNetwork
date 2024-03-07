using SocialNetwork.Core.Domain.Common;

namespace SocialNetwork.Core.Domain.Entities
{
    public class Comment : AuditableBaseEntity
    {
        public string Content { get; set; }
        public int PostId { get; set; }
        public Post? Post { get; set; }
        public string UserId { get; set; }

        public ICollection<ReplyComment>? Replies { get; set; }
    }
}
