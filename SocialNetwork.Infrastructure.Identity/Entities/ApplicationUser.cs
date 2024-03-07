using Microsoft.AspNetCore.Identity;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? ProfilePicture { get; set; }

    }
}
