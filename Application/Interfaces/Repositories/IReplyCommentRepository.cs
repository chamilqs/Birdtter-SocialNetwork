using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Interfaces.Repositories
{
    public interface IReplyCommentRepository : IGenericRepository<ReplyComment>
    {
        Task<List<ReplyComment>> GetAllByCommentId(int commentId);

    }
}
