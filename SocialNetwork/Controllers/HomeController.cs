using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.DTOs.Account;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.Services;
using SocialNetwork.Core.Application.ViewModels.Comment;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Application.ViewModels.ReplyComment;
using SocialNetwork.Core.Application.ViewModels.User;
using SocialNetwork.Infrastructure.Identity.Entities;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostRepository _postRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationResponse authViewModel;

        public HomeController(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IPostRepository postRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _postRepository = postRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Posts = await GetAllPostViewModelWithInclude();
            return View(new SavePostViewModel());
        }

        public async Task<List<PostViewModel>> GetAllPostViewModelWithInclude()
        {
            var authenticatedUserId = authViewModel.Id; 

            var postList = await _postRepository.GetAllWithIncludeAsync(new List<string> { "Comments" });

            var userDictionary = new Dictionary<string, ApplicationUser>();

            var postsViewModel = postList
                .Where(post => post.UserId == authenticatedUserId) 
                .Select(post =>
                {
                    if (!userDictionary.TryGetValue(post.UserId, out var user))
                    {
                        user = _userManager.FindByIdAsync(post.UserId).Result;
                        userDictionary.Add(post.UserId, user);
                    }

                    return new PostViewModel
                    {
                        Id = post.Id,
                        UserId = post.UserId,
                        UserName = user.Name,
                        UserLastName = user.LastName,
                        UserProfilePicture = user.ProfilePicture,
                        UserUsername = user.UserName,
                        Content = post.Content,
                        ImageUrl = post.ImageUrl,
                        MediaVideo = post.MediaVideo,
                        CreatedAt = post.Created,
                        UpdatedAt = post.LastModified,
                        Comments = post.Comments
                            .Where(comment => comment.PostId == post.Id)
                            .Select(comment =>
                            {
                                var commentUser = _userManager.FindByIdAsync(comment.UserId).Result;

                                return new CommentViewModel
                                {
                                    Id = comment.Id,
                                    Content = comment.Content,
                                    UserId = comment.UserId,
                                    UserName = commentUser.Name,
                                    UserLastName = commentUser.LastName,
                                    UserProfilePicture = commentUser.ProfilePicture,
                                    UserUsername = commentUser.UserName,
                                    PostId = comment.PostId,
                                };
                            })
                            .ToList(),
                    };
                })
                .ToList();

            return postsViewModel;
        }



    }
}
