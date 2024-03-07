using SocialNetwork.Core.Application.ViewModels.Friendship;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface IFriendshipService
    {
        Task<SaveFriendshipViewModel> AddFriendship(SaveFriendshipViewModel vm, string friendId);
    }
}
