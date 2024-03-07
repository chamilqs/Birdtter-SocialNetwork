using SocialNetwork.Core.Domain.Common;

namespace SocialNetwork.Core.Domain.Entities
{
    public class Friendship : AuditableBaseEntity
    {
        // usuario loggeado
        public string UserId { get; set; }
        public string FriendId { get; set; }
    }
}
