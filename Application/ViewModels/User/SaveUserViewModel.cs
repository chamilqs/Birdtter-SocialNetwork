using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Core.Application.ViewModels.User
{
    public class SaveUserViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe colocar un nombre.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe colocar un apellido.")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Debe colocar un nombre de usuario.")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Debe colocar un correo.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }



        [DataType(DataType.Url)]
        public string? Facebook { get; set; }
        [DataType(DataType.Url)]
        public string? Twitter { get; set; }
        [DataType(DataType.Url)]
        public string? Instagram { get; set; }
        [DataType(DataType.Url)]
        public string? LinkedIn { get; set; }
        [DataType(DataType.Url)]
        public string? YouTube { get; set; }
        [DataType(DataType.Url)]
        public string? GitHub { get; set; }
        [DataType(DataType.Url)]
        public string? Website { get; set; }



        public bool? EmailConfirmed { get; set; }
        public bool? IsActive { get; set; }

        [DataType(DataType.Text)]
        public string? Address { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coiciden")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "Debe colocar un telefono.")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public string? ProfilePicture { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }
    }
}
