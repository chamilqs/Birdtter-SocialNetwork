using SocialNetwork.Core.Application.ViewModels.AAA_Common;

namespace SocialNetwork.Core.Application.ViewModels.Friendship
{
    public class FriendshipViewModel : UserProperties
    {
        public int FriendId { get; set; }
        public string FriendUsername { get; set; }
        public string FriendName { get; set; }
        public string FriendLastName { get; set; }
        public string FriendProfilePicture { get; set; }
    }
}
