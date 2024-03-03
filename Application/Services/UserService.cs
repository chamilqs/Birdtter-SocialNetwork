using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetwork.Core.Application.DTOs.Email;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Friendship;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Application.ViewModels.SocialLink;
using SocialNetwork.Core.Application.ViewModels.User;
using SocialNetwork.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Services
{
    public class UserService : GenericService<SaveUserViewModel, UserViewModel, User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;

        public UserService(IUserRepository userRepository, IEmailService emailService, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(userRepository, mapper)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public async Task<UserViewModel> Login(LoginViewModel vm)
        {         
            User user = await _userRepository.LoginAsync(vm);

            if (user == null)
            {
                return null;
            }

            UserViewModel userVm = _mapper.Map<UserViewModel>(user);     
            return userVm;
        }     

        public override async Task<SaveUserViewModel> Add(SaveUserViewModel vm)
        {
            vm.EmailConfirmed = false;
            vm.IsActive = false;
            SaveUserViewModel userVm = await base.Add(vm);

            if (userVm == null)
            {
                return null;
            }

            await _emailService.SendAsync(new EmailRequest
            {
                To = userVm.Email,
                From = _emailService.MailSettings.EmailFrom,
                Body = @$"

                    <h2><strong>Gracias por registrarte en nuestra nueva red social.</strong></h2><br>
                    <p>Se ha creado el usuario: {userVm.Username}</p>
                    
                ",
                Subject = "¡Bienvenido a SocialToday!"
            });

            return userVm;
        }

        public override Task<SaveUserViewModel> GetByIdSaveViewModel(int id)
        {
            return base.GetByIdSaveViewModel(id);   
        }

        public async Task<UserViewModel> GetProfileWithLinks(int id)
        {
            var userList = await _userRepository.GetAllWithIncludeAsync(new List<string> { "SocialLinks" });

            var user = userList.FirstOrDefault(resultado => resultado.Id == id);

            if (user == null)
            {
                return null;
            }

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Address = user.Address,
                Phone = user.Phone,
                IsActive = user.IsActive,
                ProfilePicture = user.ProfilePicture,

                YouTube = user.SocialLinks?.YouTube ?? "",
                Facebook = user.SocialLinks?.Facebook ?? "",
                Twitter = user.SocialLinks?.Twitter ?? "",
                Instagram = user.SocialLinks?.Instagram ?? "",
                LinkedIn = user.SocialLinks?.LinkedIn ?? "",
                GitHub = user.SocialLinks?.GitHub ?? "",
                Website = user.SocialLinks?.Website ?? ""
            };

            return userViewModel;
        }

        public async Task<UserViewModel> GetProfile(int id)
        {
            User user = await _userRepository.GetByIdAsync(id);
            UserViewModel userVm = _mapper.Map<UserViewModel>(user);
            return userVm;
        }
    }
}
