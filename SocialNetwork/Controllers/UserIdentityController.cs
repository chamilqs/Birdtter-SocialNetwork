using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.ViewModels.User;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.DTOs.Account;
using WebApp.SocialNetwork.Middlewares;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace WebApp.SocialNetwork.Controllers
{
    public class UserIdentityController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserServiceIdentity _userService;
        private readonly AuthenticationResponse authViewModel;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserIdentityController(IUserServiceIdentity userService, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {
            return View(new LoginIdentityViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LoginIdentityViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AuthenticationResponse userVm = await _userService.LoginAsync(vm);
            if (userVm != null && userVm.HasError != true)
            {
                HttpContext.Session.Set("user", userVm);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                vm.HasError = userVm.HasError;
                vm.Error = userVm.Error;
                return View(vm);
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "UserIdentity", action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Register()
        {
            return View(new SaveIdentityUserViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Register(SaveIdentityUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            vm.ProfilePicture = UploadFile(vm.File, vm.Email);
            var origin = Request.Headers["origin"];

            RegisterResponse response = await _userService.RegisterAsync(vm, origin);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            return RedirectToRoute(new { controller = "UserIdentity", action = "Index" });
        }


        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string response = await _userService.ConfirmEmailAsync(userId, token);
            return View("ConfirmEmail", response);
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var origin = Request.Headers["origin"];
            ForgotPasswordResponse response = await _userService.ForgotPasswordAsync(vm, origin);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult ResetPassword(string token)
        {
            return View(new ResetPasswordViewModel { Token = token });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            ResetPasswordResponse response = await _userService.ResetPasswordAsync(vm);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
        
        public async Task<IActionResult> MyProfile()
        {
            ApplicationUser vm = await _userManager.FindByEmailAsync(authViewModel.Email);
            return View("Profile", vm);

        }

        public async Task<IActionResult> EditProfile()
        {
            ApplicationUser vm = await _userManager.FindByEmailAsync(authViewModel.Email);
            SaveIdentityUserViewModel svm = new()
            {
                Name = vm.Name,
                LastName = vm.LastName,
                Username = vm.UserName,
                Email = vm.Email,
                Phone = vm.PhoneNumber,
                ProfilePicture = vm.ProfilePicture
            };
            return View("EditProfile", svm);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(SaveIdentityUserViewModel vm)
        {

            ApplicationUser userVm = await _userManager.FindByIdAsync(authViewModel.Id);

            if (!ModelState.IsValid)
            {
                return View("EditProfile", vm);
            }

            if (vm.Password.IsNullOrEmpty())
            {
                vm.Password = userVm.PasswordHash;
            }
            
            if (vm.File != null)
            {
                vm.ProfilePicture = UploadFile(vm.File, authViewModel.Email, true, userVm.ProfilePicture);
            }
            else
            {
                vm.ProfilePicture = userVm.ProfilePicture;
            }

            await _userService.UpdateUserAsync(vm);
            return RedirectToRoute(new { controller = "UserIdentity", action = "MyProfile" });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private string UploadFile(IFormFile file, string email, bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode)
            {
                if (file == null)
                {
                    return imagePath;
                }
            }
            string basePath = $"/Images/Users/{email}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode)
            {
                string[] oldImagePart = imagePath.Split("/");
                string oldImagePath = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImagePath);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }
            return $"{basePath}/{fileName}";
        }

    }
}

