using SocialNetwork.Core.Application.ViewModels.Comment;
using SocialNetwork.Core.Application.ViewModels.Friendship;
using SocialNetwork.Core.Application.ViewModels.Post;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Core.Application.ViewModels.User
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; } 
        public bool? EmailConfirmed { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        public string Phone { get; set; }
        public string ProfilePicture { get; set; }

        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Instagram { get; set; }
        public string? LinkedIn { get; set; }
        public string? YouTube { get; set; }
        public string? GitHub { get; set; }
        public string? Website { get; set; }

        public ICollection<FriendshipViewModel>? Friends { get; set; }
        public ICollection<PostViewModel>? Posts { get; set; }
        public ICollection<CommentViewModel>? Comments { get; set; }

    }
}
