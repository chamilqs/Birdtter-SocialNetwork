using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Core.Application.ViewModels.Post
{
    public class SavePostViewModel
    {
        public int Id { get; set; }
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debe colocar un texto.")]
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
    }
}
