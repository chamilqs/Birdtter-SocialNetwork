using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.SocialLink;
using SocialNetwork.Core.Application.ViewModels.User;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Services
{
    public class SocialLinkService : GenericService<SaveSocialLinkViewModel, SocialLinkViewModel, SocialLink>, ISocialLinkService
    {
        private readonly ISocialLinkRepository _socialLinkRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;
        public SocialLinkService(ISocialLinkRepository socialrepository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(socialrepository, mapper)
        {
            _socialLinkRepository = socialrepository;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            _mapper = mapper;
        }

        public override Task<SaveSocialLinkViewModel> Add(SaveSocialLinkViewModel vm)
        {
            vm.UserId = userViewModel.Id;
            return base.Add(vm);
        }

        public async Task<SaveSocialLinkViewModel> GetSocials()
        {
            var socials = await _socialLinkRepository.GetAllAsync();
            var userSocials = socials.FirstOrDefault(s => s.UserId == userViewModel.Id);

            if (socials == null)
            {
                return null;
            }
            SaveSocialLinkViewModel socialVm = _mapper.Map<SaveSocialLinkViewModel>(userSocials);
            return socialVm;
        }
    }
}
