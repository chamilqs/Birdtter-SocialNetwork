using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Core.Application.ViewModels.User
{
    public class SaveIdentityUserViewModel
    {
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

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coiciden")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Debe colocar un telefono.")]
        [DataType(DataType.Text)]
        public string Phone { get; set; }
        public string? ProfilePicture { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
