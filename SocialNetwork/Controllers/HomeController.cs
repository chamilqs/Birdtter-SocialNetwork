using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.User;
using WebApp.SocialNetwork.Middlewares;

namespace SocialNetwork.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ValidateUserSession _validateUserSession;
        private readonly UserViewModel userViewModel;

        public HomeController(ValidateUserSession validateUserSession, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _validateUserSession = validateUserSession;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }
            UserViewModel vm = await _userService.GetProfile(userViewModel.Id);
            return View("Index", vm);
        }

    }
}
