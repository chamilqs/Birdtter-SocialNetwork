using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Infrastructure.Persistence.Contexts;
using SocialNetwork.Infrastructure.Persistence.Repository;

namespace SocialNetwork.Infrastucture.Persistence.Repositories
{
    public class SocialLinkRepository : GenericRepository<SocialLink>, ISocialLinkRepository
    {
        private readonly ApplicationContext _dbContext;
        public SocialLinkRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
