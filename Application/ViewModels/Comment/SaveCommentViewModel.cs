using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Core.Application.ViewModels.Comment
{
    public class SaveCommentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El comentario no puede estar vacío.")]
        [DataType(DataType.Text)]
        public string Content { get; set; }
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
        [Range(1, int.MaxValue)]
        public int PostId { get; set; }
    }
}
