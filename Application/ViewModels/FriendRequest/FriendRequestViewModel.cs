using SocialNetwork.Core.Domain.Enum;

namespace SocialNetwork.Core.Application.ViewModels.FriendRequest
{
    public class FriendRequestViewModel
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string Sender { get; set; }
        public string ReceiverId { get; set; }
        public string Receiver { get; set; }
        public FriendRequestStatus FriendRequestStatus { get; set; }
    }
}
