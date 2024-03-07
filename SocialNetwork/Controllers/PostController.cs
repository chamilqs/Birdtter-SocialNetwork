using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IPostRepository _postRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;

        public PostController(IPostService posttService, IHttpContextAccessor httpContextAccessor, IPostRepository postRepository)
        {
            _postService = posttService;
            _postRepository = postRepository;
            _httpContextAccessor = httpContextAccessor;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

        }

        [HttpPost]
        public async Task<IActionResult> AddPost(SavePostViewModel vm)
        {
            if (vm.File != null)
            {
                vm.ImageUrl = UploadFile(vm.File, authViewModel.Id);
            }

            if (vm.Content == null)
            {
                var posts = await _postService.GetSignedUserPosts();
                ViewBag.Posts = posts;
                return View("~/Views/Home/Index.cshtml", vm);
            }

            if (vm.MediaVideo != null)
            {
                // ver si es un video de youtube
                if (vm.MediaVideo.StartsWith("https://www.youtube.com"))
                {
                    // cambiar la url para que sea embed y se vea en el iframe
                    vm.MediaVideo = vm.MediaVideo.Replace("watch?v=", "embed/");
                }
                else
                {
                    ModelState.AddModelError("MediaVideo", "Revise que la URL sea de YouTube y que tenga una estructura similar a: https://www.youtube.com/watch?v=A8C_GSLBDdE");
                    var posts = await _postService.GetSignedUserPosts();
                    ViewBag.Posts = posts;
                    return View("~/Views/Home/Index.cshtml", vm);
                }

            }

            if (!ModelState.IsValid)
            {
                var posts = await _postService.GetSignedUserPosts();
                ViewBag.Posts = posts;
                return View("~/Views/Home/Index.cshtml", vm);
            }

            SavePostViewModel productVm = await _postService.Add(vm);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        public async Task<IActionResult> EditPost(int id)
        {
            SavePostViewModel vm = await _postService.GetByIdSaveViewModel(id);

            return View("EditPost", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(SavePostViewModel vm)
        {
            SavePostViewModel postVm = await _postService.GetByIdSaveViewModel(vm.Id);

            if (!ModelState.IsValid)
            {
                return View("EditPost", vm);
            }

            if(vm.File != null)
            {                
                vm.ImageUrl = UploadFile(vm.File, authViewModel.Id, true, postVm.ImageUrl);
            }
            else
            {
                vm.ImageUrl = postVm.ImageUrl;
            }

            if (vm.MediaVideo != null)
            {
                // ver si es un video de youtube
                if (vm.MediaVideo.StartsWith("https://www.youtube.com"))
                {
                    // cambiar la url para que sea embed y se vea en el iframe
                    vm.MediaVideo = vm.MediaVideo.Replace("watch?v=", "embed/");
                }
                else
                {
                    ModelState.AddModelError("MediaVideo", "Revise que la URL sea de YouTube y que tenga una estructura similar a: https://www.youtube.com/watch?v=A8C_GSLBDdE");
                    var posts = await _postService.GetSignedUserPosts();
                    ViewBag.Posts = posts;
                    return View("EditPost", vm);
                }

            }

            await _postService.Update(vm, vm.Id);
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            return View(await _postService.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.Delete(id);

            string basePath = $"/Images/Posts/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (Directory.Exists(path))
            {
                DirectoryInfo directory = new(path);

                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo folder in directory.GetDirectories())
                {
                    folder.Delete(true);
                }

                Directory.Delete(path);
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });
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
            string basePath = $"/Images/Posts/{email}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //create folder if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //get file extension
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
