using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface IPostService : IGenericService<SavePostViewModel, PostViewModel, Post>
    {
        Task<List<PostViewModel>> GetSignedUserPosts();
    }
}
