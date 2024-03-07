using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Infrastructure.Persistence.Contexts;
using SocialNetwork.Infrastructure.Persistence.Repository;

namespace SocialNetwork.Infrastucture.Persistence.Repositories
{
    public class ReplyCommentRepository : GenericRepository<ReplyComment>, IReplyCommentRepository
    {
        private readonly ApplicationContext _dbContext;
        public ReplyCommentRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
