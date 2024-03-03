using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Interfaces.Repositories
{
    public interface IFriendRequestRepository : IGenericRepository<FriendRequest>
    {
        Task<List<FriendRequest>> GetAllFriendRequestsByUserId(int userId);
    }
}
