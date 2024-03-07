using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetwork.Core.Application.DTOs.Account;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Services
{
    public class PostService : GenericService<SavePostViewModel, PostViewModel,Post>, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(postRepository, mapper)
        {
            _postRepository = postRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public override async Task<SavePostViewModel> Add(SavePostViewModel vm)
        {
            vm.UserId = userViewModel.Id;
            return await base.Add(vm);
        }

        public override async Task Update(SavePostViewModel vm, int id)
        {
            vm.UserId = userViewModel.Id;
            await base.Update(vm, id);
        }

        public async Task<List<PostViewModel>> GetSignedUserPosts()
        {
            var postList = await _postRepository.GetAllWithIncludeAsync(new List<string> { "Comments" });

            return postList.Where(post => post.UserId == userViewModel.Id).Select(post => new PostViewModel
            {
                Id = post.Id,

                UserId = post.UserId,
                UserName = userViewModel.Name,
                UserLastName = userViewModel.LastName,
                UserProfilePicture = userViewModel.ProfilePicture,
                UserUsername = userViewModel.UserName,

                Content = post.Content,
                ImageUrl = post.ImageUrl,
                MediaVideo = post.MediaVideo,
                CreatedAt = post.Created,
                UpdatedAt = post.LastModified,

            }).ToList();
        }
    }
}
