using SocialNetwork.Core.Domain.Common;

namespace SocialNetwork.Core.Domain.Entities
{
    public class User : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }        
        public string Password { get; set; }         
        public string Email { get; set; }     
        public bool EmailConfirmed { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public string Phone { get; set; }
        public string? ProfilePicture { get; set; }

        // relationship
        public SocialLink? SocialLinks { get; set; }

        public ICollection<Friendship>? Friends { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<FriendRequest>? FriendRequests { get; set; }

    }
}
