using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Core.Application.DTOs.Account;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Comment;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Infrastructure.Identity.Entities;

namespace SocialNetwork.Controllers
{
    [Authorize(Roles = "Basic")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IPostRepository _postRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;
        public CommentController(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ICommentService commentService, IPostRepository postRepository)
        {
            _postRepository = postRepository;
            _commentService = commentService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<IActionResult> AddComment(int id)
        {

            PostViewModel post = await GetPostByIdViewModel(id);
            SaveCommentViewModel model = new SaveCommentViewModel
            {
                Id = 0,
                PostId = id,
                UserId = authViewModel.Id
            };
            ViewBag.Post = post;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(SaveCommentViewModel model)
        {
            if(model.Content.IsNullOrEmpty())
            {
                ModelState.AddModelError("Content", "Debe escribir algo en el comentario.");
                return View("AddComment", model);
            }
            else
            {
                // model.UserId = authViewModel.Id;
                await _commentService.Add(model);
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });

        }

        public async Task<PostViewModel> GetPostByIdViewModel(int postId)
        {
            var postList = await _postRepository.GetAllWithIncludeAsync(new List<string> { "Comments" });

            var userDictionary = new Dictionary<string, ApplicationUser>();

            var postsViewModel = postList.Where(post => post.Id == postId)
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
                    };
                })
                .ToList();

            PostViewModel post = postsViewModel.FirstOrDefault();
            return post;

        }


    }
}
