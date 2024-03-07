using SocialNetwork.Core.Domain.Common;

namespace SocialNetwork.Core.Domain.Entities
{
    public class ReplyComment : AuditableBaseEntity
    {
        public string Content { get; set; }
        public int CommentId { get; set; }
        public Comment? Comment { get; set; }
        public string UserId { get; set; }
    }
}
