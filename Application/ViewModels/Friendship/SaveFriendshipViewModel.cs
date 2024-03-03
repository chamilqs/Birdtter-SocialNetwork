using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Core.Application.ViewModels.Friendship
{
    public class SaveFriendshipViewModel
    {
        public int Id { get; set; }
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
        [Range(1, int.MaxValue)]
        public int FriendId { get; set; }
    }
}
