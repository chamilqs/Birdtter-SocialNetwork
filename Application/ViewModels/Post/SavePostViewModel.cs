using Microsoft.AspNetCore.Http;
using SocialNetwork.Core.Application.ViewModels.Comment;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Core.Application.ViewModels.Post
{
    public class SavePostViewModel
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "No puede realizar una publicación vacía.")]
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        [DataType(DataType.Url)]
        public string? MediaVideo { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

        public List<CommentViewModel>? Comments { get; set; }
    }
}
