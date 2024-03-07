using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.DTOs.Account;
using SocialNetwork.Infrastructure.Identity.Entities;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.ViewModels.Friendship;
using SocialNetwork.Core.Application.ViewModels.Comment;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SocialNetwork.Controllers
{
    [Authorize(Roles = "Basic")]
    public class FriendshipController : Controller
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IPostRepository _postRepository;
        private readonly IFriendshipService _friendshipService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;

        public FriendshipController(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IFriendshipRepository friendshipRepository, IFriendshipService friendshipService, IPostRepository postRepository)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _friendshipRepository = friendshipRepository;
            _friendshipService = friendshipService;
            _postRepository = postRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Users = await GetAllUsers();
            ViewBag.FriendsPosts = await GetPostsFromFriends2();
            return View("MyFriends", await GetFriendsByUserLogged(userViewModel.Id));
        }
        public async Task<IActionResult> AddFriends()
        {
            ViewBag.FriendsPosts = await GetPostsFromFriends2();
            return View(await GetAllUsers());
        }
        public async Task<IActionResult> FriendsPosts()
        {
            ViewBag.Users = await GetAllUsers();
            return View(await GetPostsFromFriends2());
        }

        [HttpPost]
        public async Task<IActionResult> AddFriendBySearching(string friendUsername)
        {
            var user = await _userManager.FindByNameAsync(friendUsername);
            if (user == null)
            {
                return BadRequest(new { message = "Usuario no encontrado" });
            }

            var friendships = await _friendshipRepository.GetAllAsync();

            if (friendships.Any(f => (f.UserId == userViewModel.Id && f.FriendId == user.Id) || (f.UserId == user.Id && f.FriendId == userViewModel.Id)))
            {
                return BadRequest(new { message = "Ya son amigos" });
            }

            SaveFriendshipViewModel vm = new();
            await _friendshipService.AddFriendship(vm, user.Id);
            return Ok(new { message = "Amigo añadido exitosamente" });
        }

        [HttpPost]
        public async Task<IActionResult> AddFriendship(string friendId)
        {
            SaveFriendshipViewModel vm = new();
            await _friendshipService.AddFriendship(vm, friendId);
            return Ok(new { message = "Amigo añadido exitosamente" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFriendship(string friendId)
        {
            var friendships = await _friendshipRepository.GetAllAsync();

            var id = friendships
                .Where(f => (f.UserId == userViewModel.Id && f.FriendId == friendId) || (f.UserId == friendId && f.FriendId == userViewModel.Id))
                .Select(f => f.Id)
                .FirstOrDefault(); 

            var friendship = await _friendshipRepository.GetByIdAsync(id);
            await _friendshipRepository.DeleteAsync(friendship);
            return Ok(new { message = "Amigo eliminado exitosamente" });
        }
        public async Task<List<FriendshipViewModel>> GetFriendsByUserLogged(string id)
        {
            var friendsList = await _friendshipRepository.GetAllAsync();

            var userDictionary = new Dictionary<string, ApplicationUser>();
            var friendsViewModel = friendsList
                .Where(friends => friends.UserId == id || friends.FriendId == id)
                .Select(friends =>
                {
                    string userIdToFetch = friends.UserId == id ? friends.FriendId : friends.UserId;

                    if (!userDictionary.TryGetValue(userIdToFetch, out var user))
                    {
                        user = _userManager.FindByIdAsync(userIdToFetch).Result;
                        userDictionary.Add(userIdToFetch, user);
                    }

                    return new FriendshipViewModel
                    {
                        UserId = user.Id,
                        UserUsername = user.UserName,
                        UserName = user.Name,
                        UserLastName = user.LastName,
                        UserProfilePicture = user.ProfilePicture,
                        FriendId = friends.UserId == id ? friends.FriendId : friends.UserId,
                        FriendName = user.Name,
                        FriendLastName = user.LastName,
                        FriendUsername = user.UserName,
                        FriendProfilePicture = user.ProfilePicture
                    };
                })
                .ToList();

            return friendsViewModel;
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {

            var friendsList = await _friendshipRepository.GetAllAsync();
            var friendIds = friendsList
                .Where(f => f.UserId == userViewModel.Id || f.FriendId == userViewModel.Id)
                .Select(f => f.UserId == userViewModel.Id ? f.FriendId : f.UserId)
                .ToList();

            // filtrar usuarios que no sean amigos
            var users = await _userManager.Users
                .Where(user => user.EmailConfirmed && user.Id != userViewModel.Id && !friendIds.Contains(user.Id))
                .ToListAsync();

            var usersViewModel = users.Select(user => new ApplicationUser
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                UserName = user.UserName,
                ProfilePicture = user.ProfilePicture

            }).ToList();

            return usersViewModel;
        }
        public async Task<List<PostViewModel>> GetPostsFromFriends2()
        {
            var friendsList = await _friendshipRepository.GetAllAsync();
            var friendIds = friendsList
                .Where(f => f.UserId == userViewModel.Id || f.FriendId == userViewModel.Id)
                .Select(f => f.UserId == userViewModel.Id ? f.FriendId : f.UserId)
                .ToList();

            var users = await _userManager.Users
                .Where(user => friendIds.Contains(user.Id))
                .ToListAsync();

            var userDictionary = users.ToDictionary(user => user.Id);

            var postList = await _postRepository.GetAllWithIncludeAsync(new List<string> { "Comments" });

            postList = postList
                .Where(post => friendIds.Contains(post.UserId))
                .ToList();

            var postsViewModel = postList
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
                .OrderByDescending(post => post.CreatedAt)
                .ToList();

            return postsViewModel;
        }


    }
}
