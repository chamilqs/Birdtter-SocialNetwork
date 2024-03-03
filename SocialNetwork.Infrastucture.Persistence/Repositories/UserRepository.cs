using Microsoft.EntityFrameworkCore;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.ViewModels.User;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Infrastructure.Persistence.Contexts;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Persistence.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationContext _dbContext;

        public UserRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<User> AddAsync(User entity)
        {
            entity.Password = PasswordEncryptation.ComputeSha256Hash(entity.Password);
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == entity.Username);

            if (user == null)
            {
                await base.AddAsync(entity);
                return entity;
            }
            else
            {
                return null;

            }
        }
        public override async Task UpdateAsync(User entity, int id)
        {
            if (PasswordEncryptation.IsHashed(entity.Password))
            {
                await base.UpdateAsync(entity, id);
            }
            else
            {
                entity.Password = PasswordEncryptation.ComputeSha256Hash(entity.Password);
                await base.UpdateAsync(entity, id);
            }
        }

        public async Task<User> LoginAsync(LoginViewModel loginVm)
        {
            string passwordEncrypt = PasswordEncryptation.ComputeSha256Hash(loginVm.Password);
            User user = await _dbContext.Set<User>().FirstOrDefaultAsync(user => user.Username == loginVm.Username && user.Password == passwordEncrypt);
            return user;
        }

    }
}
