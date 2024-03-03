using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.ViewModels.User;
using System.Threading.Tasks;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Services;
using WebApp.SocialNetwork.Middlewares;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Core.Application.ViewModels.SocialLink;
using SocialNetwork.Core.Domain.Entities;
using AutoMapper;

namespace WebApp.SocialNetwork.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISocialLinkService _socialService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly UserViewModel userViewModel;
        private readonly ValidateUserSession _validateUserSession;

        public UserController(IUserService userService,ValidateUserSession validateUserSession, IHttpContextAccessor httpContextAccessor, ISocialLinkService socialService, IMapper mapper)
        {
            _userService = userService;
            _validateUserSession = validateUserSession;
            _httpContextAccessor = httpContextAccessor;
            _socialService = socialService;
            _mapper = mapper;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public IActionResult Index()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            return View();
        }

        public async Task<IActionResult> Test()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            UserViewModel vm = await _userService.GetProfile(userViewModel.Id);
            return View("tests", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
               return View(vm);
            }
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            UserViewModel userVm = await _userService.Login(vm);
            if (userVm != null)
            {
                HttpContext.Session.Set<UserViewModel>("user", userVm);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                ModelState.AddModelError("userValidation", "Datos de acceso incorrecto");
            }

            return View(vm);
        }

        public async Task<IActionResult> MyProfile()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            UserViewModel vm = await _userService.GetProfileWithLinks(userViewModel.Id);
            if (vm != null)
            {
                return View("Profile", vm);
            }            

            UserViewModel vmNoLinks = await _userService.GetProfile(userViewModel.Id);
            return View("Profile", vmNoLinks);         

        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        public IActionResult Register()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            return View(new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            SaveUserViewModel userVm = await _userService.Add(vm);
            if (userVm == null)
            {
                ModelState.AddModelError("Validacion", "El nombre de usuario ya existe.");
                return View("Register", vm);
            } 
            else if (userVm.Id != 0 && userVm != null)
            {
                userVm.ProfilePicture = UploadFile(vm.File, userVm.Id);
                await _userService.Update(userVm, userVm.Id);
            }

            // await _userService.Add(vm);
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        public async Task<IActionResult> EditProfile()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            SaveUserViewModel vm = await GetProfileWithLinksToEdit(userViewModel.Id);
            return View("EditProfile", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(SaveUserViewModel vm)
        {
            SaveUserViewModel usuario = await _userService.GetByIdSaveViewModel(userViewModel.Id);

            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View("EditProfile", vm);
            }

            if (!vm.CurrentPassword.IsNullOrEmpty())
            {
                if (vm.Password.IsNullOrEmpty() || vm.ConfirmPassword.IsNullOrEmpty())
                {
                    ModelState.AddModelError("Validacion", "Las contraseñas no coiciden.");
                    return View("EditProfile", vm);
                }

                // aqui vamos a comparar la contraseña hash con la contraseña actual
                var verify = PasswordEncryptation.ComputeSha256Hash(vm.CurrentPassword);

                if (usuario.Password != verify)
                {
                    ModelState.AddModelError("Validacion", "La contraseña actual no coincide.");
                    return View("EditProfile", vm);
                }

            }

            // Guardamos las redes en caso de que el usuario haya agregado alguna
            SaveSocialLinkViewModel socials = await _socialService.GetSocials();
            if (socials != null)
            {
                socials.YouTube = vm.YouTube;
                socials.Facebook = vm.Facebook;
                socials.Twitter = vm.Twitter;
                socials.GitHub = vm.GitHub;
                socials.LinkedIn = vm.LinkedIn;
                socials.Instagram = vm.Instagram;

                await _socialService.Update(socials, socials.Id);
            }
            else 
            { 
                SaveSocialLinkViewModel sl = new()
                {
                    YouTube = vm.YouTube,
                    GitHub = vm.GitHub,
                    Facebook = vm.Facebook,
                    Instagram = vm.Instagram,
                    LinkedIn = vm.LinkedIn,
                    Twitter = vm.Twitter,
                    Website = vm.Website
                };

                await _socialService.Add(sl);            
            }


            if (vm.CurrentPassword.IsNullOrEmpty())
            {
                vm.Password = usuario.Password;
            }

            vm.Id = usuario.Id;
            vm.ProfilePicture = UploadFile(vm.File, vm.Id, true, usuario.ProfilePicture);
            await _userService.Update(vm, vm.Id);
            return RedirectToRoute(new { controller = "User", action = "MyProfile" });
        }

        private string UploadFile(IFormFile file, int id, bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode)
            {
                if (file == null)
                {
                    return imagePath;
                }
            }
            string basePath = $"/Images/Users/{id}";
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

        public async Task<SaveUserViewModel> GetProfileWithLinksToEdit(int id)
        {
            var user = await _userService.GetProfileWithLinks(id);
            SaveUserViewModel userVm = _mapper.Map<SaveUserViewModel>(user);

            return userVm;
        }

    }
}
