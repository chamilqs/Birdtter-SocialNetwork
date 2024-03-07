using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetwork.Core.Application.DTOs.Account;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Friendship;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Services
{
    public class FriendshipService : GenericService<SaveFriendshipViewModel, FriendshipViewModel, Friendship>, IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;
        private readonly IMapper _mapper;
        public FriendshipService(IFriendshipRepository friendshiprepository, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(friendshiprepository, mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _friendshipRepository = friendshiprepository;
            _mapper = mapper;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<SaveFriendshipViewModel> AddFriendship(SaveFriendshipViewModel vm, string friendId)
        {

            vm.UserId = userViewModel.Id;
            vm.FriendId = friendId;

            return await base.Add(vm);
        }

    }
}
