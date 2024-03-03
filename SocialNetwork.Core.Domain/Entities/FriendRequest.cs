using SocialNetwork.Core.Domain.Common;
using SocialNetwork.Core.Domain.Enum;

namespace SocialNetwork.Core.Domain.Entities
{
    public class FriendRequest : AuditableBaseEntity
    {
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public FriendRequestStatus FriendRequestStatus { get; set; }
    }
}
