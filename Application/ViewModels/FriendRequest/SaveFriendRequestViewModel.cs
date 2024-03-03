using SocialNetwork.Core.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Core.Application.ViewModels.FriendRequest
{
    public class SaveFriendRequestViewModel
    {
        public int Id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario es requerido.")]
        [Required(ErrorMessage = "El ID del usuario es requerido.")]
        public int SenderId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "El ID del amigo es requerido.")]
        [Required(ErrorMessage = "El ID del amigo es requerido.")]
        public int ReceiverId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "El ID del status es requerido.")]
        [Required(ErrorMessage = "El estado de la solicitud de amistad es requerido.")]
        public FriendRequestStatus FriendRequestStatus { get; set; }
    }
}
