using SocialNetwork.Core.Application.ViewModels.Post;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface IPostService
    {
        Task<List<PostViewModel>> GetAllViewModelWithInclude();
    }
}
