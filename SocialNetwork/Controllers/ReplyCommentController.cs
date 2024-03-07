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
using SocialNetwork.Core.Application.ViewModels.ReplyComment;
using SocialNetwork.Infrastructure.Identity.Entities;

namespace SocialNetwork.Controllers
{
    [Authorize(Roles = "Basic")]
    public class ReplyCommentController : Controller
    {

        private readonly IReplyCommentService _replyService;
        private readonly IPostRepository _postRepository;
        private readonly IReplyCommentRepository _repliesRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;

        public ReplyCommentController(IPostRepository postRepository, UserManager<ApplicationUser> userManager, 
            IHttpContextAccessor httpContextAccessor, IReplyCommentService replyService, IReplyCommentRepository repliesRepository)
        {
            _postRepository = postRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _replyService = replyService;
            _repliesRepository = repliesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetReplies(int commentId)
        {
            var replies = await GetReplysByCommentId(commentId);
            return Json(replies);
        }

        public async Task<IActionResult> AddReply(int commentId, int postId)
        {

            PostViewModel post = await GetPostByIdViewModel(postId);
            SaveReplyCommentViewModel model = new SaveReplyCommentViewModel
            {
                Id = 0,
                CommentId = commentId,
                PostId = postId,
                UserId = authViewModel.Id
            };
            ViewBag.Post = post;
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddReply(SaveReplyCommentViewModel model)
        {
            if (model.Content.IsNullOrEmpty())
            {
                ModelState.AddModelError("Content", "Debe escribir algo en el comentario.");
                return View("AddReply", model);
            }
            else
            {
                await _replyService.Add(model);
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
        public async Task<List<ReplyCommentViewModel>> GetReplysByCommentId(int commentId)
        {
            var replyList = await _repliesRepository.GetAllAsync();

            var userDictionary = new Dictionary<string, ApplicationUser>();

            var repliesViewModel = replyList
                .Where(reply => reply.CommentId == commentId)
                .Select(reply =>
                {
                    if (!userDictionary.TryGetValue(reply.UserId, out var user))
                    {
                        user = _userManager.FindByIdAsync(reply.UserId).Result;
                        userDictionary.Add(reply.UserId, user);
                    }

                    return new ReplyCommentViewModel
                    {
                        Id = reply.Id,
                        UserId = reply.UserId,
                        UserName = user.Name,
                        UserLastName = user.LastName,
                        UserProfilePicture = user.ProfilePicture,
                        UserUsername = user.UserName,
                        Content = reply.Content,
                        CommentId = reply.CommentId,
                    };
                })
                .ToList();

            return repliesViewModel;
        }


    }
}
