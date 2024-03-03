using SocialNetwork.Core.Application.ViewModels.SocialLink;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface ISocialLinkService : IGenericService<SaveSocialLinkViewModel, SocialLinkViewModel, SocialLink>
    {
        Task<SaveSocialLinkViewModel> GetSocials();
    }
}
