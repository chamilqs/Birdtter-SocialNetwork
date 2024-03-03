using SocialNetwork.Core.Domain.Common;

namespace SocialNetwork.Core.Domain.Entities
{
    public class Friendship : AuditableBaseEntity
    {
        public int UserId { get; set; }
        public User? User { get; set; }
        public int FriendId { get; set; }
        public User? Friend { get; set; }
    }
}
